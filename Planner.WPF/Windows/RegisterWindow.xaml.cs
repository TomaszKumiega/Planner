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
            var viewmodel = DataContext as IUserViewModel;
            await viewmodel.RegisterAsync(this.TextBoxUsername.Text, this.TextBoxPassword.Password, this.TextBoxPasswordRepeat.Password,
                this.TextBoxEmail.Text, this.TextBoxFirstName.Text, this.TextBoxLastName.Text);
            this.Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
