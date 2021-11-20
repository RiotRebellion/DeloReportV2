using Delo.DAL.Entities;
using Delo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Delo.DAL
{
    public static class RepositoryRegistration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddTransient<IRepository<Person>, PersonRepository>()
            .AddTransient<IRepository<Department>, DbRepository<Department>>()
            ;
    }
}
