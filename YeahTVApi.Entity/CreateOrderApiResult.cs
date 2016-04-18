namespace YeahTVApi.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Newtonsoft;
    using Newtonsoft.Json;

    /// <summary>
    /// 结果接口
    /// </summary>
    public class CreateOrderApiResult : ApiResult
    {
        [JsonProperty("roomNo")]
        public string RoomNum { get; set; }

        [JsonProperty("message")]
        public string MessageInfo { get; set; }
        [JsonProperty("resultCode")]
        public int ResultCode { get; set; }
        [JsonProperty("sendRoom")]
        public bool SendRoom { get; set; }
        public CreateOrderApiResult()
            : base()
        {
        }

        /// <summary>
        /// 结果为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="RoomNum"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码与结果信息</returns>
        public CreateOrderApiResult WithError(string message, string RoomNum, int code = -1)
        {
            this.RoomNum = RoomNum;
            this.ResultCode = code;
            this.MessageInfo = message;
            return this;
        }

        /// <summary>
        /// 结果为正确
        /// </summary>
        /// <param name="RoomNum"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码</returns>
        public CreateOrderApiResult WithOk(string RoomNum, int code = 0)
        {
            this.ResultCode = code;
            this.RoomNum = RoomNum;
            return this;
        }

        /// <summary>
        /// 结果为正确
        /// </summary>
        /// <param name="message"></param>
        /// <param name="RoomNum"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码</returns>
        public CreateOrderApiResult WithOk(string message, string RoomNum, bool sendRoom,int code = 0)
        {
            this.RoomNum = RoomNum;
            this.ResultCode = code;
            this.MessageInfo = message;
            this.SendRoom = sendRoom;
            return this;
        }
    }
}
