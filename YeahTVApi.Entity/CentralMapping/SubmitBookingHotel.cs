using System.Linq;

using System;
using System.Collections.Generic;


namespace YeahTVApi.Entity.CentralMapping
{
   

    #region Result

    /// <summary>
    /// 提交预定表格结果对象
    /// </summary>
    public class SubmitBookingHotelResult : OperationResult
    {
        public BookingResult BookingResult { get; set; }
        public BookingType BookingType { get; set; }
    }
    #endregion
}
