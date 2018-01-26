using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System.Collections.Specialized;
using System.Configuration;
using System.Windows;

namespace PlateRecognitionSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public  partial class App : Application
    {
        public App()
        {
            StartWebApp();
        }
        
        public void StartWebApp()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            var url = appSettings["SignalrURL"]; 
            WebApp.Start(url);
        }
    }
}
