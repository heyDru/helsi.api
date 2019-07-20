using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Data.Models;
using Services.DtoModels;
using Services.SearchModels;

namespace Services.Mapper.Profiles
{
    public class PatientMapperProfile : Profile
    {
        public PatientMapperProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>();
            CreateMap<PatientSearchDocument, PatientDto>();
            CreateMap<PatientDto, PatientSearchDocument>();
            CreateMap<Patient, PatientSearchDocument>();
        }
    }
}
