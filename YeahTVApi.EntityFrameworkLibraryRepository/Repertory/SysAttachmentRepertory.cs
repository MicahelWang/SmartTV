using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysAttachmentRepertory :BaseRepertory<CoreSysAttachment, int>, ISysAttachmentRepertory
    {
        public override List<CoreSysAttachment> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public List<CoreSysAttachment> GetByIds(int[] ids)
        {
          return  this.Entities.Where(m => ids.Contains(m.Id)).ToList();
        }
    }
}
