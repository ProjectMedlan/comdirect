using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Comdirect
{
    internal static class Program
    {
        public static IConfiguration? Configuration;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            Configuration = builder.Build();

            ApplicationConfiguration.Initialize();
            Application.Run(new FrmMain());
        }
    }
}