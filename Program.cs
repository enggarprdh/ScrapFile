using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScrapFile
{
    internal class Program
    {

        private static Logger logger;
        private static string RootPath;
        private static bool ParserError = false;
        private static Stopwatch _watch;
        static void Main(string[] args)
        {
            logger = new Logger("log");
            InitLogger();
            var parser = new Parser(config =>
            {
                config.IgnoreUnknownArguments = false;
                config.CaseSensitive = false;
                config.AutoHelp = true;
                config.AutoVersion = true;
                config.HelpWriter = Console.Error;
            });

            var param = parser.ParseArguments<ScrapDto>(args)
                .WithParsed<ScrapDto>(x => DoExtract(x))
                .WithNotParsed(errors => HandleParseError(errors)); ;

            if (!ParserError)
            {
                _watch.Stop();
                logger.Debug($"Application Finished. Elapsed time: {_watch.ElapsedMilliseconds}ms");
            }

        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            ParserError = true;

            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            {
            }
            else
                Console.WriteLine("Parameter unknown, please check the documentation or use parameter '--help' for more information");


        }

        static void DoExtract(ScrapDto param)
        {
            var inputFile = param.InputFile;
            var outputFile = param.OutputFile;
            var separator = param.Separator;
            var replaceSeparator = param.ReplaceSeparator;
            var searchStr = param.SearchStrValue.Split(',').ToList();

            try
            {

                _watch = new Stopwatch();
                _watch.Start();
                logger.Info("Start ScrapFile.exe");
                Process(inputFile, outputFile, separator, replaceSeparator, searchStr);


            }
            catch (Exception ex)
            {
                logger.Error(CreateMessage(ex.Message), ex);
            }
            
        }

        static void Process(string inputFile, string outputFile, string separator, string replaceSeparator, List<string> searchStr)
        {
            logger.Info($"Read from {inputFile}");
            List<string> lines = System.IO.File.ReadAllLines(inputFile).ToList();
            List<string> result = new List<string>();


            foreach (var line in lines)
            {
                var res = searchStr.Where(x => line.Contains(x)).FirstOrDefault();
                if (res == null)
                    continue;

                var newStr = line.Replace(separator, replaceSeparator);
                result.Add(newStr);
            }

            logger.Info($"Write to {outputFile}");
            System.IO.File.WriteAllLines(outputFile, result);
        }

        static void InitLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            RootPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            var logFile = new NLog.Targets.FileTarget("logfile");
            logFile.FileName = $"{Path.Combine(Path.Combine(RootPath, $"{DateTime.Now.ToString("yyyyMMdd")}.log"))}";

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Error, logFile);

            NLog.LogManager.Configuration = config;
        }

        static string CreateMessage(string message)
        {
            string msg = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}";
            return msg;
        }
    }
}
