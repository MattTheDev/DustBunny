using System.Collections.Generic;
using DustBunny.Modules;

namespace DustBunny.Models
{
    public class DustBunnySettings
    {
        public Ids Ids { get; set; }
        public List<Bot> Bots { get; set; }
        public List<Process> Processes { get; set; }
        public Tokens Tokens { get; set; }
    }
}