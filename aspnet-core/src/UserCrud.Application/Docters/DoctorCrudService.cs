using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Docters.Dto;

namespace UserCrud.Docters
{
    public class DoctorCrudService : ApplicationService
    {
        private readonly IRepository<doctor, long> _doctorRepository;

        public DoctorCrudService(IRepository<doctor, long> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        // Get all doctors
        public async Task<List<DocterDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllListAsync();
            return doctors.Select(d => new DocterDto
            {
                Id = d.Id,
                DocterCode = d.DocterCode,
                FullName = d.FullName,
                Specialization = d.Specialization,
                Qualification = d.Qualification,
                PhoneNumber = d.PhoneNumber,
                Email = d.Email,
                IsAvailble = d.IsAvailble
            }).ToList();
        }

        // Get doctor by ID
        public async Task<DocterDto> GetDoctorByIdAsync(long id)
        {
            var doctor = await _doctorRepository.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                throw new Exception($"Doctor with id {id} not found.");
            }

            return new DocterDto
            {
                Id = doctor.Id,
                DocterCode = doctor.DocterCode,
                FullName = doctor.FullName,
                Specialization = doctor.Specialization,
                Qualification = doctor.Qualification,
                PhoneNumber = doctor.PhoneNumber,
                Email = doctor.Email,
                IsAvailble = doctor.IsAvailble
            };
        }

        // Create a new doctor
        public async Task<DocterDto> CreateDoctorAsync(CreateDocterDto input)
        {
            var doctor = new doctor
            {
                DocterCode = input.DocterCode,
                FullName = input.FullName,
                Specialization = input.Specialization,
                Qualification = input.Qualification,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                IsAvailble = input.IsAvailble
            };

            var createdDoctor = await _doctorRepository.InsertAsync(doctor);

            return new DocterDto
            {
                Id = createdDoctor.Id,
                DocterCode = createdDoctor.DocterCode,
                FullName = createdDoctor.FullName,
                Specialization = createdDoctor.Specialization,
                Qualification = createdDoctor.Qualification,
                PhoneNumber = createdDoctor.PhoneNumber,
                Email = createdDoctor.Email,
                IsAvailble = createdDoctor.IsAvailble
            };
        }

        // Update an existing doctor
        public async Task<DocterDto> UpdateDoctorAsync(UpdateDocterDto input)
        {
            var doctor = await _doctorRepository.FirstOrDefaultAsync(d => d.Id == input.Id);
            if (doctor == null)
            {
                throw new Exception($"Doctor with id {input.Id} not found.");
            }

            doctor.DocterCode = input.DocterCode;
            doctor.FullName = input.FullName;
            doctor.Specialization = input.Specialization;
            doctor.Qualification = input.Qualification;
            doctor.PhoneNumber = input.PhoneNumber;
            doctor.Email = input.Email;
            doctor.IsAvailble = input.IsAvailble;

            await _doctorRepository.UpdateAsync(doctor);

            return new DocterDto
            {
                Id = doctor.Id,
                DocterCode = doctor.DocterCode,
                FullName = doctor.FullName,
                Specialization = doctor.Specialization,
                Qualification = doctor.Qualification,
                PhoneNumber = doctor.PhoneNumber,
                Email = doctor.Email,
                IsAvailble = doctor.IsAvailble
            };
        }

        // Delete a doctor
        public async Task DeleteDoctorAsync(long id)
        {
            var doctor = await _doctorRepository.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                throw new Exception($"Doctor with id {id} not found.");
            }

            await _doctorRepository.DeleteAsync(doctor);
        }
    }
}
