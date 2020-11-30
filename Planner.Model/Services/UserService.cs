using Nancy.Json;
using Planner.Model.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public class UserService : IUserService
    {
        private string BaseURL = "https://localhost:5001/api/";
        public User User { get; private set; }
        public string Token { get; private set; }

        public async Task LoginAsync(string username, string password)
        {
            var authModel = new AuthenticateModel(username, password);
            var authJson = new JavaScriptSerializer().Serialize(authModel);

            var data = new StringContent(authJson, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.PostAsync(BaseURL + "Users", data);

                string result = response.Content.ReadAsStringAsync().Result;
                
                //TODO: fix reading result
                Token = result;

                Console.WriteLine(result);
            }
        }

        public async Task RegisterAsync(string username, string password, string email, string firstName, string lastName)
        {
            var registerModel = new RegisterModel(username, password, email, firstName, lastName);
            var registerJson = new JavaScriptSerializer().Serialize(registerModel);

            var data = new StringContent(registerJson, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                //TODO: Add token
                var response = await client.PostAsync(BaseURL + "Users", data);

                string result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            var userJson = new JavaScriptSerializer().Serialize(user);

            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using(var client = new HttpClient(clientHandler))
            {
                //TODO: Add token
                var response = await client.PutAsync(BaseURL + "Users/" + user.Id.ToString(), data);

                string result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);
            }
        }
    }
}
