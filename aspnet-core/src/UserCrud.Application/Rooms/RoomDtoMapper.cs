using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Docters;
using UserCrud.Docters.Dto;
using UserCrud.Rooms.Dto;

namespace UserCrud.Rooms
{
    public class RoomDtoMapper:Profile
    {

        public RoomDtoMapper()
        {
            CreateMap<createRoomDto, room>();
            CreateMap<updateRoomDto, room>();
            CreateMap<room, RoomDto>();
        }
    }
}
