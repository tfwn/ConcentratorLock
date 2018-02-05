using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Common
{
     public sealed class HttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private static readonly HttpClientHelper _instance = new HttpClientHelper();
        private PostFunction serialHelper = new PostFunction();
        private int _TimeOut;
        public int TimeOut {
            get {
                return _TimeOut;

            }
            set {
                _TimeOut = value;
                _httpClient.Timeout = TimeSpan.FromMilliseconds(_TimeOut * 1000);
            }
        }
        static HttpClientHelper()
        {
         
        }
       private HttpClientHelper()
       {
            _TimeOut = 30;
            _httpClient = new HttpClient() {  };
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromMilliseconds(_TimeOut * 1000);

        }

        public static HttpClientHelper Instance
        {
            get { return _instance; }
        }


        public async Task<string> PostAsync(string url, string json)
        {
               var dictParam = serialHelper.DeserializeFromString<Dictionary<string, string>>(json);
            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(dictParam));
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string url, string json,int timeout)
        {
            var dictParam = serialHelper.DeserializeFromString<Dictionary<string, string>>(json);
            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(dictParam));

            return await response.Content.ReadAsStringAsync();
        }
    }
}
