using Microsoft.Owin.Hosting;
using System;

namespace backendApp
{
    public class Program
    {
        private const string HOST = "http://localhost:9000/";

        static void Main()
        {
            // Start OWIN host 
            using (WebApp.Start<Startup>(url: HOST))
            {
                Console.WriteLine("Web Server is running.");
                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
            }
        }
    }
}