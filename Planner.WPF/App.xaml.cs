using Autofac;
using System.Windows;
using Planner.WPF.Windows;
using Planner.WPF.DIContainer;

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

            var container = ContainerConfig.Configure();
            this.MainWindow = container.Resolve<MainWindow>();
            MainWindow.Show();
        }
    }
}
