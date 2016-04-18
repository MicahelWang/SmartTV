using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum DeliveryType
    {
        [Description("送至客房")]
        SentRoom = 0,
        [Description("前台自取")]
        ReceptionDesk = 1
    }
}