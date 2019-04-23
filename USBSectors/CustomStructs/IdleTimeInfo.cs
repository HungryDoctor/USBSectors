using System;

namespace USBSectors.CustomStructs
{
    public class IdleTimeInfo
    {
        public DateTime LastInputTime { get; }
        public TimeSpan IdleTime { get; }
        public int SystemUptimeMilliseconds { get; }



        public IdleTimeInfo(DateTime lastInputTime, TimeSpan idleTime, int systemUptimeMilliseconds)
        {
            LastInputTime = lastInputTime;
            IdleTime = idleTime;
            SystemUptimeMilliseconds = systemUptimeMilliseconds;
        }
    }
}
