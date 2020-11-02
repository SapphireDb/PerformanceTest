using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTestServer.Data;
using PerformanceTestServer.Helper;
using PerformanceTestServer.Worker;
using SapphireDb.Extensions;

namespace PerformanceTestServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSingleton<Producer>();
            services.AddSingleton<DatabaseStorageWorker>();

            string dbConnectionString = _configuration.GetValue<string>("Database");
            services.AddDbContext<DataDb>(cfg => cfg.UseNpgsql(dbConnectionString));

            services.AddSapphireDb()
                .AddContext<BenchmarkDb>(cfg => cfg.UseInMemoryDatabase("benchmark"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataDb db, Producer producer)
        {
            db.Database.EnsureCreated();
            producer.Init();

            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseSapphireDb();
        }
    }
}