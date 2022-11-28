using Business.Services;
using Domain.DTO;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace Mydata
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; private set; }

        private TaskbarIcon notifyIcon;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg1MjUyQDMxMzgyZTM0MmUzMFNHSUlDTlNGMW4vUTRtZ1hGUDVuWGUrSWVzdHBTa2JZcnFEMUhhK2RnQWs9");

            // Global exception handling
            Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)));
            //services.AddDbContext<ApplicationDbContext>
            //(options => options.UseSqlServer(
            //    Configuration.GetConnectionString("Default")),
            //    ServiceLifetime.Transient);

            //services.AddAutoMapper(typeof(MappingProfiles));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            MainWindow = new MainWindow(ServiceProvider);
            Application.Current.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => e.Handled = false;

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => e.Handled = false;

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // we cannot handle this, but not to worry, I have not encountered this exception yet.
            // However, you can show/log the exception message and show a message that if the application is terminating or not.
            LogUnhandledException(e);
        }

        private void LogUnhandledException(UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMessage = string.Format(
               "An application error occurred.\nPlease check whether your data is correct and repeat the action. If this error occurs again there seems to be a more serious malfunction in the application, and you better close it.\n\nError: {0}\n",
                ex.ToString() + (ex.InnerException != null ? "\n" + ex.InnerException.Message : null));
                LogProgramError.WriteError(errorMessage);
            }
            catch
            {
                // do nothing to silently swallow error, or try something else...
            }
        }
    }
}