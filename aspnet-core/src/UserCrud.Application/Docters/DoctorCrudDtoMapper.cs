using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Docters.Dto;
using UserCrud.Patients;
using UserCrud.Patients.Dto;

namespace UserCrud.Docters
{
    public class DoctorCrudDtoMapper:Profile
    {

        public DoctorCrudDtoMapper()
        {
            CreateMap<CreateDocterDto, doctor>();
            CreateMap<UpdateDocterDto, doctor>();
            CreateMap<doctor, DocterDto>();
        }

    }
}
