namespace PatientProject
{
    public class Patient
    {
        public string Id { get; set; } // Unique identifier for the patient in your API
        public string PatientId { get; set; } // Original patient ID from the patient-service API
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
