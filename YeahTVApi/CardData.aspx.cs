namespace HZTVApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using HZTVApi.Manager;
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;
    using HZTVApi.Common;

    public partial class CardData : System.Web.UI.Page
    {
        private ITraceManager traceMagager;
        private ICheckInManager checkInManager;

        public CardData(ITraceManager traceMagager, ICheckInManager checkInManager)
        {
            this.traceMagager = traceMagager;
            this.checkInManager = checkInManager;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String CardSnr;
            System.Collections.Specialized.NameValueCollection collection = null;
            if (this.Request.RequestType == "GET")
            {
                collection = Request.QueryString;
            }
            else
            {
                collection = Request.Form;

            }
            Entity.RequestHeader header = new Entity.RequestHeader();
            header.DEVNO = collection["devNo"];
            header.Platform = "TV";
            header.Ver = "1.0";
            CardSnr = collection["CardSN"];
            ApiListResult<HZTVApi.Entity.CardData> data = new ApiListResult<HZTVApi.Entity.CardData>();
            try
            {
                data = checkInManager.GetCardData(traceMagager.GetHotelID(header), traceMagager.GetRoomNo(header), CardSnr);
            }
            catch(Exception err) {
                data.WithError(err.Message);
            }
            string rstRst = data.ToJsonString();
            Response.Write(rstRst);
            Response.ContentType = "application/json";
        }
    }
}
