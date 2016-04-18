using YeahTVApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.Manager
{
    public class FileUp
    {
        private IHttpContextService httpContextService;
        public FileUp(IHttpContextService httpContextService)
        {
            this.httpContextService = httpContextService;
        }

        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>字节数组</returns>
        public byte[] GetBinaryFile(string filename)
        {
            if (File.Exists(filename))
            {
                FileStream Fsm = null;
                try
                {
                    Fsm = File.OpenRead(filename);
                    return this.ConvertStreamToByteBuffer(Fsm);
                }
                catch
                {
                    return new byte[0];
                }
                finally
                {
                    Fsm.Close();
                }
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// 流转化为字节数组
        /// </summary>
        /// <param name="theStream">流</param>
        /// <returns>字节数组</returns>
        public byte[] ConvertStreamToByteBuffer(System.IO.Stream theStream)
        {
            int bi;
            MemoryStream tempStream = new System.IO.MemoryStream();
            try
            {
                while ((bi = theStream.ReadByte()) != -1)
                {
                    tempStream.WriteByte(((byte)bi));
                }
                return tempStream.ToArray();
            }
            catch
            {
                return new byte[0];
            }
            finally
            {
                tempStream.Close();
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="PosPhotoUpload">控件</param>
        /// <param name="saveFileName">保存的文件名</param>
        /// <param name="imagePath">保存的文件路径</param>
        public string FileSc(FileUpload PosPhotoUpload, string saveFileName, string imagePath)
        {
            string state = "";
            if (PosPhotoUpload.HasFile)
            {
                if (PosPhotoUpload.PostedFile.ContentLength / 1024 < 10240)
                {
                    string MimeType = PosPhotoUpload.PostedFile.ContentType;
                    if (String.Equals(MimeType, "image/gif") || String.Equals(MimeType, "image/pjpeg"))
                    {
                        string extFileString = System.IO.Path.GetExtension(PosPhotoUpload.PostedFile.FileName);
                        PosPhotoUpload.PostedFile.SaveAs(httpContextService.Current.Server.MapPath(imagePath));
                    }
                    else
                    {
                        state = "上传文件类型不正确";
                    }
                }
                else
                {
                    state = "上传文件不能大于10M";
                }
            }
            else
            {
                state = "没有上传文件";
            }
            return state;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="binData">字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="savePath">保存目录</param>
        //-------------------调用----------------------
        //byte[] by = GetBinaryFile("E:\\Hello.txt");
        //this.SaveFile(by,"Hello",".txt");
        //---------------------------------------------
        public static bool SaveFile(byte[] binData, string fileName, string savePath)
        {
            var uploadResult = true;
            FileStream fileStream = null;
            var m = new MemoryStream(binData);
            try
            {
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                var file = savePath + fileName;
                fileStream = new FileStream(file, FileMode.Create);
                m.WriteTo(fileStream);
            }
            catch (Exception ex)
            {
                uploadResult = false;
            }
            finally
            {
                m.Close();
                if (fileStream != null) fileStream.Close();
            }
            return uploadResult;
        }
        /// <summary>
        /// 上传文件(生成安卓720手机图片，480手机图片和640手机图片)
        /// </summary>
        /// <param name="binData">字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="savePath">保存目录</param>
        //原图片名为a.jpg则720图片为a_720.jpg,480手机图片为a_480.jpg,640手机图片为a_640.jpg
        //-------------------调用----------------------
        //byte[] by = GetBinaryFile("E:\\Hello.txt");
        //this.SaveFile(by,"Hello",".txt");
        //---------------------------------------------
        public static bool SaveFileImageByNews(byte[] binData, string fileName, string savePath)
        {
            System.Drawing.Image imgPhoto = null;
            using (MemoryStream ms = new MemoryStream(binData))
            {
                imgPhoto = System.Drawing.Image.FromStream(ms);
            }
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            using (System.Drawing.Bitmap img_610 = new System.Drawing.Bitmap(imgPhoto, 610, 274))
            {
                string strResizePicName_610 = savePath + fileName.Replace(".jpg", "_610.jpg");
                img_610.Save(strResizePicName_610, ImageFormat.Jpeg);
            }
            using (System.Drawing.Bitmap img_458 = new System.Drawing.Bitmap(imgPhoto, 458, 216))
            {
                string strResizePicName_458 = savePath + fileName.Replace(".jpg", "_458.jpg");
                img_458.Save(strResizePicName_458, ImageFormat.Jpeg);
            }
            using (System.Drawing.Bitmap img_720 = new System.Drawing.Bitmap(imgPhoto, 686, 308))
            {
                string strResizePicName_720 = savePath + fileName.Replace(".jpg", "_720.jpg");
                img_720.Save(strResizePicName_720, ImageFormat.Jpeg);
            }


            return SaveFile(binData, fileName, savePath);
        }
        /// <summary>
        /// 上传文件(生成安卓720手机图片，480手机图片和640手机图片)
        /// </summary>
        /// <param name="binData">字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="savePath">保存目录</param>
        //原图片名为a.jpg则720图片为a_720.jpg,480手机图片为a_480.jpg,640手机图片为a_640.jpg
        //-------------------调用----------------------
        //byte[] by = GetBinaryFile("E:\\Hello.txt");
        //this.SaveFile(by,"Hello",".txt");
        //---------------------------------------------
        public static bool SaveFileImageByHomePage(byte[] binData, string fileName, string savePath)
        {
            System.Drawing.Image imgPhoto = null;
            using (MemoryStream ms = new MemoryStream(binData))
            {
                imgPhoto = System.Drawing.Image.FromStream(ms);
            }
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            using (System.Drawing.Bitmap img_436 = new System.Drawing.Bitmap(imgPhoto, 546, 714))
            {
                string strResizePicName_436 = savePath + fileName.Replace(".jpg", "_436.jpg");
                img_436.Save(strResizePicName_436, ImageFormat.Jpeg);
            }
            using (System.Drawing.Bitmap img_388 = new System.Drawing.Bitmap(imgPhoto, 486, 634))
            {
                string strResizePicName_388 = savePath + fileName.Replace(".jpg", "_388.jpg");
                img_388.Save(strResizePicName_388, ImageFormat.Jpeg);
            }


            return SaveFile(binData, fileName, savePath);
        }
        public static bool SaveFileImageByHeight(byte[] binData, string fileName, string savePath, int height)
        {
            System.Drawing.Image imgPhoto = null;
            using (MemoryStream ms = new MemoryStream(binData))
            {
                imgPhoto = System.Drawing.Image.FromStream(ms);
            }
            float imgWidth = imgPhoto.Width;
            float imgHeight = imgPhoto.Height;
            if (imgHeight > 500)
            {
                imgWidth = imgWidth * (500 / imgHeight);
                imgHeight = 500;

            }
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            using (System.Drawing.Bitmap img = new System.Drawing.Bitmap(imgPhoto, Convert.ToInt32(imgWidth), Convert.ToInt32(imgHeight)))
            {
                string strResizePicName = savePath + fileName;
                img.Save(strResizePicName, ImageFormat.Jpeg);
                return true;
            }

        }
        public static bool SaveDimensionalCode(System.Drawing.Bitmap image, string fileName, string savePath)
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            image.Save(savePath + fileName, ImageFormat.Jpeg);
            return true;
        }
    }
}
