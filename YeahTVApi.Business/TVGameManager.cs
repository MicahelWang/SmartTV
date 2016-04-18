

namespace HZTVApi.Business
{
    using HZTVApi.Infrastructure; 
    using System.Collections.Generic; 
    using HZTVApi.DomainModel.Models;
    using HZTVApi.DomainModel.SearchCriteria;
    using HZTVApi.Infrastructure.BusinessInterface;
    public class TVGameManager : ITVGameManager
    {
        private ITVGameRepertory tvGameRepertory;
        public TVGameManager(ITVGameRepertory tvGameRepertory)
        {
            this.tvGameRepertory = tvGameRepertory;
        }

        /// <summary>
        /// 获取游戏列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId">category等于0是查询全部</param>
        /// <returns></returns>
        public List<TVGame> GetGameList(int pageSize, int pageIndex,int categoryId)
        {
            var criteria = new TVGameCriteria();
            criteria.PageIndex = pageIndex;
            criteria.PageSize = pageSize;
            criteria.NeedPaging = true;
            criteria.CategoryId = categoryId;
            return tvGameRepertory.SearchGames(criteria);
        }
    }
}
