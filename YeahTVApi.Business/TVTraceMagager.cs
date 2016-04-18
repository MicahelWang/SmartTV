namespace HZTVApi.Manager
{
    using HZTVApi.Common;
    using HZTVApi.DomainModel.Models;
    using HZTVApi.DomainModel.SearchCriteria;
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using HZTVApi.DomainModel.Mapping;

    public class TVTraceMagager : ITVTraceManager
    {
        private ITVTraceRepertory traceRepertory;
        private IRedisCacheManager redisCacheManager;

        public TVTraceMagager(ITVTraceRepertory traceRepertory, 
            IRedisCacheManager redisCacheManager)
        {
            this.traceRepertory = traceRepertory;
            this.redisCacheManager = redisCacheManager;
        }

        public string GetHotelID(RequestHeader header)
        {
            if (string.IsNullOrEmpty(header.hotelID))
                GetTrace(header);

            return header.hotelID;
        }

        public string GetRoomNo(RequestHeader header)
        {
            if (string.IsNullOrEmpty(header.roomNo))
                GetTrace(header);

            return header.roomNo;
        }
     
        public List<string> GetTraceHotelIds()
        {
            return traceRepertory.GetTraceHotelIds();
        }

        public string GetHotelRoomKey(RequestHeader header)
        {
            return SearchTrace(header).FirstOrDefault().TVKey;
        }

        public List<DeviceTraceForSP> LogDeviceTrace(RequestHeader header, out int status, out string tvKey)
        {
            var trace = header.ToTVTrace();
            tvKey = trace.TVKey;
            return traceRepertory.LogDeviceTrace(trace, out status);
        }

        public void RefreshDeviceHotel(RequestHeader header)
        {
            GetTrace(header);
        }

        public List<TVTrace> Search(TVTraceModelCriteria criteria)
        {
            return traceRepertory.Search(criteria);
        }

        public void AddTrace(TVTrace trace)
        {
            traceRepertory.Insert(trace);
        }

        public void UpdateTrace(TVTrace trace)
        {
            traceRepertory.Update(trace);
        }

        #region pravite method

        private void GetTrace(RequestHeader header)
        {
            var trace = SearchTrace(header);

            if (trace != null && trace.Any())
            {
                header.hotelID = trace.FirstOrDefault().HotelId;
                header.roomNo = trace.FirstOrDefault().RoomNo;
            }

            redisCacheManager.SetCache(header.DEVNO, header.hotelID + ";" + header.roomNo);
        }

        private List<TVTrace> SearchTrace(RequestHeader header)
        {
            var criteria = new TVTraceModelCriteria();

            criteria.AppVersion = header.Ver;
            criteria.Platfrom = header.Platform;
            criteria.DeviceSeries = header.DEVNO;

            var trace = traceRepertory.Search(criteria);
            return trace;
        }

        #endregion
    }
}
