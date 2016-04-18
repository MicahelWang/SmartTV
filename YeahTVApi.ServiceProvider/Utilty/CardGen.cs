namespace YeahTVApi.ServiceProvider
{
    using System;

    public class CardGen
    {
        private const int CF_CUSTOMER = 0x39;	//客人卡
        private DateTime m_TimeBase = new DateTime(2005, 1, 1, 0, 0, 0);
        private const string m_KEYA_KEXIN = "199404281970";		//房卡扇区的密码A"

        /// <summary>
        /// 科新门锁读写KEY
        /// </summary>
        public string KeyA_KeXin
        {
            get { return m_KEYA_KEXIN; }
        }

        /// <summary>
        /// 准备科新门锁的房卡数据
        /// </summary>
        /// <param name="systemPw">标识酒店的系统密码（6位数字字符串）</param>
        /// <param name="building">楼</param>
        /// <param name="floor">层</param>
        /// <param name="room">房间</param>
        /// <param name="p">批次号（1~99）</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间（年份是9999时表示永久有效）</param>
        /// <param name="waterno">流水号（1~999999）</param>
        /// <returns>byte[32]，写入第0扇区的块1和块2中</returns>
        public byte[] GetKexinData(string systemPw,
            int building, 
            int floor, 
            int room, 
            int batchNo, 
            DateTime start, 
            DateTime end, 
            int waterNo)
        {
            int min1, min2;
            byte[] buf = new byte[32];

            buf[0] = CF_CUSTOMER;
            buf[1] = Convert.ToByte(systemPw.Substring(0, 2), 16);
            buf[2] = Convert.ToByte(systemPw.Substring(2, 2), 16);
            buf[3] = Convert.ToByte(systemPw.Substring(4, 2), 16);

            buf[4] = this.DecToHex(building);
            buf[5] = this.DecToHex(floor);
            buf[6] = this.DecToHex(room);

            TimeSpan ts1 = start - this.m_TimeBase;
            min1 = (int)ts1.TotalMinutes;

            if (end.Year == 9998)
                min2 = 999998;
            else if (end.Year == 9999) //不限时间
                min2 = 999999;
            else
            {
                TimeSpan ts2 = end - start;
                min2 = (int)ts2.TotalMinutes;
            }

            //startTime, endTime
            string s1 = min1.ToString("00000000");
            string s2 = min2.ToString("000000");
            buf[13] = Convert.ToByte(s1.Substring(0, 2), 16);
            buf[14] = Convert.ToByte(s1.Substring(2, 2), 16);
            buf[15] = Convert.ToByte(s1.Substring(4, 2), 16);
            buf[16] = Convert.ToByte(s1.Substring(6, 2), 16);
            buf[17] = Convert.ToByte(s2.Substring(0, 2), 16);
            buf[18] = Convert.ToByte(s2.Substring(2, 2), 16);
            buf[19] = Convert.ToByte(s2.Substring(4, 2), 16);

            //批次号
            buf[20] = this.DecToHex(batchNo);

            //流水号
            string s3 = waterNo.ToString("000000");
            buf[22] = Convert.ToByte(s3.Substring(0, 2), 16);
            buf[23] = Convert.ToByte(s3.Substring(2, 2), 16);
            buf[24] = Convert.ToByte(s3.Substring(4, 2), 16);

            //校验码
            int checksum = this.GetCheckSum(buf);
            buf[25] = this.DecToHex(checksum);
            return buf;
        }

        private byte DecToHex(int x)
        {
            return (byte)(x / 10 * 16 + x % 10);
        }

        // 科新门锁的校验和, buf 应该是一个byte[32]
        private int GetCheckSum(byte[] buf)
        {
	        int i, sum = 0;
	        //前 25 个字节进行计算
	        for(i = 0; i < 25; i++)
		        sum += ((buf[i] >> 4) + (buf[i] & 0x0F));
	        return (int)(sum / 4.6);
        }
    }
    
}
