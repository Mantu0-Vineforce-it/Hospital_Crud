using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Patients;

namespace UserCrud.PatientImages
{
    public class patienttImages :FullAuditedEntity<long>
    {

        public long PatientId { get; set; }

        [ForeignKey("PatientId")]
        public patient Patient { get; set; }

        public byte[] Image { get; set; }

        public string FileName { get; set; }
    }
}
