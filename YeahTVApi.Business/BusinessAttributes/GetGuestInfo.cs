using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using HZTVApi.Common;
using HZ.Web.Authorization;
using System.Data;

namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 
    /// </summary>
    public class GetGuestInfo : BusinessAttribute
    {
        public GetGuestInfo()
            : base(null)
        {

        }
        public Guest GetInfo(RequestHeader header)
        {
            Guest guest = null;
            PMS.PmsServiceClient client = null;
            try
            {
                client = new PMS.PmsServiceClient();
                var list = client.GetRcpnSheetInfoByRoomNo(ApiDBManager.GetHotelID(header), ApiDBManager.GetRoomNo(header));
                if (list != null && list.Length > 0)
                {
                    guest = new Guest();
                    guest.MemberID = list[0].MemberID;
                    if (!String.IsNullOrEmpty(guest.MemberID))
                        guest.TOKEN = Common.PubFun.MD5Encrypt(list[0].MemberID, null);
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

        public List<Guest> GetInfoList(String HotelId, String RoomNo)
        {
            List<Guest> list = new List<Guest>();
            PMS.PmsServiceClient client = null;
            try
            {
                client = new PMS.PmsServiceClient();
                var result = client.GetRcpnSheetInfoByRoomNo(HotelId, RoomNo);

                if (result != null)
                {
                    MemberService.MemberServiceClient msc = new MemberService.MemberServiceClient();
                    Dictionary<String, String> dict = new Dictionary<string, string>();
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

                            var member = msc.GetMemberInfoByCardNo(guest.MemberID);
                            //note:添加member非空的判断
                            if (null != member)
                            {
                                guest.IsMember = true;

                                //获取会员等级
                                guest.MemberLevelID = member.MemberLevel;
                                guest.MemberLevelDesc = GetMemberLevelName(member.MemberLevel);
                                if (member.Point != null && !guest.IsCompanyMember())
                                {
                                    guest.Point = ((int) member.Point).ToString();
                                    guest.Point = guest.Point.Substring(0, 1) +
                                                  RepeatString("*", guest.Point.Length - 1);
                                }
                                guest.MemberHint = NextLevelHint(member.MemberID);
                            }
                            else
                            {
                                guest.IsMember = false;
                            }
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
                                        guest.Point = ((int)(System.Decimal)row["Point"]) + "";
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
                                    var row = dt1.Tables[0].Rows[0];
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
            catch (Exception err)
            {
                if (System.Web.HttpContext.Current != null)
                    HTOutputLog.SaveError(System.Web.HttpContext.Current.Request.Url.ToString(), err.Message + err.StackTrace);
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

        public static String GetMemberLevelName(String memberLevel)
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


        private String RepeatString(String s, int count)
        {
            String returnStr = "";
            for (int i = 0; i < count; i++)
            {
                returnStr += s;
            }
            return returnStr;
        }

        public string NextLevelHint(string memberID)
        {
            try
            {
                MemberExtraService.MemberExtraServiceClient client = new MemberExtraService.MemberExtraServiceClient();
                return client.GetMemberRoomdayMsg(memberID);
            }
            catch (Exception err)
            {
                HTOutputLog.SaveError("NextLevelHint_ERROR", err);
                return string.Empty;
            }


        }

    }
}
