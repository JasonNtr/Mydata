using System.ComponentModel;
using System.IO;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Business.ApiServices;
using Business.Mappings;
using Business.Services;
using Domain.DTO;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Mydata.ViewModels;
using Application = System.Windows.Application;
using System;
using Hardcodet.Wpf.TaskbarNotification;

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
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);



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
            services.AddDbContext<ApplicationDbContext>
            (options => options.UseSqlServer(
                Configuration.GetConnectionString("Default")), 
                ServiceLifetime.Transient);
            services.AddScoped<IInvoiceRepo, InvoiceRepo>();
            services.AddScoped<IInvoiceService, InvoiceService>();

            services.AddScoped<IExpenseRepo, ExpenseRepo>();
            services.AddScoped<IExpenseService, ExpenseService>();

            services.AddScoped<IIncomeRepo, IncomeRepo>();
            services.AddScoped<IMyDataIncomeResponseRepo, MyDataIncomeResponseRepo>();
            services.AddScoped<IIncomeService, IncomeService>();

            services.AddScoped<IMyDataResponseRepo, MyDataResponseRepo>();
            services.AddScoped<IMyDataCancellationResponseRepo, MyDataCancellationResponseRepo>();
            services.AddScoped<IMyDataCancelInvoiceRepo, MyDataCancelInvoiceRepo>();
            services.AddScoped<MainWindow>();
            services.AddScoped<IParticleInform, ParticleInform>();
            services.AddAutoMapper(typeof(MappingProfiles));

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            MainWindow = ServiceProvider.GetService<MainWindow>();
            Application.Current.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

         
         

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {    
            #if DEBUG   // In debug mode do not custom-handle the exception, let Visual Studio handle it
                e.Handled = false;
            #else
                ShowUnhandledException(e);  
            #endif     
        }

        private void ShowUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            string errorMessage = string.Format(
                "An application error occurred.\nPlease check whether your data is correct and repeat the action. If this error occurs again there seems to be a more serious malfunction in the application, and you better close it.\n\nError: {0}\n\nDo you want to continue?\n(if you click Yes you will continue with your work, if you click No the application will close)",
                e.Exception.Message + (e.Exception.InnerException != null ? "\n" + e.Exception.InnerException.Message : null));

            if (System.Windows.MessageBox.Show(errorMessage, "Application Error", MessageBoxButton.YesNoCancel, MessageBoxImage.Error) == MessageBoxResult.No)
            {
                if (System.Windows.MessageBox.Show("WARNING: The application will close. Any changes will not be saved!\nDo you really want to close it?", "Close the application!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    Application.Current.Shutdown();
            }
        }
    }
}
