using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Model
{
    [Table("Users")]
    public class User : INotifyPropertyChanged
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        [Key()]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
            _logger.Debug(name + " property changed");
        }
        #endregion

        #region Constructors
        public User()
        {

        }

        /// <summary>
        /// Initializes new instance of <see cref="User"/> class
        /// </summary>
        /// <param name="name"></param>
        public User(string username, string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Karma = 0;

            _logger.Debug("User: " + Id.ToString() + " created.");
        }
        #endregion

    }
}