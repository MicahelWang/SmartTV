using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using HZTVApi.Common;
using HZ.Web.Authorization;

namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 酒店详情转换类
    /// </summary>
    public class QueryMemberInfoAttribute:BusinessAttribute
    {
         /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
         public QueryMemberInfoAttribute()
             : base(typeof(CentralApiResult<QueryMemberResult>))
        {
            
        }

        public override Object ConvertTo(string json,Guest gst)
        {
            var result = base.ConvertTo(json, gst);
            if (result == null)
                return null;
            var data = result as CentralApiResult<QueryMemberResult>;
            if (data == null)
                return null;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
                throw new ApiException(data.Message);
            }
            Guest guest = new Guest();
            var detail =  data.Data.MemberInfoDetail;
            if(detail!= null)
            {
                guest.MemberID =detail.MemberId;
                guest.MemberLevelID = detail.MemberLevelID;
                guest.VNo = detail.DefaultVCardNo;
                guest.MemberLevelDesc = detail.MemberLevelDesc;
                guest.Mobile = detail.Mobile;
                guest.Name = detail.Name;
                guest.Email = detail.Email;
                guest.idCode = detail.IDNo;
                guest.idType = detail.IDType;
                guest.sex = detail.Gender;
                //guest.idTypeDesc = DictManager.GetDictName(DictManager.DictType.CardType, detail.IDType);
                guest.birthDay = detail.Birthday.ToString("yyyy-MM-dd");
                //积分
                guest.Point = detail.Point.ToString();
                //储值卡余额
                guest.CardCreditValue = detail.Value.ToString();
                //可用的优惠券张数
               
               // var dr = base.apiDBManager.GetMemberInfoEx(guest.VNo);
                //try
                //{

                //    var info = MemberInfoProcess.BLL.PersonCRM.CrmCustomAccount(guest.VNo);
                //    if (info != null)
                //        guest.CompanyContactName = info.Contacter;
                //}
                //catch { }
               
            }
            return guest;

        }

       

        public Guest Query(BaseRequestData data)
        {
            
            Dictionary<String, String> pams = new Dictionary<string, string>();
            //完成用户在大促销环境中的登录操作。
            //结果封装并返回
            DateTime StartTime = DateTime.Now;
            var response = CentralApi.GetResponse(APICallFactory.CallAction(APICallFactory.APICallType.QueryMemberInfo), pams, data.TOKEN, data.language);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            Guest guest= ConvertTo(responseJson,null) as Guest;
            return guest;
        }
        
    }
}
