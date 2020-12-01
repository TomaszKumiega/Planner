using System;
using System.Collections.Generic;
using System.Text;

namespace Planner.Model.Model
{
    public class UpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Karma { get; set; }

        public UpdateModel()
        {

        }

        public UpdateModel(string firstName, string lastName, string username, string password, string email, int karma)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Email = email;
            Karma = karma;
        }
    }
}
