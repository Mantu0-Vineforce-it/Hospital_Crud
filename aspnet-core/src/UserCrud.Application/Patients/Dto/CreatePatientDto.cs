using System;
using System.Collections.Generic;
using UserCrud.Patients.Enums;

namespace UserCrud.Patients.Dto
{
    public class CreatePatientDto
    {
        public string PatientCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public PatientEnum Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // ✅ Multiple images
        public List<string> PhotosBase64 { get; set; } = new List<string>();
    }
}
