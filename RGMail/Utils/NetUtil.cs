using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RGMail.Model;

namespace RGMail.Utils
{
    public static class NetUtil
    {
        private static string ImageURLJson => "https://api.ooopn.com/image/lofter/api.php?type=json";
        private static string YanURL => "https://v1.hitokoto.cn/?encode=text";
        private static HttpClient httpClient = new HttpClient();
        public static async Task<string> GetImageURL()
        {
            string json = await httpClient.GetStringAsync(ImageURLJson);
            JObject jobj = JObject.Parse(json);
            string url = jobj.Value<string>("imgurl");
            return url;
        }
        public static async Task<string> GetYan()
        {
            string text = await httpClient.GetStringAsync(YanURL);
            return text;
        }
        public static async Task<Stream> GetIPST()
        {
            string ipst = "https://api.ooopn.com/ipst/api.php";
            Stream stream = await httpClient.GetStreamAsync(ipst);
            return stream;
        }
        public static async Task<List<string>> GetHistoryDay()
        {
            string temp = RandomUtil.GenerateRandomLetter(10);
            string url = $"https://api.ooopn.com/history/api.php?{temp}";
            string result = await httpClient.GetStringAsync(url);
            try
            {
                JObject json = JObject.Parse(result);
                JArray content = json.Value<JArray>("content");
                List<string> lst = JsonConvert.DeserializeObject<List<string>>(content.ToString());
                return lst;
            }
            catch(Exception ex)
            {
                RGCommon.Log(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// QQ名称和头像获取
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static async Task<QQModel> GetQQImage(string qq)
        {
            string url = $"https://api.ooopn.com/qqinfo/api.php?qq={qq}";
            string result = await httpClient.GetStringAsync(url);
            QQModel model = JsonConvert.DeserializeObject<QQModel>(result);
            return model;
        }
    }
}
