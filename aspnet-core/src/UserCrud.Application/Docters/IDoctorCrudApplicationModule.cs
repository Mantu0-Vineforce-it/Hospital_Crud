using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Docters.Dto;

namespace UserCrud.Docters
{
    public interface IDoctorCrudApplicationModule
    {
        Task<List<DocterDto>> GetAllBedsAsync();
        Task<DocterDto> GetBedByIdAsync(int id);
        Task<DocterDto> CreateBedAsync(CreateDocterDto input);
        Task<DocterDto> UpdateBedAsync(UpdateDocterDto input);
        Task DeleteBedAsync(int id);

    }
}
