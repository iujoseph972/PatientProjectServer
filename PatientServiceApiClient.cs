namespace PatientProject
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class PatientServiceApiClient : IPatientServiceApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public PatientServiceApiClient(HttpClient httpClient, IOptions<PatientServiceApiOptions> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
        }

        public async Task<IEnumerable<Patient>> GetPatientsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/patients");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Patient>>(content);
        }
        //It appears the provided endpoint only returns the entire list of patients, so we will filter and return based on patientID within our server code
        //public async Task<Patient> GetPatientAsync(string patientId)
        //{
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/patients/{patientId}");
        //    response.EnsureSuccessStatusCode();
        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<Patient>(content);
        //}

        public async Task<Patient> GetPatientAsync(string patientId)
        {
            var patients = await GetPatientsAsync(); // Retrieve the list of patients

            // Find the patient by matching the patientId
            var patient = patients.FirstOrDefault(p => p.PatientId == patientId);

            return patient; // return specific patient
        }

    }

}
