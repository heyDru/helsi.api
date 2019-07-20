using Autofac;
using Data.Repos;
using Data.Repos.Abstractions;
using Services;
using Services.Abstractions;
using Services.Mapper;
using Services.SearchModels;

namespace HelsiApi.Configuration
{
    public class AutofacDiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PatientRepository>().As<IPatientRepository>();
            builder.RegisterType<PatientService>().As<IPatientService>();

            builder.RegisterType<Elasticsearch<PatientSearchDocument>>().As<ISearchable<PatientSearchDocument>>();

            var mapper = ServiceMappingConfig.Initialise();
            builder.Register(x => mapper.CreateMapper());
        }
    }
}
