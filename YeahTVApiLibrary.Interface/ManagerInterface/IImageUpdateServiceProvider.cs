﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IImageUpdateServiceProvider
    {
        string UpdateImageByBitmapStream(Stream imageBitmapStream,string fileType);
        string UpdateAppByStream(Stream imageBitmapStream, string fileType);
    }
}
