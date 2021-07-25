using Inventory_Management.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-IN");
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-IN");
            base.OnStartup(e);

            DispatcherUnhandledException += app_DispatcherUnhandledException;
            initializeCultures();
            initializeServices();

#if DEBUG
            //ThreadPool.QueueUserWorkItem(_ =>
            //{
            //    test().ConfigureAwait(false).GetAwaiter().GetResult();
            //});
#endif
        }

        private async Task test()
        {
            var data = Global.Services.GetService<IDataService>();
            var souces = data.GetDataSources();
            if (souces == null)
            {
                await data.AddDatasource(Global.DataSource = new DataSource
                {
                    CreatedOn = DateTime.Now,
                    FromDate = new DateTime(2021, 7, 1),
                    ToDate = new DateTime(2021, 7, 31),
                    Inventories = new List<Models.Inventory>
                    {
                        //new Models.Inventory
                        //{
                        //    Id = Guid.NewGuid().ToString("n"),
                        //    Name = "TOP @ 20154"
                        //}
                    }
                });
            }
            else
            {
                Global.DataSource = data.GetDataSource(souces[0]);
            }
        }

        private void initializeServices()
        {
            // initialize service in here
            Global.Services = new DefaultServices();
            
            Global.Services.Register<IDataService>(new DataService());
            Global.Services.Register<IInventoryService>(new InventoryService());
        }

        private void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            try
            {
                //var logger = Global.Services.GetService<ILoggerService>().CreateLogger(typeof(App).FullName);
                //logger.LogCritical($"Unhandled exception occurred of type {e.Exception.GetType().FullName}\nMessage:{e.Exception.Message}\nInner Exception: {e.Exception.InnerException?.GetType().FullName}\n{e.Exception.StackTrace}");

                System.IO.File.AppendAllText("cretical1.log", $"Unhandled exception occurred of type {e.Exception.GetType().FullName}\nMessage:{e.Exception.Message}\nInner Exception: {e.Exception.InnerException?.GetType().FullName}\n{e.Exception.StackTrace}");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("cretical.log", $"\n{ex.Message}");
            }
        }

        private static void initializeCultures()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-IN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-IN");

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}
