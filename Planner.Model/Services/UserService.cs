using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public class UserService : IUserService
    {
        public User User { get; private set; }

        public string Token { get; private set; }

        public Task LoginAsync()
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
