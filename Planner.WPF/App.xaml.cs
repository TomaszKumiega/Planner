using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Planner.WPF;
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
