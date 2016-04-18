using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Common
{
    public static partial class PubFun
    {
        public static String StringToMd5(this String content)
        {
            var md5Hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hasher.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(content));
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static String StringToMd5(this byte[] byteArray)
        {
            var md5Hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hasher.ComputeHash(byteArray);
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static void DisposeObject<T>(ref T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string AESEncrypt(string plainText, string strKey)
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText); //得到需要加密的字节数组      
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray(); //得到加密后的字节数组
            cs.Close();

            ms.Close();
            ms.Dispose();
            ms = null;
            string cipherText = Convert.ToBase64String(cipherBytes);
            return cipherText;
        }

        public static string MD5Encrypt(this string sEncrypt, string sKey)
        {
            MD5 md5Hash = new MD5CryptoServiceProvider();
            byte[] byteData = md5Hash.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(sEncrypt + sKey));

            var sBuilder = new StringBuilder();
            for (int i = 0; i < byteData.Length; i++)
            {
                sBuilder.Append(byteData[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }
    }
}
