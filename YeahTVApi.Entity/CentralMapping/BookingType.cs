namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 预定类型
    /// </summary>
    public enum BookingType
    {
        /// <summary>
        /// 没有设定
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// 市场活动
        /// </summary>
        Activity = 1, 
        /// <summary>
        /// 积分兑换免房
        /// </summary>
        PointExchange = 2,
        /// <summary>
        /// 促销码
        /// </summary>
        Promotion = 3,
        /// <summary>
        /// 积分加速
        /// </summary>
        ExtraPoint = 4
    }
}
