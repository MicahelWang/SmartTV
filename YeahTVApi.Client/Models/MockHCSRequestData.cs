using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YeahTVApi.Client.Models
{
    public class MockHCSRequestData
    {
        //public string ServerId { get; set; }

        //public string Sign { get; set; }

        //public string Data { get; set; }

        public MockHCSRequestData()
        {
            List<string> itemList = new List<string>();

            itemList.Add("请求下载任务接口");
            itemList.Add("业务通知接口");
            itemList.Add("系统错误通知接口");
            itemList.Add("测试数据通知接口");
            itemList.Add("性能数据传输接口");
            itemList.Add("获取HCS全局变量配置接口");

            SelectList list = new SelectList(itemList);

            ApiNameList = list;
        }

        [DisplayName("测试API")]
        public string ApiName { get; set; }

        public IEnumerable<SelectListItem> ApiNameList { get; set; }

        [DisplayName("请求URL")]
        public string Url { get; set; }

        [DisplayName("请求主体")]
        public string Body { get; set; }

        [DisplayName("私钥")]
        public string PrivateKey { get; set; }

        [DisplayName("应答消息")]
        public string ResponseData { get; set; }
    }
}