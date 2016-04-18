namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using EntityFramework.Extensions;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TagRepertory : BaseRepertory<Tag, int>, ITagRepertory
    {
        public override List<Tag> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as TagCriteria;

            var query = Query(criteria); 
            return query.ToPageList(criteria);
        }


        public List<Tag> SearchALLTagsWithRescource()
        {
            var result = (from item in
                              (from tag in base.Entities.AsQueryable()
                               join localizeResource in base.Context.Set<LocalizeResource>()
                                   on tag.RescorceId equals localizeResource.Id
                               select new
                               {
                                   Icon = tag.Icon,
                                   Id = tag.Id,
                                   ParentId = tag.ParentId,
                                   RescorceId = tag.RescorceId,
                                   ResourcesContent = localizeResource.Content,
                                   ResourcesId = localizeResource.Id,
                                   ResourcesLang = localizeResource.Lang
                               }).ToList()
                          group item by item.Id into g
                          select new Tag
                          {
                              Icon = g.FirstOrDefault().Icon,
                              Id = g.FirstOrDefault().Id,
                              ParentId = g.FirstOrDefault().ParentId,
                              RescorceId = g.FirstOrDefault().RescorceId,
                              LocalizeResources = g.Select(s => new LocalizeResource
                              {
                                  Content = s.ResourcesContent,
                                  Id = s.RescorceId,
                                  Lang = s.ResourcesLang
                              })
                          }).ToList();

            return result;
        }

        private IQueryable<Tag> Query(TagCriteria criteria)
        {
            var query = base.Entities.AsQueryable();
            if (criteria.Id != null)
            {
                query = query.Where(q => q.Id == int.Parse(criteria.Id));
            }
            if (!string.IsNullOrEmpty(criteria.RescorceId))
            {
                query = query.Where(q => q.RescorceId == criteria.RescorceId);
            }
            if (criteria.ParentId.HasValue)
            {
                query = query.Where(q => q.ParentId == criteria.ParentId);
            }
            return query;
        }

    }
}
