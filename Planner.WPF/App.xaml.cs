using Autofac;
using System.Windows;
using Planner.WPF.Windows;
using Planner.WPF.DIContainer;
using System.Net;
using Planner.Model.Services;
using Planner.ViewModel.ViewModels;
using Planner.Model;

namespace Planner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CheckForInternetConnection();
            var container = ContainerConfig.Configure();
            var userService = container.Resolve<IUserService>();
            var loginWindow = container.Resolve<LoginWindow>(new TypedParameter(typeof(IUserViewModel), 
                container.Resolve<IUserViewModel>(new TypedParameter(typeof(IUserService), userService))));

            loginWindow.Show();

            loginWindow.Closed += (s, e) => 
            { 
                this.MainWindow = container.Resolve<MainWindow>(new TypedParameter(typeof(IScheduleViewModel),
                    container.Resolve<IScheduleViewModel>(new TypedParameter(typeof(IScheduleService), 
                    container.Resolve<IScheduleService>(new TypedParameter(typeof(IUserService), userService))), new TypedParameter(typeof(UserModel), userService.User))));
                MainWindow.Show();
            };
        }

        private void CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return;
                    
            }
            catch
            {
                var result = MessageBox.Show(Planner.WPF.Properties.Resources.connectionError, Planner.WPF.Properties.Resources.connectionErrorCaption);
                this.Shutdown();
            }
        }
    }
}
