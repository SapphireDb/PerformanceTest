using System;
using System.Collections.Generic;
using PerformanceTestServer.Data.Models;
using PerformanceTestServer.Worker;
using SapphireDb.Actions;

namespace PerformanceTestServer.Actions
{
    public class MessageActions : ActionHandlerBase
    {
        private readonly DatabaseStorageWorker _storageWorker;

        public MessageActions(DatabaseStorageWorker storageWorker)
        {
            _storageWorker = storageWorker;
        }
        
        public void Received(Guid clientId, DateTime clientTime, List<LastEntryDto> lastEntries)
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan clientTimeDiff = now - clientTime;

            foreach (LastEntryDto lastEntry in lastEntries)
            {
                DataEntry entry = new DataEntry()
                {
                    ClientId = clientId,
                    AverageDiff = lastEntry.Diff,
                    Time = lastEntry.Time + clientTimeDiff
                };
                _storageWorker.Store(entry);
            }
        }
    }

    public class LastEntryDto
    {
        public DateTime Time { get; set; }

        public double Diff { get; set; }
    }
}