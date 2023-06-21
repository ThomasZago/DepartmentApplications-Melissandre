using System;
using System.Net.Http;

namespace MelissandreDepartment.DAO
{
    public class HttpClientManager
    {
        public static HttpClient HttpClient { get; private set; } = new HttpClient();
        public static string ApiUrl { get; private set; } = "https://temporaryID/api/service";
    }
}
