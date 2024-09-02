using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            logger = new Logger("log");
            logger.Info(CreateMessage("Start ScrapFile.exe"));

            var inputFile = "C:\\Works\\File\\Input\\ACP-FBL5N-15.05.2024.txt";
            var resultFileName = "result.csv";
            var outputFile = $"C:\\Works\\File\\Output\\{resultFileName}";
            var searchString = "RV,ZR";
            var searchStrValue = searchString.Split(',').ToList();
            var separator = "|";
            var replaceSeparator = ",";

            try
            {
                Process(inputFile, outputFile, separator, replaceSeparator, searchStrValue);
                logger.Info(CreateMessage($"Finish"));
            }
            catch (Exception ex)
            {
                logger.Error(CreateMessage(ex.Message), ex);
            }


        }

        static void Process(string inputFile, string outputFile, string separator, string replaceSeparator, List<string> searchStr)
        {
            logger.Info(CreateMessage($"Read from {inputFile}"));
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

            logger.Info(CreateMessage($"Write to {outputFile}"));
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
