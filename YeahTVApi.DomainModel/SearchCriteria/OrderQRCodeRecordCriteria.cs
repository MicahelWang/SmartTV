namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class OrderQRCodeRecordCriteria : BaseSearchCriteria
    {
        public string OrderType { get; set; }
        public string OrderId { get; set; }
        public string Ticket { get; set; }
    }
}
