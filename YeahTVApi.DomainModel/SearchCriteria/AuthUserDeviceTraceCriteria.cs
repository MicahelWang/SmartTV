﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
   public class AuthUserDeviceTraceCriteria:BaseSearchCriteria
    {
       public string UserId { get; set; }

       public string DeviceNo { get; set; }
    }
}
