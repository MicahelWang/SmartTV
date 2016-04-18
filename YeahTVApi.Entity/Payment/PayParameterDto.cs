using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HZTVApi.Entity.Payment
{
    public class PayParameterDto
    {
        #region 调用者信息
        public string cal_notifydata { get; set; }

        /// <summary>
        /// 支付请求号
        /// </summary>
        public string pay_uniqueid { get; set; }

        /// <summary>
        /// 支付款项的业务类型
        /// </summary>
        public string pay_netway { get; set; }

        public string pay_amount { get; set; }

        #endregion 


        #region 订单信息

        /// <summary>
        /// pms订单号 
        /// </summary>
        public string res_resno { get; set; }
        /// <summary>
        /// 会员vno，
        /// </summary>
        public string res_vno { get; set; }
        /// <summary>
        /// CRS中央库订单号
        /// </summary>
        public string res_cresno { get; set; }
        /// <summary>
        /// 酒店id
        /// </summary>
        public string res_hotelid { get; set; }
        /// <summary>
        /// 支付交易号
        /// </summary>
        public string pay_id { get; set; }

   
        /// <summary>
        /// //支付方式 A=支付宝，T=财付通 
        /// </summary>
        public string pay_paygate { get; set; }
        /// <summary>
        /// 支付金额，最小支付单位：分
        /// </summary>
        public string res_amount { get; set; }
        /// <summary>
        /// 担保类型：A首夜，B全额
        /// </summary>
        public string res_assurtype { get; set; }

        #endregion


        /// <summary>
        /// 上次异步通知页返回的步骤号，内容为数字，因为9代表成功，所以不成功时候，数字的范围值是0-8，等于零时表示第一
        /// </summary>
        public int laststep { get; set; }

        public string Result { get; set; }

        public string notify_id { get; set; }
    }
}
