﻿using MelissandreDepartment.Model;
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

        public async Task<(bool success, string message, List<Account> clients)> GetClients(List<string> requestedTypes)
        {
            List<Account> clients = new List<Account>();
            bool successState = true;
            string message = string.Empty;

            try
            {
                foreach (var type in requestedTypes)
                {
                    string requestUrl = $"{HttpClientManager.ApiUrl}/auth/get/{type}";
                    HttpResponseMessage response = await HttpClientManager.HttpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Check if the JSON content is empty or null
                        if (string.IsNullOrEmpty(jsonContent))
                        {
                            message += $"Empty JSON response for type: {type}\n";
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(jsonContent);

                            // Check if the JSON array is empty
                            if (jsonArray.Count == 0)
                            {
                                message += $"No account found for type: {type}\n";
                            }
                            else
                            {
                                // Extract the necessary information from the JSON array
                                foreach (JObject jsonObject in jsonArray)
                                {
                                    string typeString = jsonObject.Value<string>("type");

                                    if (Enum.TryParse(typeString, out ClientAccountType parsedClientType))
                                    {
                                        if (parsedClientType.ToString() == type)
                                        {
                                            ClientAccount client = new ClientAccount
                                            {
                                                FullName = jsonObject.Value<string>("fullname"),
                                                Email = jsonObject.Value<string>("email"),
                                                Role = parsedClientType,
                                                Status = AccountStatus.Active
                                                // Set other properties as needed
                                            };

                                            clients.Add(client);
                                        }
                                    }
                                    else if (Enum.TryParse(typeString, out DepartmentAccountType parsedDepartmentType))
                                    {
                                        if (parsedDepartmentType.ToString() == type)
                                        {
                                            DepartmentAccount client = new DepartmentAccount
                                            {
                                                FullName = jsonObject.Value<string>("fullname"),
                                                Email = jsonObject.Value<string>("email"),
                                                Role = parsedDepartmentType,
                                                Status = AccountStatus.Active
                                                // Set other properties as needed
                                            };

                                            clients.Add(client);
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"Invalid account type: {typeString}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        successState = false;
                        message += $"{response.StatusCode} : {response.ReasonPhrase}\n";
                    }
                }

                // Check if no accounts were found for any requested type
                if (clients.Count == 0 && !string.IsNullOrEmpty(message))
                {
                    message = $"No accounts found for the requested types: {string.Join(", ", requestedTypes)}";
                }
            }
            catch (Exception ex)
            {
                successState = false;
                message = ex.Message;
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

        public async Task<(bool success, string message)> RegisterAccount(string fullname, string email, string type, string generatedPassword)
        {
            Dictionary<string, string> requestData = new Dictionary<string, string>();
            requestData["fullname"] = fullname;
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

        public async Task<(bool success, string message)> DeleteAccount(string email, string type)
        {
            Dictionary<string, string> requestData = new Dictionary<string, string>();
            requestData["email"] = email;
            requestData["type"] = type;

            string requestUrl = $"{HttpClientManager.ApiUrl}/auth/delete/Users";
            try
            {

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
                string jsonContent = JsonConvert.SerializeObject(requestData);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send the request
                HttpResponseMessage response = await HttpClientManager.HttpClient.SendAsync(request);
                String message = "";

                if (response.IsSuccessStatusCode)
                {
                    message = $"You successfully deleted the account {email} of type {type}";
                }
                else
                {
                    message = response.StatusCode + " : " + response.ReasonPhrase;
                }

                return (response.IsSuccessStatusCode, message);
            }
            catch (HttpRequestException exception)
            {
                String message = $"There was an error:\n{exception.Message}\nPlease, send this to your administrator.";
                return (false, message);
            }
        }

        internal Task<(bool success, string message)> ActivateAccount(string email, string v1, bool v2)
        {
            throw new NotImplementedException();
        }

        internal Task<(bool success, string message)> DeactivateAccount(string email, string v1, bool v2)
        {
            throw new NotImplementedException();
        }
    }
}
