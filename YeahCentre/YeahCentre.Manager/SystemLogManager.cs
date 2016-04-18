using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class SystemLogManager : ISystemLogManager
    {
        private ISystemLogRepertory systemLogRepertory;
        public SystemLogManager(ISystemLogRepertory systemLogRepertory)
        {
            this.systemLogRepertory = systemLogRepertory;
        }
        public SystemLog GetById(int id)
        {
            return systemLogRepertory.FindByKey(id);
        }
        public List<SystemLog> Search(BaseSearchCriteria baseSearchCriteria=null)
        {
            return systemLogRepertory.Search(baseSearchCriteria);
        }
    }
}
