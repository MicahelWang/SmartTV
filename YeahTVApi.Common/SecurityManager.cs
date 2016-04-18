using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace YeahTVApi.Common
{
    public class SecurityManager
    {
       

        /// <summary>
        /// 强加密随机数
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] GetRNGBytes(int size)
        {
            byte[] arr = new byte[size];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(arr);
            return arr;
        }

        private static readonly string pwdCharArray =
           "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ<>`~!@#$^*()-_=+[]{}\\|;:'\",./";


        /// <summary>
        /// 生成固定长度的密码,复杂程度（数字+小写+大写+符号）
        /// </summary>
        /// <param name="size">长度</param>
        /// <returns></returns>
        public static string GetRNGString(int size)
        {
            return GetRNGString(size, -1);
        }

        /// <summary>
        /// 生成固定长度的密码
        /// </summary>
        /// <param name="size">长度</param>
        /// <param name="level">0-纯数字, 1-数字+小写字母, 2-数字+小写+大写字母, 3-数字+小写+大写+符号</param>
        /// <returns></returns>
        public static string GetRNGString(int size, int level)
        {
            if (level == 0)
            {
                //纯数字
                return GetRNGString(size, pwdCharArray, 0, 10);
            }
            else if (level == 1)
            {
                //数字+小写
                return GetRNGString(size, pwdCharArray, 0, 36);
            }
            else if (level == 2)
            {
                //数字+小写+大写
                return GetRNGString(size, pwdCharArray, 0, 62);
            }
            else
            {
                //数字+小写+大写+符号
                return GetRNGString(size, pwdCharArray);
            }
        }

        /// <summary>
        /// 生成固定长度的密码
        /// </summary>
        /// <param name="size">长度</param>
        /// <param name="key">密码由key包含的字符组成</param>
        /// <returns></returns>
        public static string GetRNGString(int size, string key)
        {
            return GetRNGString(size, key, 0, key.Length);
        }

        /// <summary>
        /// 生成固定长度的密码
        /// </summary>
        /// <param name="size">长度</param>
        /// <param name="key">密码由key包含的字符区间段组成</param>
        /// <param name="start">区间段开始位置</param>
        /// <param name="len">区间段长度</param>
        /// <returns></returns>
        public static string GetRNGString(int size, string key, int start, int len)
        {
            if ((string.IsNullOrEmpty(key)) || (start + len > key.Length))
            {
                throw new Exception("参数错误，key 缓冲区溢出！");
            }

            char[] rst = new char[size];
            byte[] buf = GetRNGBytes(size * 4);
            for (int i = 0; i < rst.Length; i++)
            {
                int p = Math.Abs(BitConverter.ToInt32(buf, i * 4));
                if (p >= len) p = p % len;

                rst[i] = key[start + p];
            }

            return new string(rst);
        }

        #region 哈希算法相关
        public enum HashType { Default, MD5, SHA1 }
        //Hash 
        public static byte[] GetHash(string pm_Str)
        {
            return GetHash(HashType.Default, pm_Str);
        }
        public static byte[] GetHash(HashType pm_Type, string pm_Str)
        {
            return GetHash(pm_Type, Encoding.UTF8.GetBytes(pm_Str));
        }

        public static byte[] GetHash(HashType pm_Type, byte[] pm_Buf)
        {
            return GetHashAlgorithm(pm_Type).ComputeHash(pm_Buf);
        }

        public static byte[] GetHash(Stream pm_Stream)
        {
            return GetHash(HashType.Default, pm_Stream);
        }
        public static byte[] GetHash(HashType pm_Type, Stream pm_Stream)
        {
            return GetHashAlgorithm(pm_Type).ComputeHash(pm_Stream);
        }

        private static HashAlgorithm GetHashAlgorithm(HashType pm_Type)
        {
            HashAlgorithm lc_Hash = null;
            switch (pm_Type)
            {
                case HashType.Default:
                    lc_Hash = new MD5CryptoServiceProvider();
                    break;
                case HashType.MD5:
                    lc_Hash = new MD5CryptoServiceProvider();
                    break;
                case HashType.SHA1:
                    lc_Hash = new SHA1CryptoServiceProvider();
                    break;
                default:
                    throw new Exception("没有定义的 Hash 算法");
            }
            return lc_Hash;
        }

        #endregion

        #region 加密，解密相关
 

      

        public static void RC4TransformSelf(byte[] buf, byte[] key)
        {
            ARC4Managed.TransformSelf(buf, key);
        }
        #endregion
    }
}
