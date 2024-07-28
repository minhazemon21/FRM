using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace ERP.Web.Helpers
{
    public class API
    {
        public static string PostData(string url, string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var result = client.PostAsync("http://" + url, content).Result;
            var jsonResult = result.Content.ReadAsStringAsync().Result;
            return jsonResult;
        }
        public static string PostFile(string url, HttpContent content)
        {
            var client = new HttpClient();
            var result = client.PostAsync("http://" + url, content).Result;
            var jsonResult = result.Content.ReadAsStringAsync().Result;
            return jsonResult;
        }

        public static string GetData(string url)
        {
            var client = new HttpClient();
            var result = client.GetAsync("http://" + url).Result;
            var jsonResult = result.Content.ReadAsStringAsync().Result;
            return jsonResult;
        }
    }
}