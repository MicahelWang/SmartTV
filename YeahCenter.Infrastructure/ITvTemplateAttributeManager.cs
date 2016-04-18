using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;

namespace YeahCenter.Infrastructure
{
    public interface ITvTemplateAttributeManager
    {
        TvTemplateAttribute GetById(string id);

        void Delete(string id);

        string Add(TvTemplateAttribute entity);

        void Update(TvTemplateAttribute entity);

        List<TvTemplateAttribute> GetAttributes(string elementId);

        List<TvTemplateAttribute> GetAttributesByPrentId(string elementId,string parrentId);

    }
}