namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity.AppTools;

    public interface IAppToolService
    {
        EmployeeEntity Auth(string userName, string password, string hotelId);
    }
}
