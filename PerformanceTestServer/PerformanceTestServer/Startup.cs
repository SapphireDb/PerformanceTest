using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTestServer.Data;
using PerformanceTestServer.Helper;
using PerformanceTestServer.Worker;
using SapphireDb.Extensions;

namespace PerformanceTestServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSingleton<Producer>();
            services.AddSingleton<DatabaseStorageWorker>();

            services.AddDbContext<DataDb>(cfg => cfg.UseNpgsql("User ID=realtime;Password=pw1234;Host=192.168.0.67;Port=5432;Database=sapphiredb_perf-test;"));
            // services.AddDbContext<DataDb>(cfg => cfg.UseInMemoryDatabase("test"));
            // services.AddDbContext<DataDb>(cfg =>
            //     cfg.UseMySql(
            //         "Server=sapphiredb-loadtest-resulsts.mysql.database.azure.com; Port=3306; Database=results; Uid=sapphiredb@sapphiredb-loadtest-resulsts; Pwd=Pw123456; SslMode=Preferred;"));

            services.AddSapphireDb();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Producer producer, DataDb db)
        {
            db.Database.EnsureCreated();
            producer.Init();

            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseSapphireDb();
        }
    }
}