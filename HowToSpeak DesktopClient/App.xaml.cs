using HowToSpeak_DesktopClient.Infrastructure.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HowToSpeak_DesktopClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var application = new App();
            var server = HTSServer.getInstance(ConfigurationManager.AppSettings.Get("BASE_URL_SERVER"));
            application.InitializeComponent();
            application.Run();
        }
    }
}
