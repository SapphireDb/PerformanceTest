using System;
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
        
        public void Received(DateTime messageTime, string clientIdRaw)
        {
            DateTime now = DateTime.UtcNow;
            Guid clientId = Guid.Parse(clientIdRaw);

            DataEntry entry = new DataEntry()
            {
                ClientId = clientId,
                AverageDiff = (now - messageTime).TotalMilliseconds,
                Time = messageTime
            };
            _storageWorker.Store(entry);
        }
    }
}