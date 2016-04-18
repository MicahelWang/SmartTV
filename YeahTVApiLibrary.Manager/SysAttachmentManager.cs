using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Manager
{
    public class SysAttachmentManager :ISysAttachmentManager
    {
        private readonly ISysAttachmentRepertory _repertory;
        private readonly IRedisCacheService _redisCacheService;

        public SysAttachmentManager(ISysAttachmentRepertory repertory, IRedisCacheService redisCacheService)
        {
            _repertory = repertory;
            _redisCacheService = redisCacheService;
        }

        public List<CoreSysAttachment> GetByIds(int[] ids)
        {
            return _repertory.GetByIds(ids);
        }

        public CoreSysAttachment GetById(int id)
        {
            return _repertory.GetAll().FirstOrDefault(m => m.Id == id);
        }

        public void Delete(int id)
        {
            _repertory.Delete(m => m.Id == id);
        }

        public int Add(CoreSysAttachment entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repertory.Insert(entity);
            return entity.Id;
        }
    }
}
