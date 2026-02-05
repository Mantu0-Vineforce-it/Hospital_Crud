using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
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

            // ✅ Base64 → byte[]
            if (!string.IsNullOrEmpty(input.PhotoBase64))
            {
                patient.Photo = Convert.FromBase64String(input.PhotoBase64);
            }

            await _patientRepository.InsertAsync(patient);

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

        // ===================== UPDATE =====================
        public async Task<PatientDto> UpdatePatientAsync(UpdatePattientDto input)
        {
            var patient = await _patientRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            if (patient == null)
                throw new Exception($"Patient with id {input.Id} not found.");

            patient.FirstName = input.FirstName;
            patient.LastName = input.LastName;
            patient.PatientCode = input.PatientCode;
            patient.Gender = input.Gender;
            patient.Address = input.Address;
            patient.PhoneNumber = input.PhoneNumber;
            patient.DateOfBirth = input.DateOfBirth;

            if (!string.IsNullOrEmpty(input.PhotoBase64))
            {
                patient.Photo = Convert.FromBase64String(input.PhotoBase64);
            }

            await _patientRepository.UpdateAsync(patient);

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PatientCode = patient.PatientCode,
                Gender = patient.Gender,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                PhotoBase64 = patient.Photo != null
                    ? Convert.ToBase64String(patient.Photo)
                    : null
            };
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