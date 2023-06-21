using MelissandreDepartment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<(bool success, string role, string token, string message)> Login(string email, string password)
        {
            String requestUrl = $"{HttpClientManager.ApiUrl}/login?email={email}&password={password}";

            try
            {
                HttpResponseMessage response = await HttpClientManager.HttpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    String role = response.Headers.GetValues("Role").First();
                    String token = response.Headers.GetValues("Token").First();
                    return (true, role, token, null);
                }
                else
                {
                    String message = response.Headers.GetValues("message").FirstOrDefault("There was an error in the request. \nPlease retry later");
                    return (false, null, null, message);
                }
            }
            catch (HttpRequestException exception)
            {
                String message = $"There was an error:\n{exception.Message}\nPlease, send this to your administrator.";
                return (false, null, null, message);
            }
        }
    }
}
