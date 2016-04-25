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

                var i = 0;

                // Talks //
                foreach(var td in tableData)
                {
                    // Get time and title of talk
                    string time = null;
                    string speaker = null;
                    string title = null;

                    IEnumerable<HtmlNode> timeRootNode = td.ChildNodes;

                    if (isFullTimeSlot)
                    {
                        title = td
                                .ChildNodes
                                .Single(c => c.GetAttributeValue("class", "")
                                        .Equals("talk-title"))
                                .InnerText;

                        var speakerNode = td
                                .Descendants()
                                .SingleOrDefault(d => d.GetAttributeValue("class", "")
                                .Equals("schedule-speaker"));

                        speaker = speakerNode != null ? speakerNode.InnerText : "None";
                    }
                    else
                    {
                        // For non-full time slots, there is another root for Time
                        timeRootNode = tableData;

                        // Ignore first td for it's the date node
                        if (td.GetAttributeValue("class", "").Equals("schedule-time"))
                            continue;

                        speaker = td.ChildNodes
                                .Single(d => d.GetAttributeValue("class", "")
                                .Equals("schedule-speaker"))
                                .InnerText;

                        title = td.ChildNodes
                                .Single(d => d.GetAttributeValue("class", "")
                                .Equals("talk-title"))
                                .InnerText;
                    }

                    time = timeRootNode
                            .Single(c => c.GetAttributeValue("class", "")
                                    .Contains("schedule-time"))
                            .InnerText;

                    // Create new Talk entry
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

                    i++;
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
