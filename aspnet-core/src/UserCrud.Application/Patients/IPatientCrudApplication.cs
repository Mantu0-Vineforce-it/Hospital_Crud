using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCrud.Patients
{
    public interface IPatientCrudApplication
    {
        Task<List<Patients.Dto.PatientDto>> GetAllPatientsAsync();
        Task<Patients.Dto.PatientDto> GetPatientByIdAsync(int id);
        Task<Patients.Dto.PatientDto> CreatePatientAsync(Patients.Dto.CreatePatientDto input);
        Task<Patients.Dto.PatientDto> UpdatePatientAsync(Patients.Dto.UpdatePattientDto input);
        Task DeletePatientAsync(int id);

    }
}
