namespace YeahTVApi.Infrastructure
{
    public interface ISelfServiceManager
    {
        /// <summary>
        /// 是否可以退房
        /// </summary>
        bool IsCheckOutByReceiveID(string sReceiveID);
        /// <summary>
        /// 续住
        /// </summary>
        bool ContinueToLive(string vNumber, string hotelId, string receiveOrderId, int continueDays,out string message);
        /// <summary>
        /// 退房
        /// </summary> 
        bool CheckOut(string vNumber, string hotelId, string receiveOrderId, out string message);
    }
}
