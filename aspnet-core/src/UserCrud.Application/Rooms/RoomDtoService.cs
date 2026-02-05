using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Roles.Dto;
using UserCrud.Rooms.Dto;

namespace UserCrud.Rooms
{
    public class RoomDtoService : ApplicationService
    {
        private readonly IRepository<room, long> _roomRepository;

        public RoomDtoService(IRepository<room, long> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // ===================== GET ALL =====================
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

        // ===================== GET BY ID =====================
        public async Task<RoomDto> GetRoomByIdAsync(long id)
        {
            var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                throw new UserFriendlyException($"Room with id {id} not found.");

            return new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                TotalBeds = room.TotalBeds,
                IsActive = room.IsActive
            };
        }

        // ===================== CREATE =====================
        public async Task<RoomDto> CreateRoomAsync(createRoomDto input)
        {
            try
            {
                var validationErrors = new List<ValidationResult>();

                // Check RoomNumber duplicate
                if (await _roomRepository.FirstOrDefaultAsync(r => r.RoomNumber == input.RoomNumber) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"RoomNumber '{input.RoomNumber}' is already in use.",
                        new[] { "RoomNumber" }));
                }

                if (validationErrors.Any())
                    throw new AbpValidationException("Validation failed", validationErrors);

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
            catch (AbpValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An unexpected error occurred while creating the room.", ex);
            }
        }

        // ===================== UPDATE =====================
        public async Task<RoomDto> UpdateRoomAsync(updateRoomDto input)
        {
            try
            {
                var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == input.Id);
                if (room == null)
                    throw new UserFriendlyException($"Room with id {input.Id} not found.");

                var validationErrors = new List<ValidationResult>();

                // RoomNumber duplicate (exclude current room)
                if (await _roomRepository.FirstOrDefaultAsync(
                        r => r.RoomNumber == input.RoomNumber && r.Id != input.Id) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"RoomNumber '{input.RoomNumber}' is already in use.",
                        new[] { "RoomNumber" }));
                }

                if (validationErrors.Any())
                    throw new AbpValidationException("Validation failed", validationErrors);

                // Update room fields
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
            catch (AbpValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurred while updating the room. Please try again.", ex);
            }
        }

        // ===================== DELETE =====================
        public async Task DeleteRoomAsync(long id)
        {
            var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                throw new UserFriendlyException($"Room with id {id} not found.");

            await _roomRepository.DeleteAsync(room);
        }
    }
}
