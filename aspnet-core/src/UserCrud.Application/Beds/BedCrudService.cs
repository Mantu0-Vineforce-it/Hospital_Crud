using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Beds.Dto;
using UserCrud.Rooms;
using UserCrud.Rooms.Dto;

namespace UserCrud.Beds
{
    public class BedCrudService : ApplicationService
    {
        private readonly IRepository<bed, long> _bedRepository;
        private readonly IRepository<room, long> _roomRepository;

        public BedCrudService(IRepository<bed, long> bedRepository, IRepository<room, long> roomRepository)
        {
            _bedRepository = bedRepository;
            _roomRepository = roomRepository;
        }

        // ===================== GET ALL =====================
        public async Task<List<BedDto>> GetAllBedsAsync()
        {
            var beds = await _bedRepository.GetAllListAsync();

            return beds.Select(b => new BedDto
            {
                Id = b.Id,
                BedNumber = b.BedNumber,
                IsOccupied = b.IsOccupied,
                RoomId = b.RoomId,
                Room = b.Room != null ? new RoomDto
                {
                    Id = b.Room.Id,
                    RoomType = b.Room.RoomType
                } : null
            }).ToList();
        }

        // ===================== GET BY ID =====================
        public async Task<BedDto> GetBedByIdAsync(long id)
        {
            var bed = await _bedRepository.FirstOrDefaultAsync(b => b.Id == id);
            if (bed == null)
                throw new UserFriendlyException($"Bed with id {id} not found.");

            return new BedDto
            {
                Id = bed.Id,
                BedNumber = bed.BedNumber,
                IsOccupied = bed.IsOccupied,
                RoomId = bed.RoomId,
                Room = bed.Room != null ? new RoomDto
                {
                    Id = bed.Room.Id,
                    RoomType = bed.Room.RoomType
                } : null
            };
        }

        // ===================== CREATE =====================
        public async Task<BedDto> CreateBedAsync(CreateBedDto input)
        {
            try
            {
                var validationErrors = new List<ValidationResult>();

                // Check room exists
                var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == input.RoomId);
                if (room == null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"Room with id {input.RoomId} does not exist.",
                        new[] { "RoomId" }));
                }

                // Check duplicate BedNumber in the same room
                if (await _bedRepository.FirstOrDefaultAsync(b => b.RoomId == input.RoomId && b.BedNumber == input.BedNumber) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"BedNumber '{input.BedNumber}' is already in use in this room.",
                        new[] { "BedNumber" }));
                }

                if (validationErrors.Any())
                    throw new AbpValidationException("Validation failed", validationErrors);

                var bed = new bed
                {
                    BedNumber = input.BedNumber,
                    IsOccupied = input.IsOccupied,
                    RoomId = input.RoomId
                };

                var createdBed = await _bedRepository.InsertAsync(bed);

                return new BedDto
                {
                    Id = createdBed.Id,
                    BedNumber = createdBed.BedNumber,
                    IsOccupied = createdBed.IsOccupied,
                    RoomId = createdBed.RoomId,
                    Room = room != null ? new RoomDto
                    {
                        Id = room.Id,
                        RoomType = room.RoomType
                    } : null
                };
            }
            catch (AbpValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An unexpected error occurred while creating the bed.", ex);
            }
        }

        // ===================== UPDATE =====================
        public async Task<BedDto> UpdateBedAsync(UpdateBedDto input)
        {
            try
            {
                var bed = await _bedRepository.FirstOrDefaultAsync(b => b.Id == input.Id);
                if (bed == null)
                    throw new UserFriendlyException($"Bed with id {input.Id} not found.");

                var validationErrors = new List<ValidationResult>();

                // Check duplicate BedNumber in the same room (exclude current bed)
                if (await _bedRepository.FirstOrDefaultAsync(
                    b => b.RoomId == input.RoomId && b.BedNumber == input.BedNumber && b.Id != input.Id) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"BedNumber '{input.BedNumber}' is already in use in this room.",
                        new[] { "BedNumber" }));
                }

                // Check room exists
                var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == input.RoomId);
                if (room == null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"Room with id {input.RoomId} does not exist.",
                        new[] { "RoomId" }));
                }

                if (validationErrors.Any())
                    throw new AbpValidationException("Validation failed", validationErrors);

                // Update bed fields
                bed.BedNumber = input.BedNumber;
                bed.IsOccupied = input.IsOccupied;
                bed.RoomId = input.RoomId;

                await _bedRepository.UpdateAsync(bed);

                return new BedDto
                {
                    Id = bed.Id,
                    BedNumber = bed.BedNumber,
                    IsOccupied = bed.IsOccupied,
                    RoomId = bed.RoomId,
                    Room = room != null ? new RoomDto
                    {
                        Id = room.Id,
                        RoomType = room.RoomType
                    } : null
                };
            }
            catch (AbpValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurred while updating the bed. Please try again.", ex);
            }
        }

        // ===================== DELETE =====================
        public async Task DeleteBedAsync(long id)
        {
            var bed = await _bedRepository.FirstOrDefaultAsync(b => b.Id == id);
            if (bed == null)
                throw new UserFriendlyException($"Bed with id {id} not found.");

            await _bedRepository.DeleteAsync(bed);
        }
    }
}
