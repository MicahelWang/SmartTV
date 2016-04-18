using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.Manager
{
    public class ScoreExchangManager : BaseManager<ScoreExchang, ScoreExchangCriteria>, IScoreExchangManager
    {
        private readonly IScoreExchangRepertory _scoreExchangRepertory;

        public ScoreExchangManager(IScoreExchangRepertory scoreExchangRepertory)
            : base(scoreExchangRepertory)
        {
            this._scoreExchangRepertory = scoreExchangRepertory;
        }

        public void AddOrUpdateScoreExchang(ScoreExchang scoreExchang)
        {
            var isAdd = false;
            var exist = _scoreExchangRepertory.Search(new ScoreExchangCriteria()
            {
                OrderId = scoreExchang.OrderId
            }).FirstOrDefault();

            if (exist == null)
            {
                isAdd = true;
                exist = new ScoreExchang();
            }

            scoreExchang.CopyTo(exist, new[] { "Id" });
            exist.LastUpdateTime = DateTime.Now;

            if (isAdd)
                _scoreExchangRepertory.Insert(exist);
            else
                _scoreExchangRepertory.Update(exist);

        }
    }
}
