using System;
using Autofac;
using Autofac.Builder;
using Common.Models.SearchModels;
using Data.Models;
using Microsoft.Extensions.Configuration;
using Nest;
using Services.DtoModels;

namespace HelsiApi.Configuration
{
    public class ElacsticCLientModule : Module
    {
        private readonly  IConfiguration _configuration;
        public ElacsticCLientModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegistrNestClient(_configuration["ElasticSearch:url"], _configuration["ElasticSearch:index"]);
        }
    }

    public static class RegistrNest
    {
        public static IRegistrationBuilder<IElasticClient, SimpleActivatorData, SingleRegistrationStyle>
            RegistrNestClient(this ContainerBuilder builder, string baseUrl, string defaultIndex)
        {
            return builder.Register(context =>
            {
                var c = context.Resolve<IComponentContext>();

                var settings = new ConnectionSettings(new Uri(baseUrl))
                    .DefaultIndex(defaultIndex)
                    .DefaultMappingFor<PatientSearchDocument>(x=>x.IndexName("patient").IdProperty(p=>p.UserId))
                    .DefaultFieldNameInferrer(x=>x.ToLowerInvariant());

                var client = new ElasticClient(settings);
                return client;
            }).As<IElasticClient>().SingleInstance();
        }
    }
}
