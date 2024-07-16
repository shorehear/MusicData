using System;
using System.IO;

namespace JulyPractice
{
    public static class Logger
    {
        private static readonly string logFilePath = "application.log";
        private static readonly object lockObj = new object();

        public static void Log(string message)
        {
            string formattedMessage = FormatMessage("LOG", message);
            WriteToFile(formattedMessage);
            Console.WriteLine(formattedMessage);
        }

        public static void LogInformation(string message)
        {
            string formattedMessage = FormatMessage("INFO", message);
            WriteToFile(formattedMessage);
            Console.WriteLine(formattedMessage);
        }

        public static void LogError(string message)
        {
            string formattedMessage = FormatMessage("ERROR", message);
            WriteToFile(formattedMessage);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(formattedMessage);
            Console.ResetColor();
        }

        public static void LogWarning(string message)
        {
            string formattedMessage = FormatMessage("WARN", message);
            WriteToFile(formattedMessage);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(formattedMessage);
            Console.ResetColor();
        }

        private static string FormatMessage(string level, string message)
        {
            return $"{DateTime.Now.ToString("g")} [{level}]: {message}";
        }

        private static void WriteToFile(string formattedMessage)
        {
            lock (lockObj)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine(formattedMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при записи лога: {ex.Message}");
                }
            }
        }
    }
}
