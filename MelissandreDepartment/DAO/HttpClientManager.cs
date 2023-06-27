using System;
using System.Net.Http;

namespace MelissandreDepartment.DAO
{
    public class HttpClientManager
    {
        public static HttpClient HttpClient { get; private set; } = new HttpClient();
        public static string ApiUrl { get; private set; } = "http://3.83.173.28:2000/api";
    }
}
