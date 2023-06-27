using MelissandreDepartment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;

namespace MelissandreDepartment.DAO
{
    public class HttpClientUserDAO
    {
        private static HttpClientUserDAO _instance;
        private static readonly object _lockObject = new object();

        public static HttpClientUserDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new HttpClientUserDAO();
                        }
                    }
                }
                return _instance;
            }
        }

        private HttpClientUserDAO()
        {

        }

        public async Task<(bool success, string message)> Login(string email, string password, string type)
        {
            Dictionary<string, string> requestData = new Dictionary<string, string>();
            requestData["email"] = email;
            requestData["password"] = password;
            requestData["type"] = type;
            String requestUrl = $"{HttpClientManager.ApiUrl}/auth/add/login";

            try
            {
                string jsonContent = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                content.Headers.ContentType.CharSet = "UTF-8";
                HttpResponseMessage response = await HttpClientManager.HttpClient.PostAsync(requestUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    String message = response.StatusCode + " : " + response.ReasonPhrase;
                    return (false, message);
                }
            }
            catch (HttpRequestException exception)
            {
                String message = $"There was an error:\n{exception.Message}\nPlease, send this to your administrator.";
                return (false, message);
            }
        }
    }
}
