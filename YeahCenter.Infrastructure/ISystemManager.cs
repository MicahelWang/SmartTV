using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCenter.Infrastructure
{
    public interface ISystemLogManager
    {
        SystemLog GetById(int id);
        List<SystemLog> Search(BaseSearchCriteria baseSearchCriteria);
    }
}
