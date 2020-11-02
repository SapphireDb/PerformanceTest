using System;
using System.Collections.Generic;
using PerformanceTestServer.Data.Models;
using PerformanceTestServer.Worker;
using SapphireDb.Actions;

namespace PerformanceTestServer.Actions
{
    public class StoreActions : ActionHandlerBase
    {
        private readonly DatabaseStorageWorker _storageWorker;

        public StoreActions(DatabaseStorageWorker storageWorker)
        {
            _storageWorker = storageWorker;
        }
        
        public void Measurements(Guid clientId, DateTime clientTime, List<MeasurementDto> measurements)
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan clientTimeDiff = now - clientTime;

            foreach (MeasurementDto measurement in measurements)
            {
                DataEntry entry = new DataEntry()
                {
                    ClientId = clientId,
                    AverageDiff = (measurement.Received + clientTimeDiff - measurement.Time).TotalMilliseconds,
                    Time = measurement.Time
                };
                _storageWorker.Store(entry);
            }
        }
    }

    public class MeasurementDto
    {
        public DateTime Time { get; set; }

        public DateTime Received { get; set; }
    }
}