using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapphireDb.Connection;

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
                    Console.WriteLine("Sending message");
                    SapphireMessageSender messageSender = _serviceProvider.CreateScope().ServiceProvider.GetService<SapphireMessageSender>();
                    messageSender.Send(DateTime.UtcNow);
                    await Task.Delay(2000);
                }
            });
        }
    }
}