using System;

namespace DustBunny.Models
{
    public class Process
    {
        public string Name { get; set; }
        public string AnnounceChannel { get; set; }
        public DateTime? OfflineTime { get; set; }
        public string Path { get; set; }
        public string ProcessName { get; set; }
    }
}