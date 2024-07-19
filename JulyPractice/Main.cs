using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace JulyPractice
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AllocConsole();
            Logger.LogInformation("Запуск приложения.");
            try
            {
                using (var context = new CurrentDbContext()) 
                {
                    DatabaseBackupManager backupManager = new DatabaseBackupManager();
                    backupManager.BackupDatabase(context);
                }
                AuthorizationWindow auth = new AuthorizationWindow();
                auth.Show();

                Application app = new Application();
                app.Run();
            }
            catch (Exception ex) 
            {
                Logger.LogError($"Ошибка во время работы приложения: {ex.Message}");
            }
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
