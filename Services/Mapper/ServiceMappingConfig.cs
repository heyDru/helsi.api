﻿using AutoMapper;

namespace Services.Mapper
{
    public static class ServiceMappingConfig
    {
        public static MapperConfiguration Initialise()
        {
            var mapper = new MapperConfiguration(config =>
            {
                config.AddProfiles(typeof(ServiceMappingConfig).Assembly);
            });
            return mapper;
        }
    }
}
