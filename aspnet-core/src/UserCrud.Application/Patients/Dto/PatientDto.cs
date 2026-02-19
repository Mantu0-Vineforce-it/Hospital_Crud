using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using UserCrud.Patients.Enums;

namespace UserCrud.Patients.Dto
{
    public class PatientDto : EntityDto<long>
    {
        public string PatientCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public PatientEnum Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // Base64 strings for all images
        public List<string> PhotosBase64 { get; set; } = new List<string>();

        // IDs of images already in DB (for tracking/removal)
        public List<long> ImageIds { get; set; } = new List<long>();

        // IDs of images user wants to remove
        public List<long> RemovedPhotoIds { get; set; } = new List<long>();
    }
}
