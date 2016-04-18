﻿using System.Collections.Generic;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;

    public interface IVODOrderRepertory : IBsaeRepertory<VODOrder>
    {
        string GetNewOrderId();
    }
}
