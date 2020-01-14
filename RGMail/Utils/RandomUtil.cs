using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGMail.Utils
{
    /// <summary>
    /// 随机帮助
    /// </summary>
    public static class RandomUtil
    {
        private static Random rand = new Random();
        /// <summary>
        /// 随机汉字
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCodeNum(int codeCount)
        {
            int area, code;
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < codeCount; i++)
            {
                area = rand.Next(16, 88);
                if(area == 55)
                {
                    code = rand.Next(1, 90);
                }
                else
                {
                    code = rand.Next(1, 94);
                }
                sb.Append(Encoding.GetEncoding("GB2312").GetString(new byte[] { Convert.ToByte(area + 160), Convert.ToByte(code + 160) }));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 随机生成字母
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomLetter(int Length)
        {
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            StringBuilder sb = new StringBuilder();
            int n = Pattern.Length;
            for (int i = 0; i < Length; i++)
            {
                int rnd = rand.Next(0, n);
                sb.Append(Pattern[rnd]);
            }
            return sb.ToString();
        }
    }
}
