using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PerformanceTestServer.Data;
using PerformanceTestServer.Data.Models;
using PerformanceTestServer.Helper;

namespace PerformanceTestServer.Worker
{
    public class DatabaseStorageWorker : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseStorageWorker> _logger;
        private static readonly object LockObject = new object();
        private static readonly List<DataEntry> DataEntries = new List<DataEntry>();

        private static int DataCountForStorage;
        
        public DatabaseStorageWorker(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DatabaseStorageWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            DataCountForStorage = configuration.GetValue<int>("DataCountForStorage");
            logger.LogInformation($"Initialized database Worker with storage count of {DataCountForStorage}");
        }

        public void Store(DataEntry entry)
        {
            Task.Run(() =>
            {
                lock (LockObject)
                {
                    DataEntries.Add(entry);

                    if (DataEntries.Count >= DataCountForStorage)
                    {
                        TransferToDatabase();
                    }
                }
            });
        }

        private void TransferToDatabase()
        {
            DataDb db = _serviceProvider.CreateScope().ServiceProvider.GetService<DataDb>();

            List<DataEntry> entriesGrouped = DataEntries
                .GroupBy(e => new {e.ClientId, Time = e.Time.Round(TimeSpan.FromSeconds(1))})
                .Select(grouping =>
                {
                    return new DataEntry()
                    {
                        Time = grouping.Key.Time,
                        ClientId = grouping.Key.ClientId,
                        AverageDiff = grouping.Average(v => v.AverageDiff)
                    };
                })
                .ToList();
            
            db.Entries.AddRange(entriesGrouped);
            db.SaveChanges();
            DataEntries.Clear();
            _logger.LogInformation($"Storing data to database");
        }

        public void Dispose()
        {
            TransferToDatabase();
        }
    }
}