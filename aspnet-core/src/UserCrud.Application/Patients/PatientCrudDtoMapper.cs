using AutoMapper;
using System;
using UserCrud.Patients.Dto;

namespace UserCrud.Patients
{
    public class PatientCrudDtoMapper : Profile
    {
        public PatientCrudDtoMapper()
        {
            CreateMap<CreatePatientDto, patient>()
                .ForMember(dest => dest.Photo,
                    opt => opt.MapFrom(src =>
                        !string.IsNullOrEmpty(src.PhotoBase64)
                            ? Convert.FromBase64String(src.PhotoBase64)
                            : null
                    ));

            CreateMap<UpdatePattientDto, patient>()
                .ForMember(dest => dest.Photo,
                    opt => opt.MapFrom(src =>
                        !string.IsNullOrEmpty(src.PhotoBase64)
                            ? Convert.FromBase64String(src.PhotoBase64)
                            : null
                    ));

            CreateMap<patient, PatientDto>()
                .ForMember(dest => dest.PhotoBase64,
                    opt => opt.MapFrom(src =>
                        src.Photo != null
                            ? Convert.ToBase64String(src.Photo)
                            : null
                    ));
        }
    }
}
