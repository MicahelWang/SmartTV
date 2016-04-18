namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;

    public interface IMemberInfoService : ICentralGetwayServiceBase
    {
        Guest Query(BaseRequestData data);
    }
}
