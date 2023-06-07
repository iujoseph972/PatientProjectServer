namespace PatientProject
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPatientServiceApiClient
    {
        Task<IEnumerable<Patient>> GetPatientsAsync();
        Task<Patient> GetPatientAsync(string patientId);
    }


}
