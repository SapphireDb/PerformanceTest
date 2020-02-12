using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using PerformanceTestServer.Data;

namespace PerformanceTestServer.Controllers
{
    public class DataController
    {
        private readonly DataDb _db;

        public DataController(DataDb db)
        {
            _db = db;
        }
        
        [HttpGet]
        public string GetData()
        {
            using StringWriter writer = new StringWriter();
            using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(_db.Entries.ToList());
            csv.Flush();
            return writer.ToString();
        }

        [HttpGet]
        public void Reset()
        {
            _db.Entries.RemoveRange(_db.Entries);
            _db.SaveChanges();
        }
    }
}