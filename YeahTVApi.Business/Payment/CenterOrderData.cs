using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Infrastructure.BusinessInterface.Payment;

namespace HZTVApi.Business.Payment
{
    public class CenterOrderData : ICenterOrderData
    {
        public bool ModifyOrderGuaranteeStatus(string assureType, string paytype, string holdTime, string cancelTime, string centerResno)
        {

            const string sql = @"UPDATE B_ResvMain
                                SET    sAssTypeCode = @sAssTypeCode,
                                       sPayTypeCode = @sPayTypeCode,
                                       dModifyTime = Getdate(),
                                       sModifierID = @sModifierID,
                                       dLastHoldTime = @dLastHoldTime,
                                       dLastCancelTime = @dLastCancelTime
                                WHERE  sResvID = @sResvID ";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("sAssTypeCode", assureType);
            parameters.Add("sPayTypeCode", paytype);
            parameters.Add("sModifierID", "Website");
            parameters.Add("dLastHoldTime", holdTime);
            parameters.Add("dLastCancelTime", cancelTime);
            parameters.Add("sResvID", centerResno);
            return DBHelper.RunSQL(DBHelper.DBKind.CenterOrderDb, sql, parameters) > 0;

        }
        /// <summary>
        /// 获得某订单的支付付款记录（支付宝、财付通），可过滤只列成功项。不包括退款记录
        /// </summary>
        /// <param name="apmsResno"></param>
        /// <returns></returns>
        public int Payment_getOrderPayinfos(string apmsResno, string CenterResno)
        {
            const string sql = @"SELECT count(0) 
                                 FROM   B_Payment WITH(nolock)
                                 WHERE  sPMSOrderID = @sPMSOrderID
                                        AND sResvID = @sResvID
                                        AND sPayDirectoinCode = '1'
                                        AND sPayTypeCode IN ( '1', '4', '5', '6', 'point' )
                                        AND sPayStatusCode = '4'; 
                                 ";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("sPMSOrderID", apmsResno);
            parameters.Add("sResvID", CenterResno);
            return (int)DBHelper.ExecScalar(DBHelper.DBKind.CenterOrderDb, sql, parameters);

        }
        /// <summary>
        /// 如果中央库状态未更新，只能说明支付网关业务回调未成功（账务失败），支付网关可能已成功支付
        /// </summary>
        /// <param name="resvNo"></param>
        /// <returns></returns>
        public int CountByPaymentSuccess(string pmsResno)
        {
            const string sql = @"SELECT Count(0)
                                FROM   [dbo].[Payment] WITH(nolock)
                                WHERE  netway = 'WEBRESV'
                                       AND [resvNo] = @pmsResno
                                       AND [paySuccess] = 1; 
                                ";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("pmsResno", pmsResno);
            return (int)DBHelper.ExecScalar(DBHelper.DBKind.CenterOrderDb, sql, parameters);
        }

        public string GetOrderById(string netpayID)
        {
            const string sql = @"SELECT TOP 1 orderid
                              FROM   [dbo].[Payment] WITH(nolock)
                              WHERE  ID = @netpayID; 
                              "; 

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("netpayID", netpayID);
            return DBHelper.ExecScalar(DBHelper.DBKind.CenterOrderDb, sql, parameters).ToString();

        }

        /// <summary>
        /// 获得网站来源订单支付记录的进度号
        /// </summary>
        /// <param name="apayRequestID"></param>
        /// <param name="sPayDirectoinCode"></param>
        /// <returns></returns>
        public int GetPayBizStepCode(string apayRequestID, int sPayDirectoinCode)
        {
            const string sql = @"SELECT TOP 1 sPayBizStepCode
                                 FROM   [B_Payment] WITH(nolock)
                                 WHERE  sRequestID = @sRequestID
                                        AND [sPayDirectoinCode] = @sPayDirectoinCode; 
                                 ";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("sRequestID", apayRequestID);
            parameters.Add("sPayDirectoinCode", sPayDirectoinCode);
            return (int)DBHelper.ExecScalar(DBHelper.DBKind.CenterOrderDb, sql, parameters);
        }
    }
}
