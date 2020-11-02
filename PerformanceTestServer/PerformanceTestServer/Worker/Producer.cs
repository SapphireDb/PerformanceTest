using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PerformanceTestServer.Data;
using PerformanceTestServer.Data.Models;

namespace PerformanceTestServer.Worker
{
    public class Producer
    {
        private readonly IServiceProvider _serviceProvider;

        public Producer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Init()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    BenchmarkDb db = _serviceProvider.CreateScope().ServiceProvider.GetService<BenchmarkDb>();

                    Console.WriteLine("Removing entry");
                    db.Entries.RemoveRange(db.Entries);
                    db.SaveChanges();
                    
                    Console.WriteLine("Creating entry");
                    db.Entries.Add(new BenchmarkEntry());
                    db.SaveChanges();
                    
                    await Task.Delay(2000);
                }
            });
        }
    }
}