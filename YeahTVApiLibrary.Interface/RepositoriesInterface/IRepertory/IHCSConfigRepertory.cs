using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IHCSConfigRepertory : IBsaeRepertory<HCSConfig>
    {
        //List<HCSConfig> GetAllWithInclude();
    }
}
