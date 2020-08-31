using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Model
{
    public class User
    {
        public string Name { get; private set; }
        public int Karma { get; set; }

        /// <summary>
        /// Initializes new instance of <see cref="User"/> class
        /// </summary>
        /// <param name="name"></param>
        public User(string name)
        {
            Name = name;
        }
    }
}
