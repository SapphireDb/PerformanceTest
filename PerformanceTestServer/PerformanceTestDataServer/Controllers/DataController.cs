using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PerformanceTestDataServer.Data;
using PerformanceTestDataServer.Data.Models;
using PerformanceTestDataServer.Dto;

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
            DateTime startTime = entries.Min(e => e.ReceivedOn);
            DateTime endTime = entries.Max(e => e.ReceivedOn);
            double averageServerDiff = entries.Average(e => e.DiffFromServer);
            double averageClientDiff = entries.Average(e => e.DiffFromClient);

            _db.Entries.Add(new Entry()
            {
                ClientId = clientId,
                StartTime = startTime,
                EndTime = endTime,
                AverageDiffFromClient = averageClientDiff,
                AverageDiffFromServer = averageServerDiff
            });
            _db.SaveChanges();
        }
    }
}