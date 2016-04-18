using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
    public interface IHCSTaskManager
    {
        [Cache]
        List<HCSTask> GetTask(string serverId, string sign, string oldTaskNo);

        void UpdateTaskStatus(string serverId, string sign, string bizType, string bizNo, string status, string errorMessage);

        [UnitOfWork]
        void RestMovieTask(List<CoreSysHotel> hotels, List<DeviceTrace> deviceTraces, MovieForLocalize movie,
            HCSJobOperationType operation);

        [UnitOfWork]

        void AddMovieTaskByDevice(List<MovieForLocalize> movies, DeviceTrace deviceTraces);
    }
}
