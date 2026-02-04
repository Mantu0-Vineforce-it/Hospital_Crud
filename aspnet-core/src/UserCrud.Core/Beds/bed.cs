using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Rooms;

namespace UserCrud.Beds
{
    public class bed :FullAuditedEntity<long>
    {
        public string BedNumber { get; set; }
        public bool IsOccupied { get; set; }
        public long RoomId { get; set; }

        
        [ForeignKey("RoomId")]
        public room Room { get; set; }

    }
}
