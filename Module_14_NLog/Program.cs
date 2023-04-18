using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace Module_14_NLog
{   
    // https://github.com/nlog/nlog/wiki/Tutorial#Configure-NLog-Targets-for-output
    // https://github.com/nlog/nlog/wiki/Configure-from-code
    // https://yougame.biz/threads/200323/
    // Layouts https://nlog-project.org/config/?tab=layouts
    // Targets https://nlog-project.org/config/?tab=targets
    // Layout renderers https://nlog-project.org/config/?tab=layout-renderers

    internal class Program
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {

            #region NLog Initializator

            var config = new NLog.Config.LoggingConfiguration();

            //-------------------- Target Console ----------------------------------------

            var consoleTarget = new ColoredConsoleTarget()
            {
                //Layout = @"${longdate}|${level:uppercase=true}|${logger}|${message}"
                Layout = @"${counter}|[${date:format=yyyy-MM-dd HH\:mm\:ss}] [${logger}/${uppercase: ${level}}] >> ${message} ${exception: format=ToString}"

            };
            // Rules for mapping loggers to targets
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);

            //-------------------- Target File ----------------------------------------

            var logfile = new FileTarget();
            //if (!Directory.Exists("logs"))
            //    Directory.CreateDirectory("logs");
            config.AddRule(LogLevel.Error, LogLevel.Fatal, logfile);       

            logfile.CreateDirs = true;
            logfile.FileName = $"logs{Path.DirectorySeparatorChar}lastlog.log";
           
            logfile.Layout = @"${counter}|[${date:format=yyyy-MM-dd HH\:mm\:ss}] [${logger}/${uppercase: ${level}}] >> ${message} ${exception: format=ToString}";
           
            logfile.KeepFileOpen = true;

            // Apply config
            NLog.LogManager.Configuration = config;

            #endregion NLog Initializator

            #region NLog Colors

            var Trace = new ConsoleRowHighlightingRule();
            Trace.Condition = ConditionParser.ParseExpression("level == LogLevel.Trace");
            Trace.ForegroundColor = ConsoleOutputColor.Yellow;
            consoleTarget.RowHighlightingRules.Add(Trace);
            var Debug = new ConsoleRowHighlightingRule();
            Debug.Condition = ConditionParser.ParseExpression("level == LogLevel.Debug");
            Debug.ForegroundColor = ConsoleOutputColor.DarkCyan;
            consoleTarget.RowHighlightingRules.Add(Debug);
            var Info = new ConsoleRowHighlightingRule();
            Info.Condition = ConditionParser.ParseExpression("level == LogLevel.Info");
            Info.ForegroundColor = ConsoleOutputColor.Green;
            consoleTarget.RowHighlightingRules.Add(Info);
            var Warn = new ConsoleRowHighlightingRule();
            Warn.Condition = ConditionParser.ParseExpression("level == LogLevel.Warn");
            Warn.ForegroundColor = ConsoleOutputColor.DarkYellow;
            consoleTarget.RowHighlightingRules.Add(Warn);
            var Error = new ConsoleRowHighlightingRule();
            Error.Condition = ConditionParser.ParseExpression("level == LogLevel.Error");
            Error.ForegroundColor = ConsoleOutputColor.DarkRed;
            consoleTarget.RowHighlightingRules.Add(Error);
            var Fatal = new ConsoleRowHighlightingRule();
            Fatal.Condition = ConditionParser.ParseExpression("level == LogLevel.Fatal");
            Fatal.ForegroundColor = ConsoleOutputColor.Black;
            Fatal.BackgroundColor = ConsoleOutputColor.DarkRed;
            consoleTarget.RowHighlightingRules.Add(Fatal);

            #endregion NLog Colors

            try
            {

                int var = 100;
                double rez = 0;

                for (int i = 10; i >= -10 ; i--)
                {
                    rez = var / i;
                    Console.WriteLine($"Result = {rez}");
                    Logger.Info("The value is " + rez);
                    Logger.Warn("The value is " + rez);
                    Thread.Sleep( 100 );
                }                           
            }
            catch (Exception ex)
            {
                Logger.Error("Error " + ex.Message);
                Logger.Fatal("Fatal Error! " + ex);
            }
        }
    }
}