using Delo.DAL;
using Delo.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeloReportV2.Data
{
    static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration Configuration) => services
            .AddDbContext<DeloDB>(opt =>
            {
                var type = Configuration["Type"];
                switch (type)
                {
                    case null: throw new InvalidOperationException("Не определен тип БД");

                    default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");
                    //case "MSSQL":
                    //    opt.UseSqlServer(Configuration.GetConnectionString(type));
                    //    break;
                    case "MSSQL":
                        opt.UseSqlServer(Configuration.GetConnectionString(type));
                        break;
                    case "SqLite":
                        opt.UseSqlite(Configuration.GetConnectionString(type));
                        break;
                    case "InMemory":
                        opt.UseInMemoryDatabase("test");
                        break;
                }
            })
            .AddTransient<DbInitializer>()
            .AddRepositories()
        ;
    }
}
