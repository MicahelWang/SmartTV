namespace HZTVApi.Manager
{
    using HZTVApi.Infrastructure;

    /// <summary>
    /// 
    /// </summary>
    public class SelfServiceManager : ISelfServiceManager
    {
        private ISelfServiceService selfServiceService;
        
        public SelfServiceManager(ISelfServiceService selfServiceService)
        {
            this.selfServiceService = selfServiceService;
        }

        /// <summary>
        /// 根据接待单号查询该房间是否可以办理退房
        /// </summary>
        /// <param name="sReceiveID">接待单号</param>
        /// <returns></returns>
        public bool IsCheckOutByReceiveID(string hotelId,string sReceiveID)
        {
            bool flag = false;
            //PMSICRS.ICrsSoap clientSoap = new CrsSoapClient();
            //PMSICRS.IsCheckOutByReceiveIDRequest request = new IsCheckOutByReceiveIDRequest();
            //PMSICRS.IsCheckOutByReceiveIDRequestBody body = new IsCheckOutByReceiveIDRequestBody();
            //body.sReceiveID = sReceiveID;
            //request.Body = body;
            //PMSICRS.IsCheckOutByReceiveIDResponse response = clientSoap.IsCheckOutByReceiveID(request);
            //flag = response.Body.IsCheckOutByReceiveIDResult;
            return flag;
            return selfServiceService.IsCheckOutByReceiveID(hotelId, sReceiveID);
        }

        /// <summary>
        /// 续住
        /// </summary>
        public bool ContinueToLive(string vNumber, string hotelId, string receiveOrderId, int continueDays, out string message)
        {
            SelfServiceAttribute selfService = new SelfServiceAttribute();
            return selfService.GetReceiveOrderRequestResult(hotelId, vNumber, receiveOrderId, continueDays, out message);
        }
        /// <summary>
        /// 退房
        /// </summary>
        public bool CheckOut(string vNumber, string hotelId, string receiveOrderId, out string message)
        {
            SelfServiceAttribute selfService = new SelfServiceAttribute();
            return selfService.GetReceiveOrderSelfCheckout(hotelId, vNumber, receiveOrderId,  out message);
            selfServiceService.GetReceiveOrderRequestResult(hotelId, vNumber, receiveOrderId, continueDays);
        }


    }
}
