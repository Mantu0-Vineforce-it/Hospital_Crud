using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Patients.Dto;

namespace UserCrud.Patients
{
    public class PatientCrudService : ApplicationService
    {
        private readonly IRepository<patient, long> _patientRepository;

        public PatientCrudService(IRepository<patient, long> patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // ===================== GET ALL =====================
        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllListAsync();

            return patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                PatientCode = p.PatientCode,
                Address = p.Address,
                PhoneNumber = p.PhoneNumber,
                PhotoBase64 = p.Photo != null
                    ? Convert.ToBase64String(p.Photo)
                    : null
            }).ToList();
        }

        // ===================== GET BY ID =====================
        public async Task<PatientDto> GetPatientByIdAsync(long id)
        {
            var patient = await _patientRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
                throw new Exception($"Patient with id {id} not found.");

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                Gender = patient.Gender,
                PatientCode = patient.PatientCode,
                DateOfBirth = patient.DateOfBirth,
                PhotoBase64 = patient.Photo != null
                    ? Convert.ToBase64String(patient.Photo)
                    : null
            };
        }

        // ===================== CREATE =====================
        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto input)
        {
            try
            {
                var validationErrors = new List<ValidationResult>();

                // Check PatientCode duplicate
                if (await _patientRepository.FirstOrDefaultAsync(p => p.PatientCode == input.PatientCode) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"PatientCode '{input.PatientCode}' is already in use.",
                        new[] { "PatientCode" }));
                }

                // Check Email duplicate
                if (!string.IsNullOrEmpty(input.Email) &&
                    await _patientRepository.FirstOrDefaultAsync(p => p.Email == input.Email) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"Email '{input.Email}' is already in use.",
                        new[] { "Email" }));
                }

                // Check PhoneNumber duplicate
                if (!string.IsNullOrEmpty(input.PhoneNumber) &&
                    await _patientRepository.FirstOrDefaultAsync(p => p.PhoneNumber == input.PhoneNumber) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"Phone number '{input.PhoneNumber}' is already in use.",
                        new[] { "PhoneNumber" }));
                }

                // Throw validation errors if any
                if (validationErrors.Any())
                {
                    throw new AbpValidationException("Validation failed", validationErrors);
                }

                var patient = new patient
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    PatientCode = input.PatientCode,
                    Gender = input.Gender,
                    Email = input.Email,
                    Address = input.Address,
                    PhoneNumber = input.PhoneNumber,
                    DateOfBirth = input.DateOfBirth
                };

                if (!string.IsNullOrEmpty(input.PhotoBase64))
                {
                    patient.Photo = Convert.FromBase64String(input.PhotoBase64);
                }

                var createdPatient = await _patientRepository.InsertAsync(patient);

                return new PatientDto
                {
                    Id = createdPatient.Id,
                    FirstName = createdPatient.FirstName,
                    LastName = createdPatient.LastName,
                    PatientCode = createdPatient.PatientCode,
                    Gender = createdPatient.Gender,
                    Email = createdPatient.Email,
                    Address = createdPatient.Address,
                    PhoneNumber = createdPatient.PhoneNumber,
                    DateOfBirth = createdPatient.DateOfBirth,
                    PhotoBase64 = createdPatient.Photo != null
                        ? Convert.ToBase64String(createdPatient.Photo)
                        : null
                };
            }
            catch (AbpValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(
                    "An unexpected error occurred while creating the patient.", ex);
            }
        }


        // ===================== UPDATE =====================
        public async Task<PatientDto> UpdatePatientAsync(UpdatePattientDto input)
        {
            try
            {
                var patient = await _patientRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
                if (patient == null)
                {
                    throw new Exception($"Patient with id {input.Id} not found.");
                }

                var validationErrors = new List<ValidationResult>();

                // PatientCode duplicate (exclude current)
                if (await _patientRepository.FirstOrDefaultAsync(
                        p => p.PatientCode == input.PatientCode && p.Id != input.Id) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"PatientCode '{input.PatientCode}' is already in use.",
                        new[] { "PatientCode" }));
                }

                // Email duplicate (exclude current)
                if (!string.IsNullOrEmpty(input.Email) &&
                    await _patientRepository.FirstOrDefaultAsync(
                        p => p.Email == input.Email && p.Id != input.Id) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"Email '{input.Email}' is already in use.",
                        new[] { "Email" }));
                }

                // PhoneNumber duplicate (exclude current)
                if (!string.IsNullOrEmpty(input.PhoneNumber) &&
                    await _patientRepository.FirstOrDefaultAsync(
                        p => p.PhoneNumber == input.PhoneNumber && p.Id != input.Id) != null)
                {
                    validationErrors.Add(new ValidationResult(
                        $"Phone number '{input.PhoneNumber}' is already in use.",
                        new[] { "PhoneNumber" }));
                }

                // Throw validation errors if any
                if (validationErrors.Any())
                {
                    throw new AbpValidationException("Validation failed", validationErrors);
                }

                // Update patient
                patient.FirstName = input.FirstName;
                patient.LastName = input.LastName;
                patient.PatientCode = input.PatientCode;
                patient.Gender = input.Gender;
                patient.Email = input.Email;
                patient.Address = input.Address;
                patient.PhoneNumber = input.PhoneNumber;
                patient.DateOfBirth = input.DateOfBirth;

                if (!string.IsNullOrEmpty(input.PhotoBase64))
                {
                    patient.Photo = Convert.FromBase64String(input.PhotoBase64);
                }

                await _patientRepository.UpdateAsync(patient);

                // Return updated DTO
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
                    PhotoBase64 = patient.Photo != null
                        ? Convert.ToBase64String(patient.Photo)
                        : null
                };
            }
            catch (AbpValidationException vex)
            {
                // Re-throw validation exceptions to be handled in the service layer or UI
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logger
                // _logger.LogError(ex, "Error updating patient with Id {Id}", input.Id);

                // Wrap unexpected exceptions in a user-friendly message
                throw new UserFriendlyException("An error occurred while updating the patient. Please try again.", ex);
            }
        }

        // ===================== DELETE =====================
        public async Task DeletePatientAsync(long id)
        {
            var patient = await _patientRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
                throw new Exception($"Patient with id {id} not found.");

            await _patientRepository.DeleteAsync(patient);
        }
    }
}