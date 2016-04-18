using YeahTVApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Infrastructure
{
    public interface ICentralGetwayServiceBase
    {
        Type SourceType { get; }

        Object ConvertTo(String json, Guest guest);
    }
}
