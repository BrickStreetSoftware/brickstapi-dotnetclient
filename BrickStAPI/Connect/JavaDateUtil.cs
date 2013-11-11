using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickStreetAPI.Connect
{
    // utility class for serializing / deserializing Java Date types
    public class JavaDateUtil
    {
        private static DateTime javaEpoch = new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime();

        // Java serializes Date to "milliseconds since Jan 1, 1970"
        // Convert to .NET DateTime
        public static DateTime deserializeDateTime(long value)
        {
            long ticks = value * TimeSpan.TicksPerMillisecond;
            TimeSpan span = new TimeSpan(ticks);
            DateTime xdate = javaEpoch.Add(span);
            xdate = xdate.ToUniversalTime();
            return xdate;
        }

        // Convert .NET DateTime to "milliseconds since Jan 1, 1970"
        public static long serializeDateTime(DateTime value)
        {
            // compute java milliseconds value
            DateTime utc = value.ToUniversalTime();
            TimeSpan span = new TimeSpan(utc.Ticks - javaEpoch.Ticks);
            long millisSinceJavaEpoch = (long)span.TotalMilliseconds;
            return millisSinceJavaEpoch;
        }
    }
}
