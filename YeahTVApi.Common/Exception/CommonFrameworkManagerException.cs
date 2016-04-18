using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Common
{
    public class CommonFrameworkManagerException : ApplicationException
    {
        private string message;

        public CommonFrameworkManagerException(string message, Exception innerException)
            : base()
        {
            this.message = message;
        }

        public override string Message
        {
            get
            {
                return string.Format("CommonFrameworkManager_{0}", base.Message);
            }
        }
    }
}
