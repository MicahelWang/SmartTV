using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApi.DomainModel;
using YeahTVApi.Entity;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.Manager
{
    public class HCSTaskManager : BaseManager<HCSDownloadTask, HCSTaskCriteria>, IHCSTaskManager
    {
        private IRedisCacheService _redisCacheService;
        private IRequestApiService _requestApiService;
        private IHCSTaskRepertory _hcsTaskRepertory;
        private IHCSJobRepertory _hcsJobRepertory;
        private IHCSConfigRepertory _hcsConfigRepertory;
        private IHotelMovieTraceNoTemplateRepertory _hotelMovieTraceNoTemplateRepertory;
        private IHCSGlobalConfigManager _hcsGlobalConfigManager;
        private ILogManager _logManager;
        private IDeviceTraceLibraryManager _deviceTraceLibraryManager;

        public HCSTaskManager(IRedisCacheService redisCacheService, IRequestApiService requestApiService
                                , IHCSTaskRepertory hcsTaskRepertory, IHCSJobRepertory hcsJobRepertory, IHCSConfigRepertory hcsConfigRepertory
                                , IHotelMovieTraceNoTemplateRepertory hotelMovieTraceNoTemplateRepertory
                                , IHCSGlobalConfigManager hcsGlobalConfigManager, ILogManager logManager
                                , IDeviceTraceLibraryManager deviceTraceLibraryManager) : base(hcsTaskRepertory)
        {
            _redisCacheService = redisCacheService;
            _requestApiService = requestApiService;

            _hcsTaskRepertory = hcsTaskRepertory;
            _hcsJobRepertory = hcsJobRepertory;
            _hcsConfigRepertory = hcsConfigRepertory;

            _hotelMovieTraceNoTemplateRepertory = hotelMovieTraceNoTemplateRepertory;
            _deviceTraceLibraryManager = deviceTraceLibraryManager;

            _hcsGlobalConfigManager = hcsGlobalConfigManager;

            _logManager = logManager;
        }

        //#region redis

        //public List<HCSDownloadTask> GetAllFromCache()
        //{
        //    return _redisCacheService.GetAllFromCache(RedisKey.TaskKey, _hcsTaskRepertory.GetAllWithInclude);
        //}

        ////public List<HCSDownloadTask> GetAllFromCacheByServerId(string serverId)
        ////{
        ////    HCSTaskCriteria criteria = new HCSTaskCriteria();
        ////    criteria.ServerId = serverId;

        ////    return _redisCacheService.GetAllFromCache(RedisKey.TaskKey, () => _hcsTaskRepertory.Search(criteria));
        ////}

        //public void RemoveCache()
        //{
        //    _redisCacheService.Remove(RedisKey.TaskKey);
        //    _redisCacheService.Remove(RedisKey.HotelMovieTraceNoTemplateKey);
        //}

        //private void UpdateCache(HCSDownloadTask task)
        //{
        //    var taskOld = GetAllFromCache().SingleOrDefault(m => m.Id == task.Id);
        //    var taskNew = _hcsTaskRepertory.Search(new HCSTaskCriteria()
        //                                                {
        //                                                    Id = task.Id,
        //                                                }).FirstOrDefault();
        //    _redisCacheService.UpdateItemFromSet(RedisKey.HotelTVChannelDataKey, taskOld, taskNew);
        //}

        //#endregion redis

        [UnitOfWork]
        List<HCSTask> IHCSTaskManager.GetTask(string serverId, string sign, string oldTaskNo)
        {
            HCSTaskCriteria taskCriteria = new HCSTaskCriteria();
            taskCriteria.ServerId = serverId;
            taskCriteria.Type = "VOD";
            taskCriteria.NotSuccessResultStatus = true;

            HCSConfigCriteria configCriteria = new HCSConfigCriteria();
            configCriteria.ServerId = serverId;
            configCriteria.Type = "Task";

            var taskList = Search(taskCriteria).OrderBy(p => p.CreateTime).ToList();
            var config = _hcsGlobalConfigManager.Search(configCriteria).FirstOrDefault();
            if (taskList == null || taskList.Count == 0)
            {
                throw new ApiException(ApiErrorType.Default, "请求不到任何任务，请确认任务已经分发。");
            }
            if (config == null)
            {
                throw new ApiException(ApiErrorType.Default, "取不到任务配置项，请确认已经设置该服务器的下载配置。");
            }

            // 数据库实例和领域实例转换
            HCSTask task;
            List<HCSJob> jobs;
            HCSJob item;
            List<HCSTask> resultList = new List<HCSTask>();
            HCSTaskConfig configString = JsonConvert.DeserializeObject<HCSTaskConfig>(config.Value);
            foreach (HCSDownloadTask current in taskList)
            {
                task = new HCSTask();
                task.TaskNo = current.TaskNo;

                jobs = new List<HCSJob>();
                foreach (HCSDownLoadJob currentJob in current.HCSDownLoadJobs)
                {
                    // 筛选未完成的Job
                    if (!currentJob.Status.Equals(DownloadStatus.Success.ConvertToString()))
                    {
                        item = new HCSJob();
                        jobs.Add(currentJob.CopyTo<HCSJob>(item));
                    }
                }
                task.Jobs = jobs;
                task.Config = configString;

                resultList.Add(task);
            }

            return resultList;
        }

        [UnitOfWork]
        void IHCSTaskManager.UpdateTaskStatus(string serverId, string sign, string bizType, string bizNo, string status, string errorMessage)
        {
            switch (bizType)
            {
                case "biz_task":

                    HCSTaskCriteria taskCriteria = new HCSTaskCriteria();
                    taskCriteria.ServerId = serverId;
                    taskCriteria.TaskNo = bizNo;

                    HCSDownloadTask task = Search(taskCriteria).FirstOrDefault();
                    task.ResultStatus = status.ParseAsEnum<DownloadStatus>().ToString();
                    task.ErrorMessage = errorMessage;
                    task.UpdateTime = DateTime.Now;
                    task.LastUpdateUser = serverId;

                    // 任务成功后更新资源的状态
                    if (status.ParseAsEnum<DownloadStatus>() == DownloadStatus.Success)
                    {
                        foreach (HCSDownLoadJob current in task.HCSDownLoadJobs)
                        {
                            var hotelId = _deviceTraceLibraryManager.Search(new DeviceTraceCriteria { DeviceSeries = serverId }).FirstOrDefault().HotelId;
                            var movieTrace = _hotelMovieTraceNoTemplateRepertory.Search(
                                                    new HotelMovieTraceNoTemplateCriteria() { MovieId = current.Name.Split('.')[0], HotelId = hotelId }).FirstOrDefault();

                            movieTrace.DownloadStatus = DownloadStatus.Success.ToString();
                            _hotelMovieTraceNoTemplateRepertory.Update(movieTrace);
                        }
                    }
                    _hcsTaskRepertory.Update(task);

                    break;

                case "biz_job":

                    HCSTaskCriteria criteria = new HCSTaskCriteria();
                    criteria.ServerId = serverId;
                    criteria.JobId = bizNo;

                    HCSDownloadTask jobTask = Search(criteria).FirstOrDefault();

                    //HCSDownloadTask jobTask = _hcsTaskRepertory.Search(criteria).FirstOrDefault();
                    HCSDownLoadJob job = jobTask.HCSDownLoadJobs.Where(p => p.Id == bizNo).FirstOrDefault();
                    job.Status = status.ParseAsEnum<DownloadStatus>().ToString();
                    job.ErrorMessage = errorMessage;
                    job.UpdateTime = DateTime.Now;
                    job.LastUpdateUser = serverId;

                    //_logManager.SaveInfo("JobUpdateBefore", JsonConvert.SerializeObject(job.CreateTime), AppType.HCS, job.Id);

                    // 任务成功后更新资源的状态
                    if (status.ParseAsEnum<DownloadStatus>() == DownloadStatus.Success)
                    {
                        var hotelId = _deviceTraceLibraryManager.Search(new DeviceTraceCriteria { DeviceSeries = serverId, DeviceType = DeviceType.HCSServer }).FirstOrDefault().HotelId;
                        var movieTrace = _hotelMovieTraceNoTemplateRepertory.Search(
                                                new HotelMovieTraceNoTemplateCriteria() { MovieId = job.Name.Split('.')[0], HotelId = hotelId }).FirstOrDefault();

                        movieTrace.DownloadStatus = DownloadStatus.Success.ToString();
                        _hotelMovieTraceNoTemplateRepertory.Update(movieTrace);
                    }
                    _hcsJobRepertory.Update(job);
                    break;
            }
        }

        [UnitOfWork]
        void IHCSTaskManager.RestMovieTask(List<CoreSysHotel> hotels, List<DeviceTrace> deviceTraces, MovieForLocalize movie, HCSJobOperationType operation)
        {
            if (hotels != null && deviceTraces != null && movie != null)
            {
                var taskNo = _hcsTaskRepertory.GetRecordCount();
                var configs = _hcsConfigRepertory.GetAll().Where(p => p.Type == "Task").ToList();
                object obj = new object();

                ConcurrentBag<HCSDownloadTask> adds = new ConcurrentBag<HCSDownloadTask>();

                hotels.AsParallel().ForAll(hotel =>
                {
                    var servers = deviceTraces.Where(m => m.HotelId.Equals(hotel.Id));

                    foreach (var server in servers)
                    {
                        lock (obj)
                        {
                            Interlocked.Increment(ref taskNo);
                            var task = AddMovieTask(server.DeviceSeries, new List<MovieForLocalize>() { movie }, taskNo, configs, operation);
                            if (task != null)
                                adds.Add(task);
                        }
                    }
                });

                if (adds.Count > 0)
                {
                    _hcsTaskRepertory.Insert(adds);
                    //RemoveCache();
                }
            }
        }

        private HCSDownloadTask AddMovieTask(string serverId, List<MovieForLocalize> movies, int taskNo, List<HCSConfig> configs, HCSJobOperationType operation)
        {
            HCSTaskCriteria criteria = new HCSTaskCriteria();
            criteria.ServerId = serverId;
            var config = configs.FirstOrDefault(p => p.ServerId.Equals(serverId));

            if (config == null) return null;

            var taskId = Guid.NewGuid().ToString("N");

            List<HCSDownLoadJob> jobs = new List<HCSDownLoadJob>();

            movies.ForEach(movie =>
            {
                jobs.Add(new HCSDownLoadJob()
                {
                    BizNo = movie.Id,
                    CreateTime = DateTime.Now,
                    ErrorMessage = "",
                    Id = Guid.NewGuid().ToString("N"),
                    LastUpdateUser = "System",
                    MD5 = movie.MD5 ?? "",
                    Name = movie.Id + ".mp4",
                    Priority = "N",
                    Status = "",
                    TaskId = taskId,
                    Type = HCSJobType.VOD.ToString(),
                    UpdateTime = DateTime.Now,
                    Url = movie.VodUrl ?? "",
                    Operation = operation.ToString()
                });
            });


            return (new HCSDownloadTask()
            {
                Config = config.Value,
                CreateTime = DateTime.Now,
                ErrorMessage = "",
                Id = taskId,
                LastUpdateUser = "System",
                ResultStatus = "",
                ServerId = serverId,
                Status = HcsTaskStatus.Normal.ToString(),
                TaskNo = taskNo.ToString().PadLeft(6, '0'),
                UpdateTime = DateTime.Now,
                HCSDownLoadJobs = jobs,
                Type = "VOD"
            });
        }

        [UnitOfWork]
        void IHCSTaskManager.AddMovieTaskByDevice(List<MovieForLocalize> movies, DeviceTrace deviceTraces)
        {
            if (movies != null && deviceTraces != null && movies.Count > 0)
            {

                var taskNo = _hcsTaskRepertory.GetRecordCount() + 1;
                var configs = _hcsConfigRepertory.GetAll().Where(p => p.Type == "Task").ToList();

                var adds = new ConcurrentBag<HCSDownloadTask>();

                var task = AddMovieTask(deviceTraces.DeviceSeries, movies, taskNo, configs, HCSJobOperationType.Shelve);
                if (task != null)
                    adds.Add(task);

                if (adds.Count > 0)
                {
                    _hcsTaskRepertory.Insert(adds);
                    //RemoveCache();
                }
            }
        }

    }
}
