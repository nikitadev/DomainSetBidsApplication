/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DomainSetBidsApplication"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using System.IO;
using AviaTicketsWpfApplication.Fundamentals;
using DomainSetBidsApplication.Fundamentals;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Fundamentals.Services;
using DomainSetBidsApplication.Models;
using DomainSetBidsApplication.ViewModels.Pages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RegAPI.Library;
using RegAPI.Library.Models.Interfaces;

namespace DomainSetBidsApplication.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public sealed class ViewModelLocator
    {
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public UserInfoViewModel UserInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserInfoViewModel>();
            }
        }

        public MonitorViewModel Monitor
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MonitorViewModel>();
            }
        }

        public LoggingViewModel Logging
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoggingViewModel>();
            }
        }

        public AddDomainPageViewModel AddDomainPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddDomainPageViewModel>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IApiFactory, ApiFactory>();

            var connection = new DbConnection("data");

            SimpleIoc.Default.Register<IDbConnection>(() => connection);
            SimpleIoc.Default.Register<IRepository<LogEntity>, MainRepository<LogEntity>>();
            SimpleIoc.Default.Register<IRepository<UserInfoEntity>, MainRepository<UserInfoEntity>>();
            SimpleIoc.Default.Register<IRepository<RegDomainEntity>, MainRepository<RegDomainEntity>>();

            SimpleIoc.Default.Register<ILogService, LogService>();
            SimpleIoc.Default.Register<IUserInfoService, UserInfoService>();
            SimpleIoc.Default.Register<IRegDomainService, RegDomainService>();

            SimpleIoc.Default.Register<Bootstrapper>();

            SimpleIoc.Default.Register<MainViewModel>();
            //SimpleIoc.Default.Register<MonitorViewModel>();
            SimpleIoc.Default.Register<UserInfoViewModel>();
            SimpleIoc.Default.Register<AddDomainPageViewModel>();
        }

        public static Uri GetPathPage(Type typeViewModel)
        {
            if (typeViewModel == null)
                return null;

            string name = typeViewModel.Name.Replace("Model", String.Empty);

            string path = Path.Combine("Pages", Path.ChangeExtension(name, "xaml"));

            return new Uri(String.Concat("pack://application:,,,/Views/", path));
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<IDbConnection>();
        }
    }
}