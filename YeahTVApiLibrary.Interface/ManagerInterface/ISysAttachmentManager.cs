using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface ISysAttachmentManager
    {
        List<CoreSysAttachment> GetByIds(int[] ids);

        CoreSysAttachment GetById(int id);

        void Delete(int id);

        int Add(CoreSysAttachment entity);
    }
}
