using System;
using SapphireDb.Actions;

namespace PerformanceTestServer.Actions
{
    public class DateActions : ActionHandlerBase
    {
        public long GetServerDiff(DateTime time)
        {
            return (DateTime.UtcNow - time).Milliseconds;
        }
    }
}