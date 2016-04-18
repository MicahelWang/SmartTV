using YeahTVApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Infrastructure
{
    public interface IAlarmClockService
    {
        FunResult SetMorningCall(DateTime? setTime, string hotelId, string roomNo);

        DateTime? GetMorningCall(string hotelId, string roomNo);
    }
}
