using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    }
}
