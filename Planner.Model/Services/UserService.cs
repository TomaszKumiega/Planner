using Nancy.Json;
using Newtonsoft.Json.Linq;
using Planner.Model.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public class UserService : IUserService
    {
        private string BaseURL = "https://localhost:5001/";
        public UserModel User { get; private set; }
        public string Token { get; private set; }

        public async Task<UserModel> GetUserAsync(Guid id)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", Token);
                var response = await client.GetAsync(BaseURL + "Users/" + id.ToString());

                string result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);

                return new JavaScriptSerializer().Deserialize<UserModel>(result);
            }
        }

        public async Task LoginAsync(string username, string password)
        {
            var authModel = new AuthenticateModel(username, password);
            var authJson = new JavaScriptSerializer().Serialize(authModel);

            var data = new StringContent(authJson, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.PostAsync(BaseURL + "Users/authenticate", data);

                string result = response.Content.ReadAsStringAsync().Result;

                var authResult = new JavaScriptSerializer().Deserialize<AuthenticationResult>(result);
                
                Console.WriteLine(result);
                
                Token = authResult.Token;
                User = new UserModel(authResult.Id, authResult.Username, authResult.FirstName, authResult.LastName, authResult.Email, authResult.Karma);
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
                var response = await client.PostAsync(BaseURL + "Users/register", data);

                string result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);
            }
        }

        public async Task UpdateUserAsync()
        {
            var userJson = new JavaScriptSerializer().Serialize(User);

            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using(var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", Token);
                var response = await client.PutAsync(BaseURL + "Users/" + User.Id.ToString(), data);

                string result = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(result);
            }
        }

        public async Task ReLoginAsync()
        {
            await LoginAsync(User.Username, User.Password);
        }
    }
}
