using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    public  class TV_APPS
    {
        public String APP_ID { get; set; }
        public String PLATFORM { get; set; }
        
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }
        public String APP_KEY { get; set; }
        public String SECURE_KEY { get; set; }
        public Boolean ACTIVE { get; set; }
    }


}
