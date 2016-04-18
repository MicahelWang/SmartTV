namespace YeahTVApi.ServiceProvider
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class GetGuestInfoService : IGetGuestInfoService
    {
        private ILogManager logManager;

        public GetGuestInfoService(ILogManager logManager)
        {
            this.logManager = logManager;
        }

        public Guest GetInfo(string HotelId, string RoomNo)
        {
            Guest guest = null;
            PMS.PmsServiceClient client = null;
            try
            {
                client = new PMS.PmsServiceClient();
                var list = client.GetRcpnSheetInfoByRoomNo(HotelId, RoomNo);
                if (list != null && list.Length > 0)
                {
                    guest = new Guest();
                    guest.MemberID = list[0].MemberID;
                    if (!String.IsNullOrEmpty(guest.MemberID))
                        guest.TOKEN = Common.PubFun.MD5Encrypt(list[0].MemberID,null);
                    guest.Name = list[0].CusName;
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                    client = null;
                }
            }
            return guest;
        }

        public List<Guest> GetInfoList(String HotelId,String RoomNo)
        {
            var list = new List<Guest>();
            PMS.PmsServiceClient client = null;
            try
            {
                client = new PMS.PmsServiceClient();
                var result = client.GetRcpnSheetInfoByRoomNo(HotelId, RoomNo);

                if (result != null)
                {
                    MemberService.MemberServiceClient msc = new MemberService.MemberServiceClient();
                    Dictionary<String,String> dict= new Dictionary<string,string>();
                    foreach (var item in result)
                    {
                        if (item.RecStatusID != "I" || dict.ContainsKey(item.ReceiveID))
                            continue;
                        dict.Add(item.ReceiveID, null);
                        Guest guest = new Guest();
                        guest.MemberID = item.MemberID;
                        guest.TOKEN = item.ReceiveID;
                        guest.Name = item.CusName;
                        guest.Mobile = item.CusTel;
                        guest.idCode = item.CardNo;
                        guest.idType = item.IDType;
                        guest.sex = item.Sex;
                        guest.ReceiveID = item.ReceiveID;
                        guest.InmateRecID = item.InmateRecID;
                        if (String.IsNullOrEmpty(guest.MemberID))
                        {
                            guest.IsMember = false;
                        }
                        else if (!String.IsNullOrEmpty(guest.MemberID) && result.Length == 1)
                        {
                            guest.IsMember = true;
                            var member = msc.GetMemberInfoByCardNo(guest.MemberID);
                          
                            
                            //获取会员等级
                            guest.MemberLevelID = member.MemberLevel;
                            guest.MemberLevelDesc = GetMemberLevelName(member.MemberLevel);
                            if (member.Point != null && !guest.IsCompanyMember())
                            {
                                guest.Point = ((int)member.Point).ToString();
                                guest.Point = guest.Point.Substring(0, 1) + RepeatString("*", guest.Point.Length - 1);
                            }
                            guest.MemberHint =NextLevelHint(member.MemberID);
                           
                        }
                        else
                        {
                        
                            var dt2 = msc.SearchByOnly(null, item.IDType, item.CardNo, null);

                            if (dt2 != null && dt2.Tables.Count > 0 && dt2.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow row in dt2.Tables[0].Rows)
                                {
                                    guest.IsMember = true;
                                    guest.MemberLevelID = row["MemberLevel"].ToString();
                                    guest.MemberID = row["MemberID"].ToString();
                                    guest.MemberLevelDesc = GetMemberLevelName(guest.MemberLevelID);
                                    if (row["Point"] != DBNull.Value && !guest.IsCompanyMember())
                                    {
                                        guest.Point = ((int)(System.Decimal)row["Point"])+"";
                                        guest.Point = guest.Point.Substring(0, 1) + RepeatString("*", guest.Point.Length - 1);
                        
                                    }
                                     guest.MemberHint = NextLevelHint(guest.MemberID);
                                }

                            }
                            else
                            {
                                var dt1 = msc.SearchByOnly(item.CusTel, item.IDType, item.CardNo, null);
                                if (dt1 != null && dt1.Tables.Count > 0 && dt1.Tables[0].Rows.Count > 0)
                                {
                                    guest.IsMember = true;
                                    var row=  dt1.Tables[0].Rows[0];
                                    guest.MemberHint = "手机号关联到会员[" + row["name"].ToString() + "],但证件信息不一致将不能获取积分。";
                                   
                                }
                                else
                                {
                                    guest.IsMember = false;
                                }
                            }
                        }                       
                        list.Add(guest);
                    }
                    foreach (Guest guest in list)
                    {
                        if (!String.IsNullOrEmpty(guest.Mobile))
                        {
                            guest.TOKEN = PubFun.MD5Encrypt(guest.Mobile, null);
                        }
                        else
                        {
                            guest.TOKEN = PubFun.MD5Encrypt(guest.idCode, null);
                        }

                    }
                    dict.Clear();
                    dict = null;
                    msc.Close();
                    msc = null;
                }
               
            }
            catch(Exception err)
            {
                logManager.SaveError("GetInfoList", err.Message + err.StackTrace, AppType.TV);
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                    client = null;
                }
            }
            return list;
        }

        public string GetMemberLevelName(String memberLevel)
        {
            if (string.Equals("A", memberLevel, StringComparison.CurrentCultureIgnoreCase))
            {
                return "银会员";
            }
            else if (string.Equals("B", memberLevel, StringComparison.CurrentCultureIgnoreCase))
            {
                return "金会员";
            }
            else if (string.Equals("E", memberLevel, StringComparison.CurrentCultureIgnoreCase))
            {
                return "订房卡";
            }
            else if (string.Equals("I", memberLevel, StringComparison.CurrentCultureIgnoreCase))
            {
                return "铂金会员";
            }
            else if (string.Equals("P", memberLevel, StringComparison.CurrentCultureIgnoreCase))
            {
                return "星会员";
            }
            else if (string.Equals("@", memberLevel, StringComparison.CurrentCultureIgnoreCase))
            {
                return "海友会员";
            }
            return "公司卡";
        }

        public string NextLevelHint(string memberID)
        {
            try
            {
                var client = new MemberExtraService.MemberExtraServiceClient();
                return client.GetMemberRoomdayMsg(memberID);
            }
            catch (Exception err)
            {
                logManager.SaveError("NextLevelHint_ERROR", err, AppType.TV);
                return string.Empty;
            }
        }

        public bool MobileIsMember(string mobile)
        {
            var msc = new MemberService.MemberServiceClient();
            return msc.IsHtMemberByMobile(mobile);

        }

        private string RepeatString(string s, int count)
        {
            String returnStr = "";
            for (int i = 0; i < count; i++)
            {
                returnStr += s;
            }
            return returnStr;
        }
    }
}
