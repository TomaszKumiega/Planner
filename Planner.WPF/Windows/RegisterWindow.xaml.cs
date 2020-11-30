using Planner.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow(IUserViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private async void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;
         
            var viewmodel = DataContext as IUserViewModel;
            await viewmodel.RegisterAsync(this.TextBoxUsername.Text, this.TextBoxPassword.Password, this.TextBoxPasswordRepeat.Password,
                this.TextBoxEmail.Text, this.TextBoxFirstName.Text, this.TextBoxLastName.Text);
            this.Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private bool ValidateInput()
        {
            SetDefaultForegrounds();

            Regex regex = new Regex(@"^[A-Z0-9_-]+$", RegexOptions.IgnoreCase);

            // Username validation
            if (String.IsNullOrWhiteSpace(TextBoxUsername.Text) || !regex.IsMatch(TextBoxUsername.Text))
            {
                TextBlockUsername.Foreground = Brushes.MediumOrchid;
                return false;
            }

            // Password validation
            if (String.IsNullOrWhiteSpace(TextBoxPassword.Password) || !regex.IsMatch(TextBoxPassword.Password))
            {
                TextBlockPassword.Foreground = Brushes.MediumOrchid;
                return false;
            }

            // Repeated password validation
            if (TextBoxPasswordRepeat.Password != TextBoxPassword.Password)
            {
                TextBlockPasswordRepeat.Foreground = Brushes.MediumOrchid;
                return false;
            }

            // Email validation
            try
            {
                MailAddress m = new MailAddress(TextBoxEmail.Text);
            }
            catch(FormatException)
            {
                TextBlockEmail.Foreground = Brushes.MediumOrchid;
                return false;
            }

            regex = new Regex(@"/ ^[a - z,.'-]+$/i", RegexOptions.IgnoreCase);

            // First name validation
            if (String.IsNullOrWhiteSpace(TextBoxFirstName.Text) || !regex.IsMatch(TextBoxFirstName.Text))
            {
                TextBlockFirstName.Foreground = Brushes.MediumOrchid;
                return false;
            }

            // Last name validation
            if (String.IsNullOrWhiteSpace(TextBoxLastName.Text) || !regex.IsMatch(TextBoxLastName.Text))
            {
                TextBoxLastName.Foreground = Brushes.MediumOrchid;
                return false;
            }

            return true;
        }

        private void SetDefaultForegrounds()
        {
            TextBlockUsername.Foreground = Brushes.WhiteSmoke;
            TextBlockPassword.Foreground = Brushes.WhiteSmoke;
            TextBlockPasswordRepeat.Foreground = Brushes.WhiteSmoke;
            TextBlockEmail.Foreground = Brushes.WhiteSmoke;
            TextBlockFirstName.Foreground = Brushes.WhiteSmoke;
            TextBlockLastName.Foreground = Brushes.WhiteSmoke;
        }
    }
}
