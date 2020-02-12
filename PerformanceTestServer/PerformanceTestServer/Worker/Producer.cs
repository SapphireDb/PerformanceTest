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
                    Db db = _serviceProvider.CreateScope().ServiceProvider.GetService<Db>();
                    db.Entries.RemoveRange(db.Entries);
                    db.Entries.Add(new Entry());
                    await db.SaveChangesAsync();
                    await Task.Delay(2000);
                }
            });
        }
    }
}