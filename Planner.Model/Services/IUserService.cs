using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Services
{
    public interface IUserService
    {
        User User { get; }
        string Token { get; }

        Task<Guid> LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password, string email, string firstName, string lastName);
        Task UpdateUserAsync(User user);
        Task<User> GetUserAsync(Guid id);
        Task<Guid> ReLoginAsync();
    }
}
