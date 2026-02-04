using Abp.Domain.Entities.Auditing;
using UserCrud.Docters;
using UserCrud.Docters.Enums;
namespace UserCrud.Docters
{
    public class doctor: FullAuditedEntity<long>
    {
        public string DocterCode { get; set; }

        public string FullName { get; set; }
        public DoctorSpecializationEnum Specialization { get; set; }
        public string Qualification { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAvailble { get; set; }

    }
}
