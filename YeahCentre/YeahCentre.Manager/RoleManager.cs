using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class RoleManager : IRoleManager
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ISysRoleResourceRelationRepertory _relationRepertory;
        private readonly ISysRoleRepertory _repertory;
        private readonly IPowerResourceRepertory _resourceRepertory;

        public RoleManager(ISysRoleRepertory repertory, IRedisCacheService redisCacheService,
            ISysRoleResourceRelationRepertory relationRepertory, IPowerResourceRepertory resourceRepertory)
        {
            _repertory = repertory;
            _redisCacheService = redisCacheService;
            _relationRepertory = relationRepertory;
            _resourceRepertory = resourceRepertory;
        }

        public List<ErpPowerRole> GetAll()
        {
            return (List<ErpPowerRole>)Roles();
        }

        public ErpPowerRole GetEntity(string id)
        {
            return Roles().FirstOrDefault(m => m.Id == id);
        }

        public ErpPowerRole GetEntityName(string rolename)
        {
            return Roles().FirstOrDefault(m => m.RoleName == rolename);
        }



        public List<ErpPowerRole> Search()
        {
            return (List<ErpPowerRole>)Roles();
        }

        public bool Add(ErpPowerRole entity)
        {
            entity.Id = Guid.NewGuid().ToString("N");
            _repertory.Insert(entity);
            _redisCacheService.Remove(RedisKey.RoleKey);
            return true;
        }

        public void BatchDelete(string[] roles)
        {
            _repertory.Delete(m => roles.Contains(m.Id));
            RemoveRole();
            foreach (var id in roles)
            {
                RemoveRelationsByRole(id);
                RemoveMenuByRole(id);
                RemovePowerByRole(id);
            }
        }

        public bool Update(ErpPowerRole entity)
        {
            var temp = _repertory.FindByKey(entity.Id);
            temp.RoleCode = entity.RoleCode;
            temp.RoleName = entity.RoleName;
            _repertory.Update(temp);
            _redisCacheService.Remove(RedisKey.RoleKey);
            return true;
        }

        public IPagedList<ErpPowerRole> PagedList(int pageIndex, int pageSize, string keyword)
        {
            var query = Roles();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.RoleName.StartsWith(keyword) || m.RoleCode.StartsWith(keyword));
            }
            return new PagedList<ErpPowerRole>(query.ToList(), pageIndex, pageSize);
        }

        public List<ErpPowerRoleResourceRelation> GetRelations(string id)
        {
            return GetRelationsByRole(id);
        }

        public IEnumerable<TreeNode> GetPower(string roleId)
        {
            return GetPowerByRole(roleId);
        }

        public IEnumerable<ErpPowerResource> GetPowerResource(string roleId)
        {
            return GetMenuByRole(roleId);
        }

        public int[] GetResourcesIdByRole(string roleId)
        {
            var result = GetRelationsByRole(roleId).Select(m => m.ResourceId).ToArray();
            return result;
        }

        public bool UpdatePower(string id, int[] resouceIds)
        {
            _relationRepertory.Delete(m => m.RoleId == id);
            var relations = resouceIds.Select(resouceId => new ErpPowerRoleResourceRelation
            {
                Id = Guid.NewGuid().ToString("N").ToUpper(),
                RoleId = id,
                ResourceId = resouceId
            }).ToList();
            _relationRepertory.Insert(relations);
            RemoveRelationsByRole(id);
            RemoveMenuByRole(id);
            RemovePowerByRole(id);
            return true;
        }

        private IEnumerable<ErpPowerRole> Roles()
        {
            if (!_redisCacheService.IsSet(RedisKey.RoleKey))
            {
                _redisCacheService.Add(RedisKey.RoleKey, _repertory.GetAll());
            }
            return _redisCacheService.Get<List<ErpPowerRole>>(RedisKey.RoleKey);
        }

        private void RemoveRole()
        {
            var key = RedisKey.RoleKey;
            if (_redisCacheService.IsSet(key))
            {
                _redisCacheService.Remove(key);
            }
        }

        private List<ErpPowerRoleResourceRelation> GetRelationsByRole(string roleId)
        {
            var key = RedisKey.RoleKey + "." + roleId;
            if (!_redisCacheService.IsSet(key))
            {
                _redisCacheService.Add(key, _relationRepertory.GetByRole(roleId).ToList());
            }
            return _redisCacheService.Get<List<ErpPowerRoleResourceRelation>>(key);
        }

        private void RemoveRelationsByRole(string roleId)
        {
            var key = RedisKey.RoleKey + "." + roleId;
            if (_redisCacheService.IsSet(key))
            {
                _redisCacheService.Remove(key);
            }
        }

        private List<ErpPowerResource> GetResources()
        {
            const string key = RedisKey.PowerKey;
            if (!_redisCacheService.IsSet(key))
            {
                _redisCacheService.Add(key, _resourceRepertory.GetAll());
            }
            return _redisCacheService.Get<List<ErpPowerResource>>(key);
        }

        private IEnumerable<TreeNode> GetPowerByRole(string roleId)
        {
            var key = RedisKey.PowerByRoleKey + "." + roleId;
            if (_redisCacheService.IsSet(key)) return _redisCacheService.Get<List<TreeNode>>(key);
            var resources = GetResources();
            var powerForRole = GetRelationsByRole(roleId);
            var query = from m in resources
                        join p in powerForRole on m.Id equals p.ResourceId
                            into ss
                        from s in ss.DefaultIfEmpty()
                        select new TreeNode
                        {
                            id = m.Id.ToString(),
                            pId = m.ParentFuncId.ToString(),
                            name = m.DisplayName,
                            ischecked = (s != null)
                        };
            _redisCacheService.Add(key, query.ToList());
            return _redisCacheService.Get<List<TreeNode>>(key);
        }

        public void RemovePowerByRole(string roleId)
        {
            RemoveRelationsByRole(roleId);
            var key = RedisKey.PowerByRoleKey + "." + roleId;
            if (_redisCacheService.IsSet(key))
            {
                _redisCacheService.Remove(key);
            }
        }

        private IEnumerable<ErpPowerResource> GetMenuByRole(string roleId)
        {
            var key = RedisKey.MenuByRoleKey + "." + roleId;
            if (_redisCacheService.IsSet(key)) return _redisCacheService.Get<List<ErpPowerResource>>(key);
            var resources = GetResources();
            var powerForRole = GetRelationsByRole(roleId);
            var query = from power in powerForRole
                        join resource in resources on power.ResourceId equals resource.Id
                            into ss
                        from s in ss.DefaultIfEmpty()
                        where s != null
                        select new ErpPowerResource
                        {
                            Id = s.Id,
                            Action = s.Action,
                            Controller = s.Controller,
                            Display = s.Display,
                            DisplayName = s.DisplayName,
                            Orders = s.Orders,
                            ParentFuncId = s.ParentFuncId,
                            Path = s.Path,
                            Type = s.Type,
                            Logo = s.Logo
                        };

            _redisCacheService.Add(key, query.ToList());
            return _redisCacheService.Get<List<ErpPowerResource>>(key);
        }

        public void RemoveMenuByRole(string roleId)
        {
            RemoveRelationsByRole(roleId);
            var key = RedisKey.MenuByRoleKey + "." + roleId;
            if (_redisCacheService.IsSet(key))
            {
                _redisCacheService.Remove(key);
            }
        }
    }
}