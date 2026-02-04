using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrud.PatientAdmission.Dto;
using UserCrud.Patients;
using UserCrud.Patients.Dto;

namespace UserCrud.PatientAdmission
{
    public class PatientAdmissionCrudMapper:Profile
    {
        public PatientAdmissionCrudMapper()
        {
            CreateMap<CreatePatientAdmissionDto, patientAdmission>();
            CreateMap<UpdatePatientAdmissionDto, patientAdmission>();
            CreateMap<patientAdmission, PatientAdmissionDto>();
        }

    }
}
