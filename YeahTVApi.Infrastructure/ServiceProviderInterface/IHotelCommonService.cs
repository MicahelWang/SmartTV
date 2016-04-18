namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity.CentralMapping;

    public interface IHotelCommonService : ICentralGetwayServiceBase
    {
        string GetWeather(string cityName);

    }
}
