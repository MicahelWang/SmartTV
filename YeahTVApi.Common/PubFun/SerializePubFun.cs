namespace YeahTVApi.Common
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Serialization;
    using YeahTVApi.Entity;

    public static partial class PubFun
    {
        #region Bytes <==> Base32
        private const string BASE32 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ456789";

        public static string BytesToBase32(this byte[] value)
        {
            //计算字符串的长度，每个 byte 为8位二进制数，1到5位二进制数需要1个字符，6到10位为2个，依此类推
            int length = (value.Length * 8 + 4) / 5;
            StringBuilder sb = new StringBuilder(length);
            int cur_i = 0;
            //当前 byte[] 索引
            byte cur_b = 0;
            //当前 byte
            int index;
            //5位二进制数结果
            int k;
            //当前二进制位索引
            for (int i = 0; i < length; i++)
            {
                //对每个 Base32 字符循环
                index = 0;
                //初始置 0
                for (int j = 0; j < 5; j++)
                {
                    //对每个 Base32 字符代表的二进制位循环
                    k = i * 5 + j;
                    //计算当前二进制位索引
                    if (k == value.Length * 8)
                        break;
                    //二进制位扫描结束
                    if (k % 8 == 0)
                        cur_b = value[cur_i++];
                    //转移到下一个 byte，初始化时转移到第 0 个 byte
                    index <<= 1;
                    //左移一位以便继续设置最低位
                    index |= (cur_b & 128) == 0 ? 0 : 1;
                    //将 byte 的最高位送入 index 的最低位
                    cur_b <<= 1;
                    //将 byte 左移一位以将次高位变为最高位
                }
                sb.Append(BASE32[index]);
                //追加字符
            }
            return sb.ToString();
        }

        public static byte[] Base32ToBytes(this string value)
        {
            //解码
            value = value.ToUpper();
            //转换为小写
            int length = value.Length * 5 / 8;
            //计算长度，因为末位可能会多表示 0 到 4 个二进制位，因此无需修正余数。
            byte[] r = new byte[length];
            //分配空间
            int cur_i = 0;
            //当前 Base32 字符索引
            int cur_v = 0;
            //当前 Base32 字符代表的数字
            int k;
            //当前二进制位索引
            for (int i = 0; i < length; i++)
            {
                //对每个 byte 循环
                for (int j = 0; j < 8; j++)
                {
                    //对每个 byte 对应的二进制位循环
                    k = i * 8 + j;
                    //计算当前二进制位索引
                    if (k == value.Length * 5)
                        break;
                    //二进制位扫描结束，通常 Base32 字符代表的二进制位会大于等于实际长度
                    //因此此处主要用于检测不规范 Base32 编码
                    if (k % 5 == 0)
                    {
                        cur_v = BASE32.IndexOf(value[cur_i++]);
                        //转移到下一个 Base32 字符，初始化时转移到第 0 个字符
                        if (cur_i == value.Length && value.Length % 8 != 0)
                            cur_v <<= value.Length * 5 % 8;
                        //根据 Base32 字符串代表的长度和实际 byte[] 长度
                        //修正最末尾 Base32 字符代表的数字。
                    }
                    r[i] <<= 1;
                    r[i] |= (byte)((cur_v & 16) == 0 ? 0 : 1);
                    cur_v <<= 1;
                    //编码过程的逆过程，同样是移位、送位
                }
            }
            return r;
        }
        #endregion

        #region Bytes <==> HexString
        
        public static byte[] ConvertHexStringToByte(this string hexString)
        {
            if (String.IsNullOrEmpty(hexString))
                return null;
            byte[] data = new byte[hexString.Length / 2];//HexStringToBytes(content, null);
            int i = 0;
            while (hexString.Length > 0)
            {
                String d = null;
                if (hexString.Length >= 2)
                    d = hexString.Substring(0, 2);
                else
                    d = hexString;
                data[i++] = Convert.ToByte(d, 16);
                if (hexString.Length >= 2)
                    hexString = hexString.Substring(2);
                else
                    hexString = "";
            }
            return data;
        }

        public static char HexDigit(this int num)
        {
            return ((num < 10) ? ((char)(num + 0x30)) : ((char)(num + 0x37)));
        }

        public static int ConvertHexDigit(this char val)
        {
            if ((val <= '9') && (val >= '0')) return (val - '0');
            if ((val >= 'a') && (val <= 'f')) return ((val - 'a') + 10);
            if ((val >= 'A') && (val <= 'F')) return ((val - 'A') + 10);

            throw new Exception("HexDigit Error");
        }

        public static byte[] HexStringToBytes(this string hexString)
        {
            byte[] buffer;
            if (hexString == null) throw new ArgumentNullException("hexString");

            bool flag = false;
            int pos = 0;
            int len = hexString.Length;
            if (((len >= 2) && (hexString[0] == '0')) && ((hexString[1] == 'x') || (hexString[1] == 'X')))
            {
                len = hexString.Length - 2;
                pos = 2;
            }

            if (((len % 2) != 0) && ((len % 3) != 2))
            {
                throw new ArgumentException("Argument_InvalidHexFormat");
            }

            if ((len >= 3) && (hexString[pos + 2] == ' '))
            {
                flag = true;
                buffer = new byte[(len / 3) + 1];
            }
            else
            {
                buffer = new byte[len / 2];
            }

            for (int i = 0; pos < hexString.Length; i++)
            {
                int h = ConvertHexDigit(hexString[pos]);
                int l = ConvertHexDigit(hexString[pos + 1]);
                buffer[i] = (byte)(l | (h << 4));
                if (flag) pos++;
                pos += 2;
            }
            return buffer;
        }

        public static string BytesToHexString(this byte[] buf, int idx = 0, int len = -1)
        {
            string str = null;
            if (buf == null) return str;
            if (idx < 0 || idx >= buf.Length) throw new Exception("EncodeHexString idx Error");

            len = idx + ((len < 0) ? buf.Length : len);
            if (len <= idx || len > buf.Length) throw new Exception("EncodeHexString len Error");

            char[] chArray = new char[buf.Length * 2];
            int num3 = 0;
            while (idx < len)
            {
                int num = (buf[idx] & 240) >> 4;
                chArray[num3++] = HexDigit(num);
                num = buf[idx] & 15;
                chArray[num3++] = HexDigit(num);
                idx++;
            }
            return new string(chArray);
        }
        #endregion

        #region object <==> byte[]
        /// <summary>
        /// 将 obj 转成 byte[]（二进制序列化）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjToBuffer(this object obj)
        {
            return ObjToBuffer(obj, false, (byte[])null);
        }

        /// <summary>
        /// 将 obj 转成 byte[]（二进制序列化）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isCompress"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ObjBuffer(object obj, bool isCompress, string key)
        {
            return ObjToBuffer(obj, false, Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// 将 obj 转成 byte[]（二进制序列化）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isCompress"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] ObjToBuffer(this object obj, bool isCompress, byte[] pass)
        {
            byte[] rstBuf = null;
            if (obj == null) return rstBuf;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fmt
                   = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            MemoryStream ms = new MemoryStream();
            Stream rs = ms;

            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {
                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateEncryptor(), CryptoStreamMode.Write);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Compress, true);
                    rs = zs;
                }

                fmt.Serialize(rs, obj);

                DisposeObject(ref zs);
                DisposeObject(ref cs);
                rstBuf = ms.ToArray();
                DisposeObject(ref ms);
            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
                DisposeObject(ref ms);
            }

            return rstBuf;
        }

        /// <summary>
        /// 将 buye[] 转成对应的 obj （二进制序列化），不压缩，不加密
        /// </summary>
        /// <param name="bufSource"></param>
        /// <returns></returns>
        public static T BufferToObj<T>(this byte[] bufSource) where T : class
        {
            return BufferToObj<T>(bufSource, false, (byte[])null);
        }

        /// <summary>
        /// 将 buye[] 转成对应的 obj （二进制序列化）
        /// </summary>
        /// <param name="bufSource"></param>
        /// <param name="isCompress"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T BufferToObj<T>(this byte[] bufSource, bool isCompress, string key) where T : class
        {
            return BufferToObj<T>(bufSource, isCompress, Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// 将 byte[] 转成 obj （二进制序列化）
        /// </summary>
        /// <param name="bufSource"></param>
        /// <param name="isCompress"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static T BufferToObj<T>(this byte[] bufSource, bool isCompress, byte[] pass) where T : class
        {
            T objRst = null;
            if (bufSource == null) return objRst;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fmt
                   = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();


            MemoryStream ms = new MemoryStream(bufSource);
            Stream rs = ms;

            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {

                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateDecryptor(), CryptoStreamMode.Read);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Decompress, true);
                    rs = zs;
                }

                objRst = fmt.Deserialize(rs) as T;
            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
                DisposeObject(ref ms);
            }
            return objRst;
        }

        #endregion

        #region object <==> xml
        public static string ObjToXmlString(this object obj)
        {
            return Encoding.UTF8.GetString(ObjToXmlBuffer(obj, false));
        }
        public static T XmlStringToObj<T>(this string xml) where T : class
        {
            T objRst = null;
            if (string.IsNullOrEmpty(xml)) return objRst;

            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xml))
            {
                objRst = xs.Deserialize(sr) as T;
                sr.Close();
            }
            return objRst;
        }

        public static byte[] ObjToXmlBuffer(this object obj, bool isCompress = true, string key = null)
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return ObjToXmlBuffer(obj, isCompress, bufKey);
        }
        public static byte[] ObjToXmlBuffer(this object obj, bool isCompress, byte[] pass)
        {
            if (obj == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                Obj2XmlStream(ms, obj, isCompress, pass);
                return ms.ToArray();
            }
        }

        public static void Obj2XmlFile(this string fileName, object obj, bool isCompress = true, string key = null)
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            Obj2XmlFile(fileName, obj, isCompress, bufKey);
        }
        public static void Obj2XmlFile(this string fileName, object obj, bool isCompress, byte[] pass)
        {
            using (FileStream fs = File.OpenWrite(fileName))
            {
                Obj2XmlStream(fs, obj, isCompress, pass);
                fs.Close();
            }
        }

        public static void Obj2XmlStream(this Stream stream, object obj, bool isCompress = true, string key = null)
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            Obj2XmlStream(stream, obj, isCompress, bufKey);
        }
        public static void Obj2XmlStream(this Stream stream, object obj, bool isCompress, byte[] pass)
        {
            if (obj == null) return;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            XmlSerializer xs = new XmlSerializer(obj.GetType());

            Stream rs = stream;

            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {
                //先压缩，后加密 (加密流 是 压缩流 的基础流)
                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateEncryptor(), CryptoStreamMode.Write);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Compress, true);
                    rs = zs;
                }


                xs.Serialize(rs, obj);

                DisposeObject(ref zs);
                DisposeObject(ref cs);
            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
            }
        }

        public static T XmlFileToObj<T>(this string fileName, bool isCompress = true, string key = null) where T : class
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return XmlFileToObj<T>(fileName, isCompress, bufKey);
        }
        public static T XmlFileToObj<T>(this string fileName, bool isCompress, byte[] pass) where T : class
        {
            T rst = null;
            using (FileStream fs = File.OpenRead(fileName))
            {
                rst = XmlStreamToObj<T>(fs, isCompress, pass);
                fs.Close();
            }
            return rst;
        }

        public static T XmlStreamToObj<T>(this Stream stream, bool isCompress = true, string key = null) where T : class
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return XmlStreamToObj<T>(stream, isCompress, bufKey);
        }
        public static T XmlStreamToObj<T>(this Stream stream, bool isCompress, byte[] pass) where T : class
        {

            T objRst = null;
            if (stream == null) return objRst;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            XmlSerializer xs = new XmlSerializer(typeof(T));


            //MemoryStream ms = new MemoryStream(buf);
            Stream rs = stream;

            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {

                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateDecryptor(), CryptoStreamMode.Read);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Decompress, true);
                    rs = zs;
                }

                objRst = xs.Deserialize(rs) as T;
            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
                //DisposeObject(ref ms);
            }
            return objRst;
        }

        public static T XmlBufferToObj<T>(this byte[] buf, bool isCompress = true, string key = null) where T : class
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return XmlBufferToObj<T>(buf, isCompress, bufKey);
        }
        public static T XmlBufferToObj<T>(this byte[] buf, bool isCompress, byte[] pass) where T : class
        {
            T objRst = null;
            if (buf == null) return objRst;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            using (MemoryStream ms = new MemoryStream(buf))
            {
                objRst = XmlStreamToObj<T>(ms, isCompress, pass);
            }
            return objRst;
        }
        #endregion

        #region object <=> json
        public static string ToJsonString(this object obj)
        {
            try
            { 
                return JsonConvert.SerializeObject(obj);
            }
            catch(Exception ex)
            {
                throw new ApiException("ToJsonString" + obj.ToString() + ex.Message);
            }

        }

        public static T JsonStringToObj<T>(this string json) where T : class
        {
            return JsonStringToObj<T>(json, null);
        }

        public static T JsonStringToObj<T>(this string json, JsonSerializerSettings settings) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return null;
                if (settings == null)
                    return JsonConvert.DeserializeObject<T>(json);
                else
                    return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch (Exception ex)
            {
                throw new ApiException("ToJsonString" + json + ex.Message);
            }
        }

        public static object JsonStringToObj(this string json, Type type)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return null;

                return JsonConvert.DeserializeObject(json, type);
            }
            catch (Exception ex)
            {
                throw new ApiException("ToJsonString" + json + ex.Message);
            }
        }

        public static object JsonStringToObj(this string json, Type type, JsonSerializerSettings settings)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return null;

                return JsonConvert.DeserializeObject(json, type, settings);
            }
            catch (Exception ex)
            {
                throw new ApiException("ToJsonString" + json + ex.Message);
            }
        }

        public static byte[] Obj2JsonBuffer(object obj, bool isCompress = true, string key = null)
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return Obj2JsonBuffer(obj, isCompress, bufKey);
        }
        public static byte[] Obj2JsonBuffer(object obj, bool isCompress, byte[] pass)
        {
            if (obj == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                if (obj is String)
                    String2Stream(ms, (String)obj, isCompress, pass);
                else
                    Obj2JsonStream(ms, obj, isCompress, pass);
                return ms.ToArray();
            }
        }

        public static void Obj2JsonFile(string fileName, object obj, bool isCompress = true, string key = null)
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            Obj2JsonFile(fileName, obj, isCompress, bufKey);
        }
        public static void Obj2JsonFile(string fileName, object obj, bool isCompress, byte[] pass)
        {
            using (FileStream fs = File.OpenWrite(fileName))
            {
                Obj2JsonStream(fs, obj, isCompress, pass);
                fs.Close();
            }
        }

        public static void Obj2JsonStream(Stream stream, object obj, bool isCompress = true, string key = null)
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            Obj2JsonStream(stream, obj, isCompress, bufKey);
        }

        public static void String2Stream(Stream stream, String content, bool isCompress, byte[] pass)
        {
            if (content == null) return;
            bool isCrypto = ((pass != null) && (pass.Length > 0));


            Stream rs = stream;
            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {
                //先压缩，后加密 (加密流 是 压缩流 的基础流)
                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateEncryptor(), CryptoStreamMode.Write);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Compress, true);
                    rs = zs;
                }

                //将对象写到 rs stream 中
                using (var jsw = new StreamWriter(rs))
                {
                    jsw.Write(content);
                }

            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
            }
        }


        public static void Obj2JsonStream(Stream stream, object obj, bool isCompress, byte[] pass)
        {
            if (obj == null) return;
            bool isCrypto = ((pass != null) && (pass.Length > 0));
            //JsonTextWriter
            JsonSerializer js = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            Stream rs = stream;
            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {
                //先压缩，后加密 (加密流 是 压缩流 的基础流)
                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateEncryptor(), CryptoStreamMode.Write);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Compress, true);
                    rs = zs;
                }

                //将对象写到 rs stream 中
                using (var jsw = new StreamWriter(rs))
                using (var jtw = new JsonTextWriter(jsw))
                {
                    js.Serialize(jtw, obj);
                }

            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
            }
        }

        public static T JsonBufferToObj<T>(byte[] buf, bool isCompress = true, string key = null) where T : class
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return JsonBufferToObj<T>(buf, isCompress, bufKey);
        }
        public static T JsonBufferToObj<T>(byte[] buf, bool isCompress, byte[] pass) where T : class
        {
            T objRst = null;
            if (buf == null) return objRst;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            using (MemoryStream ms = new MemoryStream(buf))
            {
                objRst = JsonStreamToObj<T>(ms, isCompress, pass);
            }
            return objRst;
        }

        public static T JsonFileToObj<T>(string fileName, bool isCompress = true, string key = null) where T : class
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return JsonFileToObj<T>(fileName, isCompress, bufKey);
        }
        public static T JsonFileToObj<T>(string fileName, bool isCompress, byte[] pass) where T : class
        {
            T rst = null;
            using (FileStream fs = File.OpenRead(fileName))
            {
                rst = JsonStreamToObj<T>(fs, isCompress, pass);
                fs.Close();
            }
            return rst;
        }

        public static T JsonStreamToObj<T>(Stream stream, bool isCompress = true, string key = null) where T : class
        {
            byte[] bufKey = null;
            if (key != null) bufKey = Encoding.UTF8.GetBytes(key);
            return JsonStreamToObj<T>(stream, isCompress, bufKey);
        }
        public static T JsonStreamToObj<T>(Stream stream, bool isCompress, byte[] pass) where T : class
        {

            T objRst = null;
            if (stream == null) return objRst;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            JsonSerializer js = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            //MemoryStream ms = new MemoryStream(buf);
            Stream rs = stream;

            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {
                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateDecryptor(), CryptoStreamMode.Read);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Decompress, true);
                    rs = zs;
                }

                using (var jsw = new StreamReader(rs))
                using (var jtw = new JsonTextReader(jsw))
                {
                    objRst = js.Deserialize<T>(jtw);
                }
            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
                //DisposeObject(ref ms);
            }
            return objRst;
        }

        public static string JsonStreamToJsonString(Stream stream, bool isCompress, byte[] pass)
        {

            string objRst = null;
            if (stream == null) return objRst;
            bool isCrypto = ((pass != null) && (pass.Length > 0));

            JsonSerializer js = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            Stream rs = stream;

            CryptoStream cs = null;
            GZipStream zs = null;

            try
            {
                if (isCrypto)
                {
                    ARC4Managed am = new ARC4Managed();
                    am.Key = pass;
                    cs = new CryptoStream(rs, am.CreateDecryptor(), CryptoStreamMode.Read);
                    rs = cs;
                }

                if (isCompress)
                {
                    zs = new GZipStream(rs, CompressionMode.Decompress, true);
                    rs = zs;
                }

                using (var jsw = new StreamReader(rs))
                {
                    objRst = jsw.ReadToEnd();
                }
            }
            finally
            {
                DisposeObject(ref zs);
                DisposeObject(ref cs);
            }
            return objRst;
        }
        public static Dictionary<String, String> JsonStringToDictionary(string json)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<String, String>>(json);
        }
        #endregion

        //struct 转换为 byte[]
        public static byte[] StructToBuffer(object objStruct)
        {
            int size = Marshal.SizeOf(objStruct);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(objStruct, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        //byte[] 转换为 struct
        public static T BufferToStruct<T>(byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return (T)Marshal.PtrToStructure(buffer, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
