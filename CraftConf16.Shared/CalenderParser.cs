using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraftConf16.Shared
{
    public class CalenderParser : ICalenderParser
    {
        public async Task<Dictionary<string, List<Talk>>> GetSchedule(ConfEvent confEvent)
        {
            var rootNode = await GetRootNode();

            var dayNode = rootNode.Descendants()
                            .Where(d => d.GetAttributeValue("class", "")
                            .Contains("day-" + (int)confEvent))
                            .Single()
                            .FirstChild
                            .NextSibling;

            // Get all rows of this day
            var rows = dayNode.Descendants("tr");

            // Get Header
            var stages = rows
                        .First()
                        .ChildNodes
                        .Where(c => c.GetAttributeValue("class", "")
                        .Equals("stage"))
                        .Select(s => s.InnerText)
                        .ToList();

            var resultList = new Dictionary<string, List<Talk>>();

            // Row = Timeslot
            foreach(var row in rows)
            {
                var tableData = row.Descendants("td");

                // Leave out header row
                if (tableData.Count() == 0)
                    continue;

                // Full Time Slots need a special treatment
                var isFullTimeSlot = tableData.Any(x => x.Descendants().Any(d => d.GetAttributeValue("class", "").Contains("full-slot")));
                                
                var amountTalksThisRow = row.ChildNodes.Count(c => c.GetAttributeValue("class", "").Equals("schedule-slot"));

                // Talks //
                for(var i = 0; i< amountTalksThisRow; i++)
                {
                    // Get time and title of talk
                    string time = null;
                    string speaker = null;
                    string title = null;

                    IEnumerable<HtmlNode> timeRootNode = tableData;

                    if (isFullTimeSlot)
                    {
                        title = tableData
                                    .First()
                                    .ChildNodes
                                    .Single(c => c.GetAttributeValue("class", "")
                                            .Equals("talk-title"))
                                    .InnerText;

                        var speakerNode = tableData
                                .First()
                                .Descendants()
                                .SingleOrDefault(d => d.GetAttributeValue("class", "")
                                .Equals("schedule-speaker"));

                        speaker = speakerNode != null ? speakerNode.InnerText : "None";

                        // For full time slots, there is another root for Time
                        timeRootNode = tableData.First().ChildNodes;
                    }
                    else
                    {
                        continue;
                        title = tableData
                                .SingleOrDefault(d => d.GetAttributeValue("class", "")
                                .Equals("schedule-speaker")).InnerText;
                    }

                    time = timeRootNode
                            .Single(c => c.GetAttributeValue("class", "")
                                    .Contains("schedule-time"))
                            .InnerText;

                    // Create new Talk entry
                    //titles.ForEach(t => t.)
                    var talk = new Talk()
                    {
                        StartTime = time.Substring(0, 5),
                        EndTime = time.Substring(8, 5),
                        Title = title,
                        Speaker = speaker
                    };

                    // Check if it's full-time slot or splitted slot
                    var key = isFullTimeSlot ? "Full Slot" : stages[i];

                    if (resultList.ContainsKey(key))
                        resultList[key].Add(talk);
                    else
                        resultList.Add(key, new List<Talk>() { talk });
                }                
            }

            return resultList;
        }

        public async Task<HtmlNode> GetRootNode()
        {
            var htmlWeb = new HtmlWeb();
            HtmlDocument doc = null;

            try
            {
                doc = await htmlWeb.LoadFromWebAsync("https://craft-conf.com/2016");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return doc.DocumentNode;
        }
    }
}
