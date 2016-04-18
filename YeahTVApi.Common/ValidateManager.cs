using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace YeahTVApi.Common
{
    public class ValidateManager
    {
        public static bool IsValidEmail(string strIn)
        {
            
            // Return true if strIn is in valid e-mail format.

            return System.Text.RegularExpressions.Regex.IsMatch(

               strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)" +

               @"|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        public static bool ValidateMobile(string mobile)
        {
            long temp;
            if (mobile.StartsWith("13") || mobile.StartsWith("15") || mobile.StartsWith("18") || mobile.StartsWith("147"))
            {
                if (mobile.Length == 11 && long.TryParse(mobile, out temp))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        public static bool ValidateIdent(string ident)
        {
            switch (ident.Length)
            {
                case 15:
                    return CheckIdentCard15(ident);
                case 18:
                    return CheckIdentCard18(ident);
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证会员推荐验证码
        /// </summary>
        public static string MGM(string code)
        {
            string result = "";
            for (int i = 0; i < code.Length; i++)
            {
                result += MGMCode(code.Substring(i, 1));
            }
            return result;
        }

        #region 身份证号码验证

        private static bool CheckIdentCard18(string ident)
        {
            long n = 0;
            if (long.TryParse(ident.Remove(17), out n) == false || n < Math.Pow(10, 16) ||
                long.TryParse(ident.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false; //数字验证

            string address =
                "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(ident.Remove(2)) == -1)
                return false; //省份验证

            string birth = ident.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false; //生日验证

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = ident.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != ident.Substring(17, 1).ToLower())
            {
                return false; //校验码验证
            }
            return true; //符合GB11643-1999标准

        }

        private static bool CheckIdentCard15(string ident)
        {
            long n = 0;
            if (long.TryParse(ident, out n) == false || n < Math.Pow(10, 14))
            {
                return false; //数字验证
            }
            string address =
                "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(ident.Remove(2)) == -1)
            {
                return false; //省份验证
            }
            string birth = ident.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false; //生日验证
            }
            return true; //符合15位身份证标准
        }

        #endregion

        #region  会员推荐验证码

        private static string MGMCode(string index)
        {
            switch (index)
            {
                case "0":
                    return "r";
                case "1":
                    return "a";
                case "2":
                    return "e";
                case "3":
                    return "h";
                case "4":
                    return "t";
                case "5":
                    return "p";
                case "6":
                    return "s";
                case "7":
                    return "y";
                case "8":
                    return "g";
                case "9":
                    return "k";
                case "r":
                    return "0";
                case "a":
                    return "1";
                case "e":
                    return "2";
                case "h":
                    return "3";
                case "t":
                    return "4";
                case "p":
                    return "5";
                case "s":
                    return "6";
                case "y":
                    return "7";
                case "g":
                    return "8";
                case "k":
                    return "9";
                default:
                    return "";
            }
        }

        #endregion

    }

}
