using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartBackupApp.App
{
    class Program
    {
        private static bool _gobacktosleep      = Boolean.Parse(ConfigurationManager.AppSettings["SleepOnCompletion"]);
        private static bool _zipData            = Boolean.Parse(ConfigurationManager.AppSettings["ZipResult"]);
        private static int _versions            = int.Parse(ConfigurationManager.AppSettings["Versions"]);
        private static string _source           = ConfigurationManager.AppSettings["Source"];
        private static string _target           = ConfigurationManager.AppSettings["Target"];
        private static string _targetInstance   = $"{_target}\\{DateTime.Now.ToString("yyyy-MM-dd-HHmmssfff")}\\";

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            try {
                SetupConsole();
                LogCurrentSettings(ConfigurationManager.AppSettings);

                var fileService     = new Services.FileService();
                var stateService    = new Services.StateService();
                var zipService      = new Services.ZipService();

                _logger.Info("##### 1. Transfer data");
                fileService.DirectoryCopy(_source, _targetInstance, true);

                _logger.Info("##### 2. Cleanup obsolete data");
                fileService.CleanDirectory(_target, _versions);

                _logger.Info("##### 3. Zip target");
                ZipTarget(zipService);

                _logger.Info("##### 4. Set machine state");
                SetState(stateService);

                _logger.Info($"All task's completed, {_source} has succesfully been backed up to {_target}");
                if (Debugger.IsAttached)
                    Console.ReadKey();
            }
            catch (Exception ex)
            {
                _logger.Info("To see more verbose logging, enable 'Debug' or 'Trace' logging in the NLog.config.");
                _logger.Fatal(ex, "Application crashed:");
                throw;
            }
        }

        private static void ZipTarget(Services.ZipService zipService)
        {
            if (_zipData) { zipService.ZipFolder(_targetInstance, true); }
            else { _logger.Trace("Skipping task, due to configuration..."); }
        }

        private static void SetState(Services.StateService stateService)
        {
            if (_gobacktosleep)
            {
                _logger.Trace("Putting machine to sleep");
                if (stateService.Sleep(true))
                    _logger.Trace("Putting machine to sleep.");
                else
                    _logger.Error("Unable to force machine to sleep");

            }
            else
            { 
                _logger.Trace("Skipping task, due to configuration...");
            }
        }
        private static void LogCurrentSettings(NameValueCollection settings)
        {
            foreach (var key in settings.AllKeys)
            {
                _logger.Info($"Settings: {key} = '{settings[key]}'");
            }
        }
        private static void SetupConsole()
        {
            Console.WindowWidth = Console.LargestWindowWidth / 2;
            Console.WindowHeight = Console.LargestWindowHeight / 3;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@" _________                      __    __________                __                      _____                 ");
            Console.WriteLine(@"/   _____/ _____ _____ ________/  |_  \______   \_____    ____ |  | ____ ________      /  _  \ ______ ______  ");
            Console.WriteLine(@" \_____  \ /     \\__  \\_  __ \   __\  |    |  _/\__  \ _/ ___\|  |/ /  |  \____ \   /  /_\  \\____ \\____ \ ");
            Console.WriteLine(@" /        \  Y Y  \/ __ \|  | \/|  |    |    |   \ / __ \\  \___|    <|  |  /  |_> > /    |    \  |_> >  |_> >");
            Console.WriteLine(@"/_______  /__|_|  (____  /__|   |__|    |______  /(____  /\___  >__|_ \____/|   __/  \____|__  /   __/|   __/");
            Console.WriteLine(@"        \/      \/     \/                      \/      \/     \/     \/     |__|             \/|__|   |__|   ");
            Console.WriteLine($"                                                                                        Author: Jais Edelmann");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{Assembly.GetExecutingAssembly().FullName}");
            Console.WriteLine(@"");
            Console.WriteLine(@"");
            Console.WriteLine(@"");
        }
    } 
}
 