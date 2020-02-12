using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTestServer.Data;
using PerformanceTestServer.Worker;
using SapphireDb.Extensions;

namespace PerformanceTestServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Producer>();
            
            services.AddMvc(cfg => { cfg.EnableEndpointRouting = false; });
            
            services.AddDbContext<DataDb>(cfg => cfg.UseInMemoryDatabase("data_db"));
            
            services.AddSapphireDb()
                .AddContext<Db>(cfg => cfg.UseInMemoryDatabase("db"));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Producer producer)
        {
            producer.Init();
            
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseSapphireDb();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}