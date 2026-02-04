using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.Beds.Dto;
using UserCrud.Docters;
using UserCrud.Docters.Dto;

namespace UserCrud.Beds
{
    public class BedCrudDtoMapper :Profile
    {
        public BedCrudDtoMapper()
        {
            CreateMap<CreateBedDto, bed>();
            CreateMap<UpdateBedDto, bed>();
            CreateMap<bed, BedDto>();
        }

    }
}
