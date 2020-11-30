using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public class UserService : IUserService
    {
        private string BaseURL = "https://localhost:5001/api/";
        public User User { get; private set; }

        public string Token { get; private set; }

        public Task LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(string username, string password, string email, string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
