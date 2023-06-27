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
using System.Net;

namespace MelissandreDepartment.DAO
{
    public class HttpClientUserDAO
    {
        private HttpClientUserDAO userDAO;
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

        public async Task<(bool success, string message)> SendNewPassword(string email, string type)
        {
            string requestUrl = $"{HttpClientManager.ApiUrl}/auth/modify/password/";

            try
            {
                Dictionary<string, string> requestData = new Dictionary<string, string>
                    {
                        { "email", email },
                        { "type", type }
                    };

                string jsonContent = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                content.Headers.ContentType.CharSet = "UTF-8";
                HttpResponseMessage response = await HttpClientManager.HttpClient.PutAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string message = $"New password sent successfully to {email} of type {type}.";
                    return (true, message); // Password modification successful
                }
                else
                {
                    string message = $"Failed to send new password to  {email} of type {type} : {response.StatusCode} : {response.ReasonPhrase}";
                    return (false, message);
                }
            }
            catch (HttpRequestException exception)
            {
                string message = $"There was an error:\n{exception.Message}\nPlease, send this to your administrator.";
                return (false, message);
            }
        }

        public async Task<(bool success, string message)> RegisterAccount(string email, string type, string generatedPassword)
        {
            Dictionary<string, string> requestData = new Dictionary<string, string>();
            requestData["fullname"] = String.Empty;
            requestData["email"] = email;
            requestData["password"] = generatedPassword;
            requestData["type"] = type;
            requestData["address"] = String.Empty;
            requestData["sponsorship"] = String.Empty;
            requestData["longitude"] = String.Empty;
            requestData["latitude"] = String.Empty;

            string requestUrl = $"{HttpClientManager.ApiUrl}/auth/add/register";
            try
            {
                string jsonContent = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                content.Headers.ContentType.CharSet = "UTF-8";
                HttpResponseMessage response = await HttpClientManager.HttpClient.PostAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    String message = $"You successfully created account {email} of type {type}";
                    (bool success, string message2) = await SendNewPassword(email, type);
                    if (success)
                    {
                        return (true, message);
                    }
                    else
                    {
                        return(false, message + "\n" + message2 );
                    }
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
