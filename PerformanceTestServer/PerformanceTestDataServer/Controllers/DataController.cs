using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PerformanceTestDataServer.Data;
using PerformanceTestDataServer.Data.Models;
using PerformanceTestDataServer.Dto;
using PerformanceTestDataServer.Helper;

namespace PerformanceTestDataServer.Controllers
{
    public class DataController : Controller
    {
        private readonly Db _db;

        public DataController(Db db)
        {
            _db = db;
        }
        
        [HttpPost]
        public void PostData([FromBody] List<EntryDto> entries)
        {
            Guid clientId = entries.FirstOrDefault()?.ClientId ?? Guid.Empty;

            IEnumerable<IGrouping<DateTime, EntryDto>> groupedByTime = entries.GroupBy(e => e.ReceivedOn.Round(TimeSpan.FromSeconds(1)));

            foreach (IGrouping<DateTime,EntryDto> timeGrouping in groupedByTime)
            {
                double averageServerDiff = timeGrouping.Average(e => e.AverageServerDiff);
                double averageClientDiff = timeGrouping.Average(e => e.AverageClientDiff);
                _db.Entries.Add(new Entry()
                {
                    ClientId = clientId,
                    AverageClientDiff = averageClientDiff,
                    AverageServerDiff = averageServerDiff,
                    Time = timeGrouping.Key
                });    
            }
            
            _db.SaveChanges();
        }
    }
}