using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TercerosExternos.Application.Common.Configuration;
using TercerosExternos.Domain.Interfaces;
using TercerosExternos.Infrastructure.Data;
using TercerosExternos.Infrastructure.Repositories;
using TercerosExternos.Infrastructure.Services;
using WatchDog;
using WatchDog.src.Enums;

namespace TercerosExternos.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider => new DapperConnectionFactory(configuration));

            //Configuración WatchDog
            services.Configure<WatchDogSettings>(configuration.GetSection("WatchDogSettings"));

            services.AddWatchDogServices(opt =>
            {
                opt.IsAutoClear = true;
                opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Weekly;
                opt.SetExternalDbConnString = configuration.GetConnectionString("LrpGenericoDB");
                opt.DbDriverOption = WatchDogDbDriverEnum.MSSQL;
            });

            //Configuración Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:RedisDB"];
                options.InstanceName = "LogIT_Keys";
            });

            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
