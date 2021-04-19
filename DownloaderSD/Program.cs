using System;
using System.Configuration;

namespace DownloaderSD
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = string.Empty;
            //uri = "http://wallpaperswide.com/architecture-desktop-wallpapers.html";
            //uri = "http://wallpaperswide.com/architecture-desktop-wallpapers/page/";
            //uri = "http://wallpaperswide.com/modern_house_design_winter_landscape-wallpapers.html";

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Batch download of wallpapers started...");

            uri = ConfigurationManager.AppSettings["MasterUrl"];

            Process p = new Process();
            p.Start(uri);

            Console.ReadLine();
        }

    }
}
