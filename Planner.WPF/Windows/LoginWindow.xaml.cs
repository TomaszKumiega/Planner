using Planner.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Planner.WPF.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public delegate void LoginFinishedHandler(object sender, EventArgs e);
        public event LoginFinishedHandler LoginFinished;

        public LoginWindow(IUserViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private async void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: validate input
            var viewmodel = DataContext as IUserViewModel;
            await viewmodel.LoginAsync(this.TextBoxUsername.Text, this.TextBoxPassword.Password);
            LoginFinished(this, new EventArgs());
            this.Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HyperlinkRegister_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegisterWindow(DataContext as IUserViewModel);
            this.Visibility = Visibility.Hidden;
            registrationWindow.Show();
            registrationWindow.ButtonBack.Click += (s, e) => { this.Visibility = Visibility.Visible; };
            registrationWindow.Closed += (s, e) => { this.Visibility = Visibility.Visible; };
        }

        private void RectangleTopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.TextBoxPassword.Focus();
            }
        }

        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) this.ButtonLogIn_Click(this, e);
        }

        private void ButtonLogIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) this.ButtonLogIn_Click(this, e);
        }

        private void ButtonExit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) this.ButtonExit_Click(this, e);
        }

        private void TextBlockRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) this.HyperlinkRegister_Click(this, e);
        }
    }
}
