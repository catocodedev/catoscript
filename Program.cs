using System.Runtime.InteropServices;


namespace cato
{
    public class CatoData
    {
        public static string version = "Dev0.1.3";
        public static string purver = "Dev0.1.0";
        public string OS = "null";
    }
    public class Kits
    {
        private Dictionary<string, string> kits = new Dictionary<string, string>();
        public void Set(string key, string value)
        {
            if (kits.ContainsKey(key))
            {
                kits[key] = value;
            }
            else
            {
                kits.Add(key, value);
            }
        }
        public string Get(string key)
        {
            string result = null;

            if (kits.ContainsKey(key))
            {
                result = kits[key];
            }

            return result;
        }
        public void Update(string key, string value)
        {
            kits[key] = value;
        }
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
        static void catoexception(string type, string info, string line, int linenum, int errornum)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine(type +"Execption: "+ info + " | " + line + "(line:"+ linenum +")");
            Console.WriteLine("ERROR CODE : " + errornum);
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
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
                Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
                { 
                }
                    static void myHandler(object sender, ConsoleCancelEventArgs args)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    args.Cancel = true;
                    System.Environment.Exit(0);
                }
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
                    case "eval":
                        Console.Clear();
                        string test = Console.ReadLine(); ;
                        Execute(test, 0);
                        Console.WriteLine("==========EVAL RAN PRESS ANY KEY TO RETURN TO CLI========");
                        Console.ReadKey();
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("-------- CatoScript " + CatoData.version + " ----------");
                        break;
                    case "clean":
                        Console.Clear();
                        Console.WriteLine("-------- CatoScript " + CatoData.version + " ----------");
                        break;
                    case "help":
                        Console.WriteLine("----------CatoScript Help-------------------");
                        Console.WriteLine("run - Runs a .cato file");
                        Console.WriteLine("version/ver - shows version of catoscript");
                        Console.WriteLine("exit - leave the CLI");
                        Console.WriteLine("pur - open PUR CLI");
                        Console.WriteLine("eval - test a line of code");
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
                Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
                {
                }
                static void myHandler(object sender, ConsoleCancelEventArgs args)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    args.Cancel = true;
                    System.Environment.Exit(0);
                }
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
                    case "init":
                    // add code to make folders and files
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
        static void Execute(string line, int linenumber)
        {
            var kit = new Kits();
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
            else if (line.StartsWith("console.clean#"))
            {
                Console.Clear();
            }
            else if (line.StartsWith("console.overwrite.up "))
            {
                if (getBetween(line, "|\"", "\"|") != String.Empty)
                {
                    Console.SetCursorPosition(0, Console.CursorTop -1);
                    Console.WriteLine(getBetween(line, "|\"", "\"|"));
                }
                else
                {
                    catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nconsole.send can not send an empty string.\"", line, linenumber, 101);
                }
            }
            else if (line.StartsWith("console.object.load% "))
            {
                string text = "";
                int delay = 0;
                if (getBetween(line, "|\"", "\"") != String.Empty)
                {
                    text = getBetween(line, "|\"", "\"");
                }
                else
                {
                    catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nconsole.object can not send an empty string.\"", line, linenumber, 101);
                }
                try
                {
                    delay = Int32.Parse(getBetween(line, ",", "|"));
                }
                catch (FormatException)
                {
                    catoexception("InvalidInput", getBetween(line, ",", "|") + " is invalid", line, linenumber, 102);
                }
                Console.WriteLine("");
                for (int i = 0; i < 101; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop -1);
                    Console.WriteLine(text + i +"%");
                    Thread.Sleep(delay);
                }
            }
            else if (line.StartsWith("console.set.back.color "))
            {
                if (getBetween(line, "|\"", "\"|") != String.Empty)
                {
                    string backcolor = getBetween(line, "|\"", "\"|");
                    if (Enum.TryParse(backcolor, out ConsoleColor background))
                    {
                        Console.BackgroundColor = background;
                    }
                    else
                    {
                        catoexception("Invaild Option", "Option for console color was not vaild! \nColor can not be set to "+getBetween(line, "|\"", "\"|")+"", line, linenumber, 400);
                    }
                }
                else
                {
                    catoexception("NullReference", "Object reference was not set to an instance of an object. \nconsole.send can not send an empty string.\"", line, linenumber, 101);
                }

            }
            else if (line.StartsWith("console.set.text.color "))
            {
                if (getBetween(line, "|\"", "\"|") != String.Empty)
                {
                    string textcolor = getBetween(line, "|\"", "\"|");
                    if (Enum.TryParse(textcolor, out ConsoleColor textc))
                    {
                        Console.ForegroundColor = textc;
                    }
                    else
                    {
                        catoexception("Invaild Option", "Option for console color was not vaild! \nColor can not be set to "+getBetween(line, "|\"", "\"|")+"", line, linenumber, 400);
                    }
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
            else if (line.StartsWith("math.add "))
            {
                try
                {
                    int num1 = Int32.Parse(getBetween(line, "|", "+"));
                    int num2 = Int32.Parse(getBetween(line, "+", "|"));
                    Console.WriteLine(num1 + num2);
                }
                catch (FormatException)
                {
                    catoexception("InvalidInput", getBetween(line, "|", "+") + " & " + getBetween(line, "+", "|") + " are invalid", line, linenumber, 102);
                }
            }
            else if (line.StartsWith("math.sub "))
            {
                try
                {
                    int num1 = Int32.Parse(getBetween(line, "|", "-"));
                    int num2 = Int32.Parse(getBetween(line, "-", "|"));
                    Console.WriteLine(num1 - num2);
                }
                catch (FormatException)
                {
                    catoexception("InvalidInput", getBetween(line, "|", "-") + " & " + getBetween(line, "-", "|") + " are invalid", line, linenumber, 102);
                }
            }
            else if (line.StartsWith("math.multi "))
            {
                try
                {
                    int num1 = Int32.Parse(getBetween(line, "|", "*"));
                    int num2 = Int32.Parse(getBetween(line, "*", "|"));
                    Console.WriteLine(num1 * num2);
                }
                catch (FormatException)
                {
                    catoexception("InvalidInput", getBetween(line, "|", "*") + " & " + getBetween(line, "*", "|") + " are invalid", line, linenumber, 102);
                }
            }
            else if (line.StartsWith("math.divide "))
            {
                try
                {
                    int num1 = Int32.Parse(getBetween(line, "|", "/"));
                    int num2 = Int32.Parse(getBetween(line, "/", "|"));
                    Console.WriteLine(num1 / num2);
                }
                catch (FormatException)
                {
                    catoexception("InvalidInput", getBetween(line, "|", "/") + " & " + getBetween(line, "/", "|") + " are invalid", line, linenumber, 102);
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
            else if (line.StartsWith("script.pause.keywait#"))
            {
                Console.WriteLine("Script Paused! untill user presses a key");
                Console.ReadKey();
            }
            else if (line.StartsWith("script.quit#"))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                System.Environment.Exit(0);
            }
            else if (line.StartsWith("get.OS#"))
            {
                Console.WriteLine("The User OS is " + RuntimeInformation.OSDescription + "|" + RuntimeInformation.OSArchitecture);
            }
            else if (line.StartsWith("@kit "))
            {
                try
                {
                    kit.Set(getBetween(line, "{", ","), getBetween(line, ",\"", "\"}"));
                    Console.WriteLine(getBetween(line, "{", ",") + " | " + getBetween(line, ",\"", "\"}"));
                }
                catch (Exception)
                {
                    catoexception("InvaildKitDelcare", "The kit could not be delacared!", line, linenumber, 300);
                }
                kit.Set(getBetween(line, "{\"", "\","), getBetween(line, ",\"", "\"}"));
            }
            else if (line.StartsWith("console.send.kit "))
            {
                if (kit.Get(getBetween(line, "|", "|")) != String.Empty)
                {
                    Console.WriteLine(kit.Get(getBetween(line, "|", "|")));
                }
                else
                {
                    catoexception("InvaildKit", "The kit "+ getBetween(line, "|", "|") +" wasn't found!", line, linenumber, 301);
                }
            }
            else if (line.StartsWith("kit.set "))
            {
                try
                {
                    kit.Update(getBetween(line, "|", ","),getBetween(line, ",\"", "\"|"));
                }
                catch (Exception)
                {
                    catoexception("InvaildKit", "The kit "+ getBetween(line, "|", ",") +" wasn't found!", line, linenumber, 301);
                }
                kit.Update(getBetween(line, "|", ","), getBetween(line, ",\"", "\"|"));
            }
            else if (line.StartsWith("kit.set.from.input "))
            {
                string value = Console.ReadLine();
                try
                {
                    kit.Update(getBetween(line, "|", "|"), value);
                }
                catch (Exception)
                {
                    catoexception("InvaildKit", "The kit "+ getBetween(line, "|", "|") +" wasn't found!", line, linenumber, 301);
                }
                kit.Update(getBetween(line, "|", "|"), value);
            }
            else if (line.StartsWith("if "))
            {
                string test1 = getBetween(line, "|\"", "\" ->");
                string test2 = getBetween(line, "-> \"", "\"|");
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
                            Execute(str, n+1);
                        });

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
                // static readonly HttpClient Client = new HttpClient();
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
                            Console.ReadKey();
                            System.Environment.Exit(0);
                            break;

                        case "version":
                            Console.WriteLine(CatoData.version);
                            Console.ReadKey();
                            System.Environment.Exit(0);
                            break;

                        case "pur":
                            pur();
                            break;

                        case "":
                            cli();
                            break;
                        case "help":
                            Console.WriteLine("----------CatoScript Help-------------------");
                            Console.WriteLine("run - Runs a .cato file");
                            Console.WriteLine("version/ver - shows version of catoscript");
                            Console.WriteLine("pur - open PUR CLI");
                            Console.WriteLine("eval - test a line of code");
                            Console.WriteLine("--------------------------------------------");
                            Console.ReadKey();
                            System.Environment.Exit(0);
                            break;
                        case "eval":
                            Console.Clear();
                            string test = Console.ReadLine(); ;
                            Execute(test, 0);
                            Console.WriteLine("==========EVAL RAN PRESS ANY KEY TO QUIT========");
                            Console.ReadKey();
                            System.Environment.Exit(0);
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
                Console.Clear();
                Console.WriteLine("Execution ended! Press any key to close CatoScript...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
        }
    }
}
