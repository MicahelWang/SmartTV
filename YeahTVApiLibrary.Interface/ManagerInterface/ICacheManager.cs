using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface ICacheManager
    {
        void SetWeather();

        void SetAppsList();
    }
}
