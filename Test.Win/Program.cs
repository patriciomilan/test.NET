using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Test.Win.Proxy.OnConfigure += Proxy_OnConfigure;
            Application.Run(new Personas());
        }

        private static void Proxy_OnConfigure(ProxyConfiguracion configuracion)
        {
            configuracion.UrlBase = System.Configuration.ConfigurationManager.AppSettings["urlBase"];
        }
    }
}
