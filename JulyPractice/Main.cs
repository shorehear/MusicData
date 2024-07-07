using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Net;
using System.Windows;

namespace JulyPractice
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AuthorizationWindow auth = new AuthorizationWindow();
            auth.Show();

            Application app = new Application();
            app.Run();
        }
    }
}
