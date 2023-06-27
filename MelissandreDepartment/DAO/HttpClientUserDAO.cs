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
using MelissandreServiceLibrary.Enum;
using Newtonsoft.Json.Linq;

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

        public async Task<(bool success, string message, List<ClientAccount> clients)> GetClients()
        {
            List<ClientAccount> clients = new List<ClientAccount>();
            bool successState = true;
            string message = string.Empty;

            try
            {
                foreach (ClientAccountType type in Enum.GetValues(typeof(ClientAccountType)))
                {
                    string requestUrl = $"{HttpClientManager.ApiUrl}/auth/get/{type}";
                    HttpResponseMessage response = await HttpClientManager.HttpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(jsonContent);

                        // Extract the necessary information from the JSON array
                        foreach (JObject jsonObject in jsonArray)
                        {
                            string typeString = jsonObject.Value<string>("type");
                            if (Enum.TryParse(typeString, out ClientAccountType parsedType))
                            {
                                if (parsedType == type)
                                {
                                    ClientAccount client = new ClientAccount
                                    {
                                        Email = jsonObject.Value<string>("email"),
                                        Role = parsedType,
                                        Status = AccountStatus.Active
                                        // Set other properties as needed
                                    };
                                    clients.Add(client);
                                }
                            }
                            else
                            {
                                throw new Exception($"Invalid client account type: {typeString}");
                            }
                        }
                    }
                    else
                    {
                        successState = false;
                        message += $"{response.StatusCode} : {response.ReasonPhrase}\n";
                    }
                }
            }



            catch (HttpRequestException exception)
            {
                successState = false;
                message = $"There was an error:\n{exception.Message}\nPlease, send this to your administrator.";
            }

            return (successState, message, clients);
        }
    }
}
