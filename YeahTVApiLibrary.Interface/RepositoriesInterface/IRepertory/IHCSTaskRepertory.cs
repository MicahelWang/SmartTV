using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IHCSTaskRepertory : IBsaeRepertory<HCSDownloadTask>
    {
        //List<HCSDownloadTask> GetAllWithInclude();
        int GetRecordCount();
    }
}
