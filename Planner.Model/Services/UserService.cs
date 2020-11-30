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

                Token = result;

                Console.WriteLine(result);
            }
        }

        public async Task RegisterAsync(string username, string password, string email, string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
