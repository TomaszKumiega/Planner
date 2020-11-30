﻿using Planner.Model;
using Planner.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Planner.ViewModel.ViewModels
{
    public class UserViewModel : IUserViewModel
    {
        private IUserService _userService;
        public User User { get; private set; }

        public UserViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task LoginAsync(string username, string password)
        {
            var id = await _userService.LoginAsync(username, password);
            User = await _userService.GetUserAsync(id);
        }

        public async Task<bool> RegisterAsync(string username, string password, string repeatedPassword, string email, string firstName, string lastName)
        {
            if (password != repeatedPassword) return false;
            else
            {
                await _userService.RegisterAsync(username, password, email, firstName, lastName);
                return true;
            }
        }

    }
}
