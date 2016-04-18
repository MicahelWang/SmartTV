using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.Manager
{

    public class VODRequestManager : IVODRequestManager
    {
        private IVODRequestRepertory vODRequestRepertory;
        public VODRequestManager(IVODRequestRepertory vODRequestRepertory)
        {
            this.vODRequestRepertory = vODRequestRepertory;
        }
        public void Add(VODRequest request)
        {
            vODRequestRepertory.Insert(request);
        }
    }
}
