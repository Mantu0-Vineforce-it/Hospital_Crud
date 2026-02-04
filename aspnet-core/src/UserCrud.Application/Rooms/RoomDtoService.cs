using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Rooms.Dto;

namespace UserCrud.Rooms
{
    public class RoomDtoService : ApplicationService, IRoomDtoApplicationModule
    {
        private readonly IRepository<room, long> _roomRepository;

        public RoomDtoService(IRepository<room, long> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // Get all rooms
        public async Task<List<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllListAsync();
            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                TotalBeds = r.TotalBeds,
                IsActive = r.IsActive
            }).ToList();
        }

        // Get room by ID
        public async Task<RoomDto> GetRoomByIdAsync(long id)
        {
            var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                throw new Exception($"Room with id {id} not found.");
            }

            return new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                TotalBeds = room.TotalBeds,
                IsActive = room.IsActive
            };
        }

        // Create a new room
        public async Task<RoomDto> CreateRoomAsync(createRoomDto input)
        {
            var room = new room
            {
                RoomNumber = input.RoomNumber,
                RoomType = input.RoomType,
                TotalBeds = input.TotalBeds,
                IsActive = input.IsActive
            };

            var createdRoom = await _roomRepository.InsertAsync(room);

            return new RoomDto
            {
                Id = createdRoom.Id,
                RoomNumber = createdRoom.RoomNumber,
                RoomType = createdRoom.RoomType,
                TotalBeds = createdRoom.TotalBeds,
                IsActive = createdRoom.IsActive
            };
        }

        // Update an existing room
        public async Task<RoomDto> UpdateRoomAsync(updateRoomDto input)
        {
            var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == input.Id);
            if (room == null)
            {
                throw new Exception($"Room with id {input.Id} not found.");
            }

            room.RoomNumber = input.RoomNumber;
            room.RoomType = input.RoomType;
            room.TotalBeds = input.TotalBeds;
            room.IsActive = input.IsActive;

            await _roomRepository.UpdateAsync(room);

            return new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                TotalBeds = room.TotalBeds,
                IsActive = room.IsActive
            };
        }

        // Delete a room
        public async Task DeleteRoomAsync(long id)
        {
            var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                throw new Exception($"Room with id {id} not found.");
            }

            await _roomRepository.DeleteAsync(room);
        }
    }
}
