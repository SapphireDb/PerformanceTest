using System;
using System.Collections.Generic;
using System.Linq;
using PerformanceTestServer.Data;
using PerformanceTestServer.Data.Dto;
using PerformanceTestServer.Data.Models;
using PerformanceTestServer.Helper;
using SapphireDb.Actions;

namespace PerformanceTestServer.Actions
{
    public class DataActions : ActionHandlerBase
    {
        private readonly DataDb _db;

        public DataActions(DataDb db)
        {
            _db = db;
        }

        public void Send(List<DataEntryDto> entries, string clientIdString)
        {
            Guid clientId = Guid.Parse(clientIdString);
            
            IEnumerable<IGrouping<DateTime, DataEntryDto>> groupedByTime = entries.GroupBy(e => e.ReceivedOn.Round(TimeSpan.FromSeconds(1)));
            
            foreach (IGrouping<DateTime, DataEntryDto> timeGrouping in groupedByTime)
            {
                double averageDiff = timeGrouping.Average(e => (e.ReceivedOn - e.CreatedOn).TotalMilliseconds);
                _db.Entries.Add(new DataEntry()
                {
                    ClientId = clientId,
                    AverageDiff = averageDiff,
                    Time = timeGrouping.Key
                });    
            }
            
            _db.SaveChanges();
        }
    }
}