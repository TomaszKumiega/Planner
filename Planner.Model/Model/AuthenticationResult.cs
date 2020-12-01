using System;
using System.Collections.Generic;
using System.Text;

namespace Planner.Model.Model
{
    public class AuthenticationResult
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int Karma { get; set; }
    }
}
