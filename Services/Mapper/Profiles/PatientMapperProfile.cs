using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Data.Models;
using Services.DtoModels;

namespace Services.Mapper.Profiles
{
    public class PatientMapperProfile : Profile
    {
        public PatientMapperProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>();
        }
    }
}
