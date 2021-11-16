using System;
using System.Net;
using System.Runtime.InteropServices;

namespace cato
{
    public class CatoData
    {
        public static string version = "Dev0.1.0";
        public static string purver = "Dev0.1.0";
        public string OS = "null";
    }
    internal static class cato
    {
        public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }

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
        static void catoexception (string type, string info, string line, int linenum, int errornum)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine(type +"Execption: "+ info + " | " + line + "(line:"+ linenum +")");
            Console.WriteLine("ERROR CODE : " + errornum);
            Console.ReadKey();
            System.Environment.Exit(errornum);

        }
        static void cli()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("-------- CatoScript " + CatoData.version + " ----------");
            Console.WriteLine("Cato CLI ready!");
            string input = "";
            while (input != "exit")
            {
                input = Console.ReadLine();
                switch (input)
                {
                    case "exit":
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Environment.Exit(0);
                        break;
                    case "run":
                        Console.WriteLine("Type the name/path of the file");
                        string tmp1 = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Run(tmp1);
                        Console.WriteLine("Cato File finnished running. Press any key to retrun to CLI");
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("-------- CatoScript " + CatoData.version + " ----------");
                        Console.WriteLine("Cato CLI ready!");
                        break;
                    case "ver":
                        Console.WriteLine(CatoData.version);
                        break;

                    case "version":
                        Console.WriteLine(CatoData.version);
                        break;
                    case "pur":
                        pur();
                        input = "";
                        break;
                    case "help":
                        Console.WriteLine("----------CatoScript Help-------------------");
                        Console.WriteLine("run - Runs a .cato file");
                        Console.WriteLine("version/ver - shows version of catoscript");
                        Console.WriteLine("exit - leave the CLI");
                        Console.WriteLine("pur - open PUR CLI");
                        Console.WriteLine("--------------------------------------------");
                        break;
                    default:
                        Console.WriteLine("Unknown Command! Try help");
                        break;
                }
            }
        }
            static void pur()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
                Console.WriteLine("--------PUR "+ CatoData.purver +"----------");
                Console.WriteLine("PUR CLI ready!");
            string purcmd = "";
            while (purcmd != "quit")
            {
                purcmd = Console.ReadLine();
                switch (purcmd)
                {
                    case "get":
                        // Client.DownloadFile("https://script.cato.fun/pkgs/" + args[2] + "/data/" + args[2] + ".catop", "./logo.png");
                        break;
                    case "help":
                        Console.WriteLine("------Pur Help-------");
                        Console.WriteLine("quit - retrun to CatoScript CLI");
                        break;
                    case "quit":
                        cli();
                        break;
                    default:
                        Console.WriteLine("Unknown Command! Try help");
                        break;
                }
            }   
        }
        static void Execute(string line, int linenumber, CatoData catoData)
        {
            if (line.StartsWith("%"))
            {
                // nothing because comment
            }
            else if (line.StartsWith("console.send "))
            {
                if (getBetween(line, "|\"", "\"|") != String.Empty)
                {
                    Console.WriteLine(getBetween(line, "|\"", "\"|"));
                }
                else
                {
                    catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nconsole.send can not send an empty string.\"", line, linenumber, 101);
                }
            }
            else if (line.StartsWith("debug.throw "))
            {
                catoexception("UserGenerated", getBetween(line, "|\"", "\"|"), line, linenumber, 200);
            }
            else if (line.StartsWith("random.num "))
            {
                Random engine = new();
                //this should grab |min:max|
                try
                {
                    int min = Int32.Parse(getBetween(line, "|", "~"));
                    int max = Int32.Parse(getBetween(line, "~", "|"));
                    int value = engine.Next(min, max);
                    Console.WriteLine(value);
                }
                catch (FormatException)
                {
                    catoexception("InvalidInput", getBetween(line, "|", "~") + " & " + getBetween(line, "~", "|") + " are invalid", line, linenumber, 102);
                }
            }
            else if (line.StartsWith("script.pause.time "))
            {
                try
                {
                    Thread.Sleep(Int32.Parse(getBetween(line, "|", "|")));
                }
                catch (FormatException)
                {
                    catoexception("InvalidInput", getBetween(line, "|", "|") + " is invalid", line, linenumber, 102);
                }
            }
            else if (line.StartsWith("script.pause.keywait "))
            {
                Console.WriteLine("Script Paused! untill user presses a key");
                Console.ReadKey();
            }
            else if (line.StartsWith("script.quit#"))
            {
                System.Environment.Exit(0);
            }
            else if (line.StartsWith("open.webpage "))
            {
                string site = "";
                if (getBetween(line, "|\"", "\"|") != String.Empty)
                {
                    site = getBetween(line, "|\"", "\"|");
                    System.Diagnostics.Process.Start(site);
                }
                else
                {
                    catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nopen.site can not open an empty string.\"", line, linenumber, 101);
                }
            
            }
            else if (line.StartsWith("get.OS#"))
            {
                Console.WriteLine("The User OS is " + catoData.OS);
            }
            else
            {
                catoexception("InvalidFunction", "\"This function was not recognized.\"", line, linenumber, 100);
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
                        readText.Each((str, n) =>
                        {
                            Execute(str, n+1, CatoData);
                        });

                        //foreach (string s in readText)
                       // {
                        //    Execute(s);
                       // }
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
        static void Main(String[] args, CatoData catoData)
        {
            // static readonly HttpClient Client = new HttpClient();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                catoData.OS = "Linux";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                catoData.OS = "Windows";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                catoData.OS = "OSX";
            }
            if (args == null || args.Length == 0)
            {
                cli();
            }
            else
            {
                try
                {
                    switch (args[0])
                    {
                        case "ver":
                            Console.WriteLine(CatoData.version);
                            break;

                        case "version":
                            Console.WriteLine(CatoData.version);
                            break;

                        case "pur":
                            pur();
                            break;

                        case "":
                            cli();
                            break;
                        default:
                            if (args[0].Contains(".cato"))
                            {
                                Run(args[0]);
                            }
                            else
                            {
                                cli();
                            }
                            break;
                    }
                }
                catch (Exception err)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Clear();
                    Console.WriteLine("ERROR CAUGHT!" + err);
                }
                Console.WriteLine("Execution ended. Press any key to close CatoScript...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
        }
    }
}
