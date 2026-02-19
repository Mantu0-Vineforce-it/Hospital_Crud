using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.PatientImages;
using UserCrud.Patients.Dto;

namespace UserCrud.Patients
{
    public class PatientCrudService : ApplicationService
    {
        private readonly IRepository<patient, long> _patientRepository;
        private readonly IRepository<patienttImages, long> _patientImagesRepository;

        public PatientCrudService(
            IRepository<patient, long> patientRepository,
            IRepository<patienttImages, long> patientImagesRepository
        )
        {
            _patientRepository = patientRepository;
            _patientImagesRepository = patientImagesRepository;
        }

        // ===================== GET ALL =====================
        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository
                .GetAll()
                .Include(p => p.Images)
                .ToListAsync();
            return ObjectMapper.Map<List<PatientDto>>(patients);
        }


        public async Task<PatientDto> GetPatientByIdAsync(long id)
        {
            var patient = await _patientRepository
                .GetAll()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                throw new UserFriendlyException($"Patient with id {id} not found.");

            return ObjectMapper.Map<PatientDto>(patient);

        }


        // ===================== CREATE =====================
        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto input)
        {
            ValidateDuplicates(input.PatientCode, input.Email, input.PhoneNumber);

            var patient = new patient
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                PatientCode = input.PatientCode,
                Gender = input.Gender,
                Email = input.Email,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                DateOfBirth = input.DateOfBirth,
                Images = new List<patienttImages>()
            };

            // Add images
            if (input.PhotosBase64 != null && input.PhotosBase64.Any())
            {
                foreach (var base64 in input.PhotosBase64)
                {
                    patient.Images.Add(new patienttImages
                    {
                        Image = Convert.FromBase64String(base64),
                        FileName = Guid.NewGuid().ToString()
                    });
                }
            }

            await _patientRepository.InsertAsync(patient);

            return MapToDto(patient);
        }

        // ===================== UPDATE =====================
        public async Task<PatientDto> UpdatePatientAsync(UpdatePattientDto input)
        {
            var patient = await _patientRepository
                .GetAll()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == input.Id);

            if (patient == null)
                throw new UserFriendlyException($"Patient with id {input.Id} not found.");

            ValidateDuplicates(input.PatientCode, input.Email, input.PhoneNumber, input.Id);

            // Update basic fields
            patient.FirstName = input.FirstName;
            patient.LastName = input.LastName;
            patient.PatientCode = input.PatientCode;
            patient.Gender = input.Gender;
            patient.Email = input.Email;
            patient.Address = input.Address;
            patient.PhoneNumber = input.PhoneNumber;
            patient.DateOfBirth = input.DateOfBirth;

            if (patient.Images == null)
                patient.Images = new List<patienttImages>();

            // ================= REMOVE =================
            if (input.RemovedPhotoIds?.Any() == true)
            {
                var imagesToRemove = patient.Images
                    .Where(img => input.RemovedPhotoIds.Contains(img.Id))
                    .ToList();

                foreach (var img in imagesToRemove)
                {
                    await _patientImagesRepository.DeleteAsync(img);
                    patient.Images.Remove(img);   // 🔥 keep memory in sync
                }
            }

            // ================= ADD =================
            if (input.PhotosBase64?.Any() == true)
            {
                var existingBase64 = patient.Images
                    .Select(img => Convert.ToBase64String(img.Image))
                    .ToHashSet();

                foreach (var base64 in input.PhotosBase64)
                {
                    if (!existingBase64.Contains(base64))
                    {
                        patient.Images.Add(new patienttImages
                        {
                            Image = Convert.FromBase64String(base64),
                            FileName = Guid.NewGuid().ToString()
                        });

                        existingBase64.Add(base64);
                    }
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            // ================= RETURN UPDATED DTO =================
            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PatientCode = patient.PatientCode,
                Gender = patient.Gender,
                Email = patient.Email,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,

                // 🔥 VERY IMPORTANT: return both
                PhotosBase64 = patient.Images
                    .Select(img => Convert.ToBase64String(img.Image))
                    .ToList(),

                ImageIds = patient.Images
                    .Select(img => img.Id)
                    .ToList()
            };
        }



        // ===================== DELETE =====================
        public async Task DeletePatientAsync(long id)
        {
            var patient = await _patientRepository.FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                throw new UserFriendlyException($"Patient with id {id} not found.");

            await _patientRepository.DeleteAsync(patient);
        }

        // ===================== COMMON DTO MAPPER =====================
        private PatientDto MapToDto(patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PatientCode = patient.PatientCode,
                Gender = patient.Gender,
                Email = patient.Email,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                PhotosBase64 = patient.Images != null
                    ? patient.Images.Select(img => Convert.ToBase64String(img.Image)).ToList()
                    : new List<string>()
            };
        }

        // ===================== DUPLICATE VALIDATION =====================
        private void ValidateDuplicates(string patientCode, string email, string phone, long? currentId = null)
        {
            var errors = new List<ValidationResult>();

            if (_patientRepository.GetAll().Any(p => p.PatientCode == patientCode && p.Id != currentId))
                errors.Add(new ValidationResult("PatientCode already exists.", new[] { "PatientCode" }));

            if (!string.IsNullOrEmpty(email) &&
                _patientRepository.GetAll().Any(p => p.Email == email && p.Id != currentId))
                errors.Add(new ValidationResult("Email already exists.", new[] { "Email" }));

            if (!string.IsNullOrEmpty(phone) &&
                _patientRepository.GetAll().Any(p => p.PhoneNumber == phone && p.Id != currentId))
                errors.Add(new ValidationResult("PhoneNumber already exists.", new[] { "PhoneNumber" }));

            if (errors.Any())
                throw new AbpValidationException("Validation failed", errors);
        }
    }
}
