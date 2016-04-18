using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using HZTVApi.Common;
using HZ.Web.Authorization;
using HZTVApi.Infrastructure;

namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmClock : BusinessAttribute
    {
        public AlarmClock()
            : base(null)
        {
        }

        public FunResult SetMorningCall(RequestHeader header, DateTime? Time)
        {
            FunResult fr = new FunResult();
            PMS.PmsServiceClient client = null;
            try
            {
                client = new PMS.PmsServiceClient();
                var list = client.GetMorningCallByRoomNo(ApiDBManager.GetHotelID(header), ApiDBManager.GetRoomNo(header));
                Boolean exist = false;
                //先将历史的时间清空，然后再设置新的闹钟时间
                if (list != null)
                {
                    List<String> receives = new List<string>();
                    foreach (var item in list)
                    {
                        if (item.StatusID != "01")
                        {
                            continue;
                        }
                        else if (Time != null && item.CallTime == Time)
                        {
                            exist = true;
                        }
                        else
                        {
                            receives.Add(item.McID);

                        }
                    }
                    if (receives.Count > 0)
                    {
                        client.CancelMorningCall(ApiDBManager.GetHotelID(header), ApiDBManager.GetRoomNo(header), receives.ToArray());
                    }
                }

                if (!exist && Time != null)
                {

                    var result = client.SetMorningCall(ApiDBManager.GetHotelID(header), ApiDBManager.GetRoomNo(header), Time.Value);
                    if (!result.Success)
                    {
                        fr.WithError(result.ErrogMsg);
                    } 
                }
            }
            catch (Exception err)
            {
                HTOutputLog.SaveError(this.GetType().ToString(), err, err.Message + err.StackTrace);
                fr.WithError(err.Message);
            }
            return fr;
        }

        public HZTVApi.Business.PMS.MorningCallDataContract[] GetMorningCall(RequestHeader header)
        {
            PMS.PmsServiceClient client = null;
            HZTVApi.Business.PMS.MorningCallDataContract[] result = null;
            try
            {
                client = new PMS.PmsServiceClient();
                result = client.GetMorningCallByRoomNo(ApiDBManager.GetHotelID(header), ApiDBManager.GetRoomNo(header));
                if (result != null)
                {
                    List<PMS.MorningCallDataContract> list = new List<PMS.MorningCallDataContract>();
                    list.InsertRange(0, result);
                    result = list.Where(x => x.StatusID == "01").ToArray();
                }
            }
            catch { }
            finally
            {
                if (client != null)
                    client.Close();
            }
            return result;
        }
    }
}
