using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YeahCenter.RefreshCacheService
{
    public partial class RefreshCacheService : ServiceBase
    {
        string sSource = "RefreshCache";
        private static WaitHandle ReadDataHandle { get; set; }
        private static RegisteredWaitHandle taskWait;

        public RefreshCacheService()
        {
            InitializeComponent();

            ReadDataHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            this.AutoLog = false;
            using (SettingHelper setting = new SettingHelper())
            {
                sSource = setting.ServiceName;
            } 
            string sLog = "Application";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            //EventLog.WriteEntry(sSource, sEvent,EventLogEntryType.Warning, 234);
        }

        protected override void OnStart(string[] args)
        {
            WriteLog(sSource+" 服务启动");

            taskWait = ThreadPool.UnsafeRegisterWaitForSingleObject(ReadDataHandle, (x, b) => { RequestInterface(Service.Default.RunTime, Service.Default.RequestUrl); }, null, 30 * 1000, true);
        }
        private void RequestInterface(TimeSpan timeSpan, string requestUrl)
        {
            try
            {
                GetResponseString(requestUrl);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            finally
            {
                var tomorrow = DateTime.Now.Date.AddDays(1);
                var nextRunTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                var waitTime = Convert.ToInt32((nextRunTime - DateTime.Now).TotalMilliseconds);
                WriteLog(string.Format("请求刷新缓存成功!下次执行时间：{0} 还剩{1}毫秒！", nextRunTime, waitTime));
                taskWait = ThreadPool.UnsafeRegisterWaitForSingleObject(ReadDataHandle, (x, b) => { RequestInterface(timeSpan, requestUrl); }, null, waitTime, true);
            }
        }

        protected override void OnStop()
        {
            WriteLog(sSource+" 服务停止");
            if (taskWait != null)
                taskWait.Unregister(ReadDataHandle);
        }

        private void GetResponseString(string url, int runTotal = 0)
        {
            bool isRequestSuccess = false;
            try
            {
                var httpClient = new HttpClient();
                var result = httpClient.GetStringAsync(url).Result;
                if (string.IsNullOrWhiteSpace(result) || result.ToLower() != "success")
                {
                    isRequestSuccess = false;
                    WriteLog(string.Format("调用接口操作 失败：{0} URL：{1}", result, url));
                }
                else
                {
                    isRequestSuccess = true;
                    WriteLog(string.Format("调用接口操作 成功  URL：{0}！", url));
                }
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("调用接口操作 URL：{0} 异常：{1}  ", url, ex.Message));
            }
            if (!isRequestSuccess)
            {
                if (runTotal <= 5)
                {
                    runTotal++;
                    Thread.Sleep(1000 * 60 * runTotal);
                    GetResponseString(url, runTotal);
                }
            }
        }

        private void WriteLog(string message)
        {
            EventLog.WriteEntry(sSource, message);
        }
    }
}
