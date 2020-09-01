using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ToDoList.Model
{
    [Table("Users")]
    public class User
    {
        [Key()]
        public Guid Id { get; set; }
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
