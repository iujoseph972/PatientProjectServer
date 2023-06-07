namespace PatientProject.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    [ApiController]
    [Route("[controller]")]
    
    public class PatientsController : ControllerBase
    {
        private readonly IPatientServiceApiClient _patientServiceApiClient;

        private readonly IConfiguration _configuration;




        public PatientsController(IConfiguration configuration, IPatientServiceApiClient patientServiceApiClient)
        {
            _configuration = configuration;
            _patientServiceApiClient = patientServiceApiClient;
        }

        [HttpGet]
        [Authorize] // Apply authentication to the controller
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _patientServiceApiClient.GetPatientsAsync();

            var mappedPatients = MapPatients(patients); // Map patients with unique identifiers
            return Ok(mappedPatients);
        }

        [HttpGet("{patientId}")]
        [Authorize] // Apply authentication to the controller
        public async Task<IActionResult> GetPatient(string patientId)
        {
            var patient = await _patientServiceApiClient.GetPatientAsync(patientId);
            if (patient == null)
            {
                return NotFound();
            }
            var mappedPatient = MapPatient(patient); // Map patient with a unique identifier
            return Ok(mappedPatient);
        }


        [HttpPost("/login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            //validate the credentials
            if (IsValidCredentials(email, password))
            {
                var claims = new[]
  {
            new Claim(ClaimTypes.Name, email),
           
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //var token = new JwtSecurityToken(
                //    issuer: _configuration["Jwt:Issuer"],
                //    audience: _configuration["Jwt:Audience"],
                //    claims: claims,
                //    expires: DateTime.UtcNow.AddDays(1),
                //    signingCredentials: credentials
                //);
                var tokenString = _configuration["Token:Key"].ToString(); // we are going to hard code the Token in app settings as stated being allowed in requirements

                //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized(); // Return unauthorized response if the credentials are invalid
        }

        private bool IsValidCredentials(string email, string password)
        {
            // Hardcoded credentials for demonstration purposes
            const string hardcodedEmail = "user@example.com";
            const string hardcodedPassword = "password";

            // Compare the provided credentials with the hardcoded credentials
            return email == hardcodedEmail && password == hardcodedPassword;
        }




        private IEnumerable<Patient> MapPatients(IEnumerable<Patient> patients)
        {
            var mappedPatients = new List<Patient>();
            foreach (var patient in patients)
            {
                mappedPatients.Add(new Patient
                {
                    Id = GenerateUniqueIdentifier(), // Generate a unique identifier for the patient
                    //PatientId = patient.PatientId,  //we do not want to display the patietntId provided instead we will use ID which generates a new GUID
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Gender = patient.Gender,
                    DateOfBirth = patient.DateOfBirth,
                    AddressLine1 = patient.AddressLine1,
                    AddressLine2 = patient.AddressLine2,
                    City = patient.City,
                    State = patient.State,
                    PostalCode = patient.PostalCode
                
                });
            }
            return mappedPatients;
        }

        private Patient MapPatient(Patient patient)
        {
            return new Patient
            {
                Id = GenerateUniqueIdentifier(), // Generate a unique identifier for the patient
              //  PatientId = patient.PatientId, //we do not want to display the patietntId provided instead we will use ID which generates a new GUID
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth,
                AddressLine1 = patient.AddressLine1,
                AddressLine2 = patient.AddressLine2,
                City = patient.City,
                State = patient.State,
                PostalCode = patient.PostalCode
                
            };
        }

        private string GenerateUniqueIdentifier()
        {
            return Guid.NewGuid().ToString(); // Generate a unique identifier using a GUID
        }
    }
}
