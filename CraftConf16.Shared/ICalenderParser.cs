using System.Collections.Generic;
using System.Threading.Tasks;

namespace CraftConf16.Shared
{
    public interface ICalenderParser
    {
        Task<Dictionary<string,List<Talk>>> GetSchedule(ConfEvent confEvent);
    }
}