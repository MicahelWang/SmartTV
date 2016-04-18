using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class CurrentUser
    {
        public string UID { get; set; }
        public string Account { get; set; }
        public string ChineseName { get; set; }
        public string RoleId { get; set; }
        
        public string RoleType { get; set; }
        public int? UserType { get; set; }

        public string Token { get; set; }
        public IList<ErpPowerResource> FunList { get; set; }


        /// <summary>
        /// 验证当前用户是否权限
        /// </summary>
        /// <param name="funcId"></param>
        /// <returns></returns>
        public bool CheckPermission(int funcId)
        {
            return FunList.Any(_ => _.Id == funcId);
        }
    }
}
