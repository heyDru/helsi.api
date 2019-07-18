using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Data.Repos;
using Data.Repos.Abstractions;

namespace HelsiDemo.Configuration.Autofac
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PatientRepository>().As<IPatientRepository>();
        }
    }
}
