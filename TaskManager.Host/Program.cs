using Microsoft.Owin.Hosting;
using System;
using TaskManager.Api;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8000"))
            {
                Console.WriteLine("Сервер запущен. Нажмите любую клавишу для завершения работы...");
                Console.ReadLine();
            }
        }
    }
}
