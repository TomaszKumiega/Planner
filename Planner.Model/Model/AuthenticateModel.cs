using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Planner.Model.Model
{
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public AuthenticateModel()
        {

        }

        public AuthenticateModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
