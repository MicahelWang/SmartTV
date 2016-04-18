using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApiLibrary.Service;

namespace YeahTVApiLibrary.UnitTests
{
    [TestClass]
    public class RequestApiServiceTest
    {
        [TestMethod]
        public void GetRequestApi()
        {
            var services = new RequestApiService();
            var actual = services.Get("http://www.cnblogs.com/notniu/p/3898001.html");

            // Assert
            Assert.IsNotNull(actual);
        }
        
    }
}
