using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTestServer.Data;
using SapphireDb.Extensions;

namespace PerformanceTestServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSapphireDb()
                .AddContext<Db>(cfg => cfg.UseInMemoryDatabase("db"));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSapphireDb();
        }
    }
}