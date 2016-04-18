namespace YeahTVApi.ServiceProvider
{
    using YeahTVApi.Entity.AppTools;
    using YeahTVApi.Infrastructure;
    using YeahTVApi.ServiceProvider.PMSRoomStatusService;

    public class AppToolService : IAppToolService
    {
        /// <summary>
        /// pms登录返回
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public EmployeeEntity Auth(string userName, string password, string hotelId)
        {
            EmployeeEntity entity = new EmployeeEntity();
            RoomStatusModifySoap pmsClient = new RoomStatusModifySoapClient();
            IsLoginRequest request = new IsLoginRequest();
            IsLoginRequestBody body = new IsLoginRequestBody();
            body.userName = userName;
            body.passWord = password;
            body.hotelId = hotelId;
            request.Body = body;
            IsLoginResponseBody responseBody = pmsClient.IsLogin(request).Body;
            entity.IsLoginResult = responseBody.IsLoginResult;
            entity.employeeName = responseBody.employeeName;
            entity.hotelName = responseBody.hotelName;
            entity.userPermissions = responseBody.userPermissions;
            return entity;
        }
    }
}
