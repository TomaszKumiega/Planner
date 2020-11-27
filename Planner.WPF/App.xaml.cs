using Autofac;
using System.Windows;
using Planner.WPF.Windows;
using Planner.WPF.DIContainer;
using System.Net;

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
            this.MainWindow = container.Resolve<MainWindow>();
            MainWindow.Show();
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
