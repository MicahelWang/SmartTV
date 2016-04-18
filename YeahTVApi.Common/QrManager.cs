using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace YeahTVApi.Common
{
    public class QrManager
    {
        EncodingOptions options = null;
        BarcodeWriter writer = null;

        public QrManager()
        {
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 300,
                Height = 300,
                Margin=1
                
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
        }

        /// <summary>
        /// 获得二维码
        /// </summary>
        /// <returns></returns>
        public byte[] GetImageDataQr(Object obj)
        {
            byte[] bitmapBytes = null;
            if (null != obj)
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        var bitmap = writer.Write(JsonConvert.SerializeObject(obj));
                        bitmap.Save(stream, ImageFormat.Jpeg);
                        byte[] data = new byte[stream.Length];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(data, 0, Convert.ToInt32(stream.Length));
                        bitmapBytes = data;
                    }

                }
                catch (Exception)
                {
                    bitmapBytes = null;
                }
            }
            return bitmapBytes;
        }

        /// <summary>
        /// 获得二维码
        /// </summary>
        /// <returns></returns>
        public byte[] GetImageDataQr_URL(string url)
        {
            byte[] bitmapBytes = null;
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        var bitmap = writer.Write(url);
                        bitmap.Save(stream, ImageFormat.Jpeg);
                        byte[] data = new byte[stream.Length];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(data, 0, Convert.ToInt32(stream.Length));
                        bitmapBytes = data;
                    }

                }
                catch (Exception)
                {
                    bitmapBytes = null;
                }
            }
            return bitmapBytes;
        } 

        private byte[] BitmapToByte(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
            ms.Close();
            return bytes;
        }
    }
}
