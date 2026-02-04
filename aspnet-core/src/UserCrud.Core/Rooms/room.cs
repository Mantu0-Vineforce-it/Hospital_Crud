using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Beds;
using UserCrud.Rooms.Enums;

namespace UserCrud.Rooms
{
    public class room: FullAuditedEntity<long>
    {
        public string RoomNumber { get; set; }
        public RoomEnum RoomType { get; set; }
        public int TotalBeds { get; set; }
        public bool IsActive { get; set; }
        public ICollection<bed> Beds { get; set; }
    }
}
