using AndroidXml;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using YeahTVApi.DomainModel.Models.DataModel;

namespace YeahAppCentre.Web.Utility
{
    public static class AndroidXmlParser
    {
        private static readonly string path = System.AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "").Replace("bin\\Release", "");

        public static List<AndroidInfo> GetAndroidInfo(this string fileName)
        {
            return GetInfos(fileName);
        }

        private static List<AndroidInfo> GetInfos(string fileName)
        {
            var androidInfos = new List<AndroidInfo>();
            try
            {
                //要分析的文件名称
                var manifest = "AndroidManifest.xml";

                //读取apk,通过解压的方式读取
                using (var zip = ZipFile.Read(path + fileName))
                {
                    using (Stream zipstream = zip[manifest].OpenReader())
                    {
                        //将解压出来的文件保存到一个路径（必须这样）
                        using (var fileStream = File.Create(path + manifest, (int)zipstream.Length))
                        {
                            // Initialize the bytes array with the stream length and then fill it with data
                            byte[] bytesInStream = new byte[zipstream.Length];
                            zipstream.Read(bytesInStream, 0, bytesInStream.Length);
                            // Use write method to write to the file specified above
                            fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                        }
                    }
                }

                //读取解压文件的字节数
                byte[] data = File.ReadAllBytes(path + manifest);
                if (data.Length == 0)
                {
                    throw new IOException("Empty file");
                }

                #region 读取文件内容
                using (var stream = new MemoryStream(data))
                {
                    var reader = new AndroidXmlReader(stream);

                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                {
                                    var info = new AndroidInfo();
                                    androidInfos.Add(info);
                                    info.Name = reader.Name;
                                    info.Settings = new List<AndroidSetting>();
                                    for (int i = 0; i < reader.AttributeCount; i++)
                                    {
                                        reader.MoveToAttribute(i);

                                        AndroidSetting setting = new AndroidSetting() { Name = reader.Name, Value = reader.Value };
                                        info.Settings.Add(setting);
                                    }
                                    reader.MoveToElement();
                                    break;
                                }
                        }
                    }
                }
                #endregion
            }
            catch
            {
                throw new IOException("AndroidXmlParser Error!");
            }
            finally
            {
                DeleteFile(path + fileName);
            }
            return androidInfos;
        }

        private static void DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
