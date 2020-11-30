using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public interface IUserService
    {
        UserModel User { get; }
        string Token { get; }

        Task LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password, string email, string firstName, string lastName);
        Task UpdateUserAsync();
        Task<UserModel> GetUserAsync(Guid id);
        Task ReLoginAsync();
    }
}
