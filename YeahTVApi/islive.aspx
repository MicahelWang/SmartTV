<%@ Page Language="C#" AutoEventWireup="true" %>

提供给IT监控系统的“存活监测探针文件”，请勿删除

    <% 


        //DateTime time = DateTime.Parse("2014-04-15 16:57");
        //var Header = new YeahTVApi.Entity.RequestHeader()
        //{
        //    APP_ID = "195F8175-D7B6-4173-8CE4-829D7F29AAE5",
        //    DEVNO = "00:7f:0e:07:26:0b",
        //    Platform = "TV",

        //};
        //YeahTVApi.Entity.FunResult result = new YeahTVApi.Manager.BusinessAttributes.AlarmClock().SetMorningCall(Header, time);
        NBearLite.Database.Default.ExecuteNonQuery(System.Data.CommandType.Text, "SELECT GETDATE()");
        Response.End();
         %>

  