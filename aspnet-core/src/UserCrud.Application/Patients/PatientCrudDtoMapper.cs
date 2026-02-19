using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UserCrud.PatientImages;
using UserCrud.Patients;
using UserCrud.Patients.Dto;

namespace UserCrud.Patients
{
    public class PatientCrudDtoMapper : Profile
    {
        public PatientCrudDtoMapper()
        {
            // ================= CREATE =================
            CreateMap<CreatePatientDto, patient>()
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src =>
                        src.PhotosBase64 != null && src.PhotosBase64.Any()
                            ? src.PhotosBase64.Select(base64 => new patienttImages
                            {
                                Image = Convert.FromBase64String(base64),
                                FileName = Guid.NewGuid().ToString()
                            }).ToList()
                            : new List<patienttImages>()
                    ));

            // ================= ENTITY → DTO =================
            CreateMap<patient, PatientDto>()
                .ForMember(dest => dest.PhotosBase64,
                    opt => opt.MapFrom(src =>
                        src.Images != null && src.Images.Any()
                            ? src.Images.Select(img =>
                                Convert.ToBase64String(img.Image)
                              ).ToList()
                            : new List<string>()
                    ))
                .ForMember(dest => dest.ImageIds,
                    opt => opt.MapFrom(src =>
                        src.Images != null && src.Images.Any()
                            ? src.Images.Select(img => img.Id).ToList()
                            : new List<long>()
                    ));
        }
    }
}
