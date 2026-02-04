using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Beds.Dto;
using UserCrud.Docters.Dto;

namespace UserCrud.Beds
{
    public interface IBedCrudApplicationModule
    {
        Task<List<BedDto>> GetAllPatientsAsync();
        Task<BedDto> GetPatientByIdAsync(int id);
        Task<BedDto> CreatePatientAsync(CreateBedDto input);
        Task<BedDto> UpdatePatientAsync(UpdateBedDto input);
        Task DeletePatientAsync(int id);

    }
}
