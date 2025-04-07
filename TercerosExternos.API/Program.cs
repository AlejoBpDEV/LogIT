using Microsoft.Extensions.Options;
using Serilog;
using TercerosExternos.API.Middleware;
using TercerosExternos.Application;
using TercerosExternos.Application.Common.Configuration;
using WatchDog;

namespace TercerosExternos.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //Configure the Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddWatchDogServices();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
            }

            //Configure the ReDoc
            app.UseReDoc(d =>
            {
                d.DocumentTitle = "Documentación API";
                d.RoutePrefix = "api-docs"; // ReDoc en /api-docs
                d.SpecUrl = "/swagger/v1/swagger.json";
            });

            //Configure the WatchDog
            var watchDogSettings = app.Services.GetRequiredService<IOptions<WatchDogSettings>>().Value;

            app.UseWatchDog(opt =>
            {
                opt.WatchPageUsername = watchDogSettings.WatchPageUsername;
                opt.WatchPagePassword = watchDogSettings.WatchPagePassword;
                opt.Blacklist = watchDogSettings.Blacklist;
            });
            app.UseWatchDogExceptionLogger();


            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionResMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
