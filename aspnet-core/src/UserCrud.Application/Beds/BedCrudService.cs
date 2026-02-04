using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
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

        // Get all beds
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
            RoomType = b.Room.RoomType,
        } : null // Handle null Room case
    }).ToList();
}


        // Get bed by ID
        public async Task<BedDto> GetBedByIdAsync(int id)
        {
            var bed = await _bedRepository.FirstOrDefaultAsync(b => b.Id == id);
            if (bed == null)
            {
                throw new Exception($"Bed with id {id} not found.");
            }

            return new BedDto
            {
                Id = bed.Id,
                BedNumber = bed.BedNumber,
                IsOccupied = bed.IsOccupied,
                RoomId = bed.RoomId,
                Room = bed.Room != null ? new RoomDto
                {
                    Id = bed.Room.Id,
                    RoomType = bed.Room.RoomType,
                } : null
            };
        }

        public async Task<BedDto> CreateBedAsync(CreateBedDto input)
        {
            // Ensure room exists
            var room = await _roomRepository.FirstOrDefaultAsync(r => r.Id == input.RoomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with id {input.RoomId} not found.");
            }

            // Check if bed with the same number exists in the room
            var existingBed = await _bedRepository.FirstOrDefaultAsync(b => b.RoomId == input.RoomId && b.BedNumber == input.BedNumber);
            if (existingBed != null)
            {
                throw new InvalidOperationException($"A bed with number {input.BedNumber} already exists in this room.");
            }

            // Create new bed
            var bed = new bed
            {
                BedNumber = input.BedNumber,
                IsOccupied = input.IsOccupied,
                RoomId = input.RoomId
            };

            var createdBed = await _bedRepository.InsertAsync(bed);

            // Return DTO with room details
            return new BedDto
            {
                Id = createdBed.Id,
                BedNumber = createdBed.BedNumber,
                IsOccupied = createdBed.IsOccupied,
                RoomId = createdBed.RoomId,
                Room = new RoomDto
                {
                    Id = createdBed.Room.Id,
                    RoomType = createdBed.Room.RoomType
                }
            };
        }


        // Update an existing bed
        public async Task<BedDto> UpdateBedAsync(UpdateBedDto input)
        {
            var bed = await _bedRepository.FirstOrDefaultAsync(b => b.Id == input.Id);
            if (bed == null)
            {
                throw new Exception($"Bed with id {input.Id} not found.");
            }

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
                Room = bed.Room != null ? new RoomDto
                {
                    Id = bed.Room.Id,
                    RoomType = bed.Room.RoomType,
                } : null // Added the closing curly brace and a fallback to null
            };
        }


        // Delete a bed
        public async Task DeleteBedAsync(int id)
        {
            var bed = await _bedRepository.FirstOrDefaultAsync(b => b.Id == id);
            if (bed == null)
            {
                throw new Exception($"Bed with id {id} not found.");
            }

            await _bedRepository.DeleteAsync(bed);
        }
    }
}
