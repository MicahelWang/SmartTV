using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class KeyValue
    {
        [DisplayName("键")]
        public string Key { get; set; }
        [DisplayName("值")]
        public object Value { get; set; }
    }
}
