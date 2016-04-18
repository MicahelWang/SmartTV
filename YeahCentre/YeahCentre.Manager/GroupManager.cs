using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.SearchCriteria;
using System;

namespace YeahCentre.Manager
{
    public class GroupManager : IGroupManager
    {
        private readonly ISysGroupRepertory _repertory;
        private readonly IRedisCacheService _redisCacheService;
        private IEnumerable<CoreSysGroup> Groups
        {
            get
            {
                if (!_redisCacheService.IsSet(RedisKey.GroupKey))
                {
                    _redisCacheService.Add(RedisKey.GroupKey, _repertory.GetAll());
                }
                return _redisCacheService.Get<List<CoreSysGroup>>(RedisKey.GroupKey);
            }
        }


        private void UpdateCache()
        {
            _redisCacheService.Remove(RedisKey.GroupKey);
        }

        public GroupManager(ISysGroupRepertory repertory, IRedisCacheService redisCacheService)
        {
            this._repertory = repertory;
            _redisCacheService = redisCacheService;
        }

        public CoreSysGroup GetGroup(string groupId)
        {
            return Groups.FirstOrDefault(m => m.Id == groupId);
        }

        public List<CoreSysGroup> GetAll()
        {
            return Groups.ToList();
        }


        public List<CoreSysGroup> Search(GroupCriteria searchCriteria)
        {
            return _repertory.Search(searchCriteria);
        }

        public void Insert(CoreSysGroup ef)
        {
            var list = Search(new GroupCriteria() { GroupName = ef.GroupName }).FirstOrDefault();
            ef.Id = Guid.NewGuid().ToString("N");
            if(list==null)
            {
                _repertory.Insert(ef);
                UpdateCache();
            }
            else
            {
                throw new Exception("该集团已存在！");
            }
        }

        public void Update(CoreSysGroup ef)
        {
            try
            {
                var obj = Search(new GroupCriteria() { Id = ef.Id }).FirstOrDefault();
                if (obj != null)
                {
                    obj.GroupName = ef.GroupName;
                    obj.IsDelete = ef.IsDelete;
                    obj.TemplateId = ef.TemplateId;
                    obj.ApiKey = ef.ApiKey;
                    _repertory.Update(obj);
                    UpdateCache();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}