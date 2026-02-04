using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Rooms.Enums;

namespace UserCrud.Rooms.Dto
{
    public class createRoomDto:FullAuditedEntity<long>
    {

        public string RoomNumber { get; set; }
        public RoomEnum RoomType { get; set; }
        public int TotalBeds { get; set; }
        public bool IsActive { get; set; }
    }
}
