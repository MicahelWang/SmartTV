using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YeahTVApi.Common
{
    public class XMLPubFun
    {
        /// <summary>
        /// 获取xml文档
        /// </summary>
        /// <param name="xmlFilePath">XML路径</param>
        /// <returns></returns>
        public XmlDocument GetXmlDoucument(string xmlFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
            return doc;
        }
    }
}
