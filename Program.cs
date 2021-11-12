using System;
using System.Net;

namespace cato
{
    class cato
    {
        static void Main(String[] args)
        {
            WebClient Client = new();
            string version = "Dev1.0.0";
            try
            {
                switch (args[0])
                {
                    case "ver":
                        Console.WriteLine(version);
                        break;

                    case "version":
                        Console.WriteLine(version);
                        break;

                    case "pur":
                        switch (Console.ReadLine())
                        {
                            case "get":
                                Client.DownloadFile("http://script.cato.fun/assets/logo.png", "./logo.png");
                                break;
                        }
                        break;

                    case "run":
                        string file = args[1];
                        string fileExt = System.IO.Path.GetExtension(file);
                        if (file != null)
                        {
                            if (fileExt == ".cato")
                            {
                                if (File.Exists(file))
                                {
                                    string[] readText = File.ReadAllLines(file);
                                    foreach (string s in readText)
                                    {
                                        Console.WriteLine(s);

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("File doesn't exist.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("File not a catoscript file");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please write a file name after run");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid syntax.");
                        Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + ".exe (ver/pur/run) [filename/get]");
                        break;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Invalid syntax.");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + ".exe (ver/pur/run) [filename/get]");
                Console.WriteLine();
                Console.WriteLine(err.ToString());
                Console.WriteLine();
            }
            Console.WriteLine("Execution ended. Press any key to close CatoScript...");
            Console.ReadKey();
        }
    }
}
