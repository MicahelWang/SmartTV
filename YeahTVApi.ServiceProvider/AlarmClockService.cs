namespace YeahTVApi.ServiceProvider
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using YeahTVApi.ServiceProvider.PMS;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AlarmClockService : IAlarmClockService
    {
        private IMongoLogManager mongoLogManager;
        private IHttpContextService httpContextService;
        
        public AlarmClockService(IMongoLogManager mongoLogManager)
        {
            this.mongoLogManager = mongoLogManager;
        }

        public FunResult SetMorningCall(DateTime? setTime, string hotelId, string roomNo)
        {
            FunResult fr = new FunResult();
            PMS.PmsServiceClient client = null;
            try
            {
                client = new PMS.PmsServiceClient();
                var list = client.GetMorningCallByRoomNo(hotelId, roomNo);
                Boolean exist = false;
                //先将历史的时间清空，然后再设置新的闹钟时间
                if (list != null)
                {
                    var receives = new List<string>();
                    foreach (var item in list)
                    {
                        if (item.StatusID != "01")
                        {
                            continue;
                        }
                        else if (setTime != null && item.CallTime == setTime)
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
                        client.CancelMorningCall(hotelId, roomNo, receives.ToArray());
                    }
                }

                if (!exist && setTime != null)
                {

                    var result = client.SetMorningCall(hotelId, roomNo, setTime.Value);
                    if (!result.Success)
                    {
                        fr.WithError(result.ErrogMsg);
                    }
                }
            }
            catch (Exception err)
            {
                mongoLogManager.SaveError(err.Message + err.StackTrace, err, AppType.TV, this.GetType().ToString());
                fr.WithError(err.Message);
            }
            return fr;
        }

        public DateTime? GetMorningCall(string hotelId, string roomNo)
        {
            PMS.PmsServiceClient client = null;
            MorningCallDataContract[] result = null;
            try
            {
                client = new PMS.PmsServiceClient();
                result = client.GetMorningCallByRoomNo(hotelId, roomNo);
                if (result != null)
                {
                    var list = new List<PMS.MorningCallDataContract>();
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
            return result != null && result.Count() > 0 ? result[0].CallTime as Nullable<DateTime> : null;
        }
    }
}
