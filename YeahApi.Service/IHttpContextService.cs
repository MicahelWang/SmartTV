using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YeahResourceApi.Service
{
    public interface IHttpContextService
    {
        HttpContextBase Current { get; }
    }
}
