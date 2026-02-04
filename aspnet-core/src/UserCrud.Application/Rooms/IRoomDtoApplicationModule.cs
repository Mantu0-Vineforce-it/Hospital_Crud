using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCrud.Rooms
{
    public interface IRoomDtoApplicationModule
    {

        Task<List<Rooms.Dto.RoomDto>> GetAllRoomsAsync();
        Task<Rooms.Dto.RoomDto> GetRoomByIdAsync(long id);
        Task<Rooms.Dto.RoomDto> CreateRoomAsync(Rooms.Dto.createRoomDto input);
        Task<Rooms.Dto.RoomDto> UpdateRoomAsync(Rooms.Dto.updateRoomDto input);
        Task DeleteRoomAsync(long id);
    }
}
