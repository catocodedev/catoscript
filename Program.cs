using System;
using System.Net;

namespace cato
{
    class cato
    {
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return string.Empty;
            }
        }       

        static void Execute(string line)
        {
            if (line.StartsWith("console.send"))
            {
                Console.WriteLine(getBetween(line, "/\"", "\\\""));
            }
            else if (line.StartsWith("log.write")) 
            {
                Console.WriteLine(getBetween(line, "/\"", "\\\""));
            }
        }

        static void Run(string file) 
        {
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
                            Execute(s);
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
        }
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

                    case "help":
                        Console.WriteLine("----------CatoScript Help-------------------");
                        Console.WriteLine("run - Runs a .cato file");
                        Console.WriteLine("version/ver - shows version of catoscript");
                        Console.WriteLine("--------------------------------------------");
                        break;

                    case "pur":
                        switch (args[1])
                        {
                            case "get":
                                Client.DownloadFile("http://script.cato.fun/pkgs/"+ args[1] +"/data/"+ args[1] +".catop", "./logo.png");
                                break;
                            case "help":
                                Console.WriteLine("Pur Help");
                                break;
                        }
                        break;

                    case "run":
                     Run(args[1]);
                        break;
                    default:
                        if (args[0].Contains(".cato"))
                        {
                            Run(args[0]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid syntax. try help");
                            Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + ".exe (ver/pur/run) [filename/get]");
                        }
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
