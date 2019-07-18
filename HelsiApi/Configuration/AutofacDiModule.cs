using Autofac;
using Data.Repos;
using Data.Repos.Abstractions;
using Services;
using Services.Abstractions;
using Services.Mapper;

namespace HelsiApi.Configuration
{
    public class AutofacDiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PatientRepository>().As<IPatientRepository>();
            builder.RegisterType<PatientService>().As<IPatientService>();

            var mapper = ServiceMappingConfig.Initialise();
            builder.Register(x => mapper.CreateMapper());
        }
    }
}
