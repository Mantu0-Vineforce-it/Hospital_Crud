using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.PatientAdmission.Dto;

namespace UserCrud.PatientAdmission
{
    public interface IPatientAdmissionDtoApplicationModule
    {
        Task<List<PatientAdmissionDto>> GetAllPatientAdmissionsAsync();
        Task<PatientAdmissionDto> GetPatientAdmissionByIdAsync(int id);
        Task<PatientAdmissionDto> CreatePatientAdmissionAsync(CreatePatientAdmissionDto input);
        Task<PatientAdmissionDto> UpdatePatientAdmissionAsync(UpdatePatientAdmissionDto input);
        Task DeletePatientAdmissionAsync(int id);

    }
}
