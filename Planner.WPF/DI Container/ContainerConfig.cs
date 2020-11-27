using Autofac;
using Planner.Model;
using Planner.Model.Repositories;
using Planner.WPF.Windows;
using Planner.ViewModel.ViewModels;
using Planner.WPF.UserControls;

namespace Planner.WPF.DIContainer
{
    public static class ContainerConfig
    {

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<ScheduleViewModel>().As<IScheduleViewModel>();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<ScheduleDbContext>().AsSelf();
            builder.RegisterType<UserControlFactory>().As<IUserControlFactory>();

            return builder.Build();
        }
    }
}
