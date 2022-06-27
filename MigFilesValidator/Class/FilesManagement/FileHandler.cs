using System;
using System.Collections.Generic;
using System.Text;
using PasifaeG3Migrations.Class.Extras;

namespace GuaraniMigFilesScanner.Class.FilesManagement
{
    using GuaraniMigFilesScanner.Class.MigFiles;

    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ExHandler
    {
        public static void Handle(Exception ex, bool log = false)
        {
            Console.WriteLine($"###.### EXCEPTION ###.###");

            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");

            Console.WriteLine($"###.### EXCEPTION ###.###");

            if (log) Log(ex);
        }
        public static async Task HandleAsync(Exception ex, int delay = 2000, bool log = false)
        {
            Console.WriteLine($"###.### EXCEPTION ###.###");

            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");

            Console.WriteLine($"###.### EXCEPTION ###.###");

            if (log) Log(ex);

            await Task.Delay(delay);
        }
        public static void Log(Exception ex)
        {
            Console.WriteLine($"Exception TO ! LOG {ex.Message}");

            try
            {
                string filePath;
                string fileName = $"Exception {DateTime.Now.ToShortDateString()}.txt";

                filePath = $"{FileHandler.LogsPath}{fileName}";

                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"###.### EXCEPTION ###.###");

                sb.AppendLine($"Message: {ex.Message}");
                sb.AppendLine($"StackTrace: {ex.StackTrace}");

                sb.AppendLine($"###.### EXCEPTION ###.###");

                FileHandler.AppendText(filePath, sb.ToString());
            }

            catch (Exception exx) { Console.WriteLine($"Exception !! en LOG {exx.Message}"); }
        }
    }
    public static class FileHandler
    {
        public static string MigFilesPath => $"{Directory.GetCurrentDirectory()}\\Mig Files\\";
        public static string RulesSourcePath => $"{Directory.GetCurrentDirectory()}\\Rules Source\\";
        public static string OutputFilePath => $"{Directory.GetCurrentDirectory()}\\Output Files\\";
        public static string LogsPath => $"{Directory.GetCurrentDirectory()}\\Logs\\";
        public static string ResourcesPath => $"{Directory.GetCurrentDirectory()}\\Resources\\";

        public static string GetScannerOutPutPath(MigFileType type)
        {

            string newDir = $"{OutputFilePath}{type} Resultados {DateTime.Now.TimeOfDay:hh\\-mm}hs";
            { }
            Directory.CreateDirectory(newDir);
            return $"{newDir}\\";
        }


        public static void VerifyPaths()
        {
            try
            {
                if (!Directory.Exists(MigFilesPath))
                    Directory.CreateDirectory(MigFilesPath);

                if (!Directory.Exists(RulesSourcePath))
                    Directory.CreateDirectory(RulesSourcePath);

                if (!Directory.Exists(OutputFilePath))
                    Directory.CreateDirectory(OutputFilePath);

                if (!Directory.Exists(LogsPath))
                    Directory.CreateDirectory(LogsPath);

                if (!Directory.Exists(ResourcesPath))
                    Directory.CreateDirectory(ResourcesPath);
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, true);
            }
        }
        public static string[] GetFiles(string path) => Directory.GetFiles(path);
        public static void AppendText(string filePath, string text) { using StreamWriter sw = File.AppendText(filePath); sw.WriteLine(text); }
        public static string[] GetFileLines(string filePath)
        {
            List<string> lines = new List<string>();

            foreach (var l in File.ReadLines(filePath, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(l)) continue;
                if (l.Split('|').IsNullOrEmpty()) continue;
                lines.Add(l);
            }
            return lines.ToArray();
        }


        public static async ValueTask<string[]> GetFileLinesParallelAsync(string filePath)
        {
            List<string> lines = new List<string>();

            await Task.Run(() =>
            {
                var obj = new object();

                Parallel.ForEach(File.ReadLines(filePath, Encoding.UTF8), (l) =>
                {
                    if (string.IsNullOrWhiteSpace(l)) return;

                    if (l.Split('|').IsEmpty()) return;

                    lock (obj) lines.Add(l);
                });
            });

            return lines.ToArray();
        }

        public static string GetRuleSourcesFile(MigFileType type)
        {
            string[] rulesSources = Directory.GetFiles(RulesSourcePath);

            var auxRuleSourcePath = rulesSources.Where(r => r.Contains(type.ToString()));

            if (auxRuleSourcePath.Count() > 1)
            {
                //Console.WriteLine($"ERROR: Hay más de un archivo regla para {type}");

                auxRuleSourcePath = rulesSources.Where(r => r.Split('\\').Last().Equals($"{type}.cfg"));

                return auxRuleSourcePath.First();
            }
            else if (auxRuleSourcePath.Count() == 0)
            {
                Console.WriteLine($"ERROR: No Hay ningún archivo regla para {type}");
                return null;
            }
            else
            {
                return auxRuleSourcePath.First();
            }
        }

    }
}
