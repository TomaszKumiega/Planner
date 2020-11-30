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

        Task LoginAsync();
        Task RegisterAsync();
        Task UpdateUserAsync();
    }
}
