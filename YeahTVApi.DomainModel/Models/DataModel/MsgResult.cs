namespace YeahTVApi.DomainModel.Models
{
    public class MsgResult
    {
        public string Msg { get; set; }


        public bool HasError { get; set; }

        public string ErrorCode { get; set; }

        public object Data { get; set; }
    }
}