using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CraftConf16.Shared
{
    public class CalenderParser : ICalenderParser
    {
        public void GetSchedule(ConfEvent confEvent)
        {
            switch(confEvent)
            {
                case ConfEvent.SessionDay1:
                    GetScheduleForEvent();
                    break;
                case ConfEvent.SessionDay2:
                    GetScheduleForEvent();
                    break;
                default:
                    throw new ArgumentException("Event currently not supported");
            }
        }

        public async Task<IEnumerable<Talk>> GetScheduleForEvent()
        {
            var htmlWeb = new HtmlWeb();
            HtmlDocument doc = null;

            try
            {
                doc = await htmlWeb.LoadFromWebAsync("https://craft-conf.com/2016");
            }
            catch(Exception ex)
            {
                throw;
            }
            
            return null;
        }
    }
}
