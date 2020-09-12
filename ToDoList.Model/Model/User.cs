using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ToDoList.Model
{
    [Table("Users")]
    public class User : INotifyPropertyChanged
    {
        [Key()]
        public Guid Id { get; set; }
        public string Name { get; set; }
        private int _karma;
        public int Karma 
        { 
            get => _karma; 
            set
            {
                _karma = value;
                OnPropertyChanged("Karma");
            }
        }

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        /// <summary>
        /// Initializes new instance of <see cref="User"/> class
        /// </summary>
        /// <param name="name"></param>
        public User(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Karma = 0;
        }

    }
}
