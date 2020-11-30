using Microsoft.Graph;
using Planner.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planner.ViewModel.ViewModels
{
    public class UserViewModel
    {
        private IUserService _userService;

        public UserViewModel(IUserService userService)
        {
            _userService = userService;
        }
    }
}
