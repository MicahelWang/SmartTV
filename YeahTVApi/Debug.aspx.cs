using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HZTVApi.Entity;
using HZTVApi.Manager;
using HZTVApi.Common;
using HZTVApi.Filter;


using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;
using HZTVApi.ServiceProvider.MemberService;

namespace HZTVApi
{
    public partial class Debug : System.Web.UI.Page
    {
        private IMemberService memberService;

        public Debug(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Business.MemberService.MemberServiceClient client = new Business.MemberService.MemberServiceClient();
            //String cardno, message;
            //int i = client.RegisterNetMember(new Business.MemberService.PersonMember()
            //{
            //    MemberLevel = "B",
            //    Mobile = "13916373785",
            //    Password = "123321a",
            //    Name = "江斌"
            //}, null, out cardno, out message);

            //GuestManager.GetRoomDetail("2011039", "1609");
            //return;
            //BaseRequestData bd = new HZTVApi.Entity.BaseRequestData();
            //bd.Platform = "android";
            //bd.ver = "4.0";
            //bd.devNo = "B9496233-8D10-4021-9815-A9DBA64312B9";
            //bd.TOKEN = "app9c400d54-b177-4d2f-ab97-a06a357b5020";

            //DateTime time = DateTime.Parse("2014-08-05 17:17");
            //var Header = new RequestHeader()
            //{
            //    APP_ID = "ADD3CCB6-7C46-43B5-9889-9449656DAA5B",
            //    DEVNO = "44:33:4c:81:9d:94",
            //    Platform = "TV",
            //    Ver = "1.0"
            //};
            //Header.DEVNO = "44:33:4c:74:85:db";//3d
            //GuestManager.GetRoomDetail(apiDBManager.GetHotelID(header), apiDBManager.GetRoomNo(header));
            //Header.DEVNO = "5c:ff:35:8e:15:4b";
            //AppManager.GetAppStartConfig(Header.ToBaseRequestData(), Header, "http://app1.htinns.com");
            //var list = new GetGuestInfo().GetInfoList(apiDBManager.GetHotelID(header), apiDBManager.GetRoomNo(header));



        }
    }
}
