using System;
using System.Collections.Generic;
using System.Text;

namespace CraftConf16.Shared
{
    public class Talk
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Title { get; set; }

        public string Speaker { get; set; }

        public string TimeSlot { get { return StartTime + "-" + EndTime; } }
    }
}
