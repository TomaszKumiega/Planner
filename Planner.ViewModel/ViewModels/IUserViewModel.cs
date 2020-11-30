using Planner.Model;
using System.Threading.Tasks;

namespace Planner.ViewModel.ViewModels
{
    public interface IUserViewModel
    {
        UserModel User { get; }

        Task LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password, string repeatedPassword, string email, string firstName, string lastName);
    }
}