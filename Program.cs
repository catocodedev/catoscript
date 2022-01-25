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
                Console.WriteLine(key + " updated as " + kits[key]);
            }
            else
            {
                kits.Add(key, value);
                Console.WriteLine(key + " Set as " + kits[key]);
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
        static void catoexception(string type, string info, string op, int opnum, int errornum)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine(type +"Execption: "+ info + " | " + op + "(Operation:"+ opnum +")");
            Console.WriteLine("ERROR CODE : " + errornum);
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            System.Environment.Exit(errornum);

        }
        static void start()
        {
            Console.WriteLine("Starting main.cato ...");
            Console.WriteLine("================================================================");
            RunFile("main.cato");
        }
        static void Run(string run)
        {
            string tmp = "tmp";
            string topop = "";
            string subop = "";
            string perams = "";
            int opnum = 0;
            string[] subs = run.Split(';');
            foreach (string sub in subs)
            {
                opnum++;
                if (sub.StartsWith("%"))
                {
                    topop = "%";
                    subop = null;
                    perams = null;
                }
                else {
                    try
                    {
                        var pieces = sub.Split(new[] { '.' }, 2);
                        topop = pieces[0];
                        subop = pieces[1].GetUntilOrEmpty(" ");
                    }
                    catch (Exception) {
                        catoexception("OperationParseFail", "Run Parser could not parse the Operation to run! Please check the operation and refer to docs.", sub, opnum, 600);

                    }
                    if (sub.EndsWith("#"))
                {
                    perams = "#";
                }
                else
                {
                        try
                        {
                            var pies = sub.Split(new[] { '|' }, 2);
                            perams = pies[1].GetUntilOrEmpty("|");
                        }
                        catch (Exception)
                        {
                            catoexception("PeramParseFail", "Run Parser could not parse perams to run! Please check the operation and refer to docs.", perams, opnum, 600);
                        }
                    }
            }
                // Console.WriteLine(topop + " | " + subop + " | " + perams);
                Execute(sub, topop, subop, perams, opnum);
            }
        }
        public static string GetUntilOrEmpty(this string text, string stopAt)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
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
                        RunFile(tmp1);
                        Console.WriteLine("Cato File finnished running. Press any key to retrun to CLI");
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.WriteLine("-------- CatoScript " + CatoData.version + " ----------");
                        Console.WriteLine("Cato CLI ready!");
                        break;

                    case "ver":
                    case "version":
                        Console.WriteLine(CatoData.version);
                        break;

                    case "pur":
                        pur();
                        input = "";
                        break;

                    case "eval":
                        Console.Clear();
                        string eval = Console.ReadLine();
                        Run(eval);
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
                    case "start":
                        start();
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
                        if (!System.IO.File.Exists("main.cato"))
                        {
                            Console.WriteLine("Creating main cato file");
                            using (System.IO.FileStream fs = System.IO.File.Create("main.cato"))
                            {
                            
                            }
                        }
                        else
                        {
                            Console.WriteLine("main file already exists!");
                        }
                        Console.WriteLine("INIT Done!");
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
        static void Execute(string op, string topop, string subop, string perams, int opnum)
        {
            var kit = new Kits();
            string parsed = null;
            switch (topop)
            {
                case "%":
                    // nothing because comment
                    break;
                case "console":
                    if (!perams.EndsWith("#"))
                    {
                        try
                    {
                            parsed = perams.Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                        }catch (Exception ex)
                    {
                        catoexception("PeramParseFail", "Console Parser could not parse |" + perams + "| to run!", op, opnum, 600);
                    }
                    }
                    else
                    {
                        parsed = "#";
                    }
                    switch (subop)
                    {
                        case "send":
                            if (parsed != String.Empty)
                            {
                                Console.WriteLine(parsed);
                            }
                            else
                            {
                                catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nconsole.send can not send an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "clean":
                            Console.Clear();
                            break;
                        case "overwrite.up":
                            if (parsed != String.Empty)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop - 1);
                                Console.WriteLine(parsed);
                            }
                            else
                            {
                                catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nconsole.send can not send an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "object.load%":
                            string text = "";
                            int delay = 0;
                            if (parsed != String.Empty)
                            {
                                text = parsed;
                            }
                            else
                            {
                                catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nconsole.object can not send an empty string.\"", op, opnum, 101);
                            }
                            try
                            {
                                delay = Int32.Parse(perams);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", perams + " is invalid", op, opnum, 102);
                            }
                            Console.WriteLine("");
                            for (int i = 0; i < 101; i++)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop - 1);
                                Console.WriteLine(text + i + "%");
                                Thread.Sleep(delay);
                            }
                            break;
                        case "set.back.color":
                            if (parsed != String.Empty)
                            {
                                string backcolor = parsed;
                                if (Enum.TryParse(backcolor, out ConsoleColor background))
                                {
                                    Console.BackgroundColor = background;
                                }
                                else
                                {
                                    catoexception("Invaild Option", "Option for console color was not vaild! \nColor can not be set to " + perams + "", op, opnum, 400);
                                }
                            }
                            else
                            {
                                catoexception("NullReference", "Object reference was not set to an instance of an object. \nconsole.send can not send an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "set.text.color":
                            {
                                if (parsed != String.Empty)
                                {
                                    string textcolor = parsed;
                                    if (Enum.TryParse(textcolor, out ConsoleColor textc))
                                    {
                                        Console.ForegroundColor = textc;
                                    }
                                    else
                                    {
                                        catoexception("Invaild Option", "Option for console color was not vaild! \nColor can not be set to " + perams + "", op, opnum, 400);
                                    }
                                }
                                else
                                {
                                    catoexception("NullReference", "\"Object reference was not set to an instance of an object. \nconsole.send can not send an empty string.\"", op, opnum, 101);
                                }

                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + " Is not a vaild SubOperation of console", op, opnum, 104);
                            break;
                    }
                    break;
                case "debug":
                    if (!perams.EndsWith("#"))
                    {
                        try
                        {
                            parsed = perams.Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                        }
                        catch (Exception ex)
                        {
                            catoexception("PeramParseFail", "Console Parser could not parse |" + perams + "| to run!", op, opnum, 600);
                        }
                    }
                    else
                    {
                        parsed = "#";
                    }
                    switch (subop)
                    {
                        case "throw":
                            catoexception("UserGenerated", parsed, op, opnum, 200);
                            break;
                        case "get.OS":
                            Console.WriteLine(RuntimeInformation.OSDescription + "|" + RuntimeInformation.OSArchitecture);
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + " Is not a vaild SubOperation of debug", op, opnum, 104);
                            break;
                    }
                    break;
                case "random":
                    switch (subop)
                    {
                        case "num":
                            Random engine = new();
                            //this should grab |min:max|
                            try
                            {
                                int min = Int32.Parse(perams);
                                int max = Int32.Parse(perams);
                                int value = engine.Next(min, max);
                                Console.WriteLine(value);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", perams + " are invalid", op, opnum, 102);
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of random", op, opnum, 104);
                            break;
                    }
                    break;
                case "math":
                    // add math peram parser
                    switch (subop)
                    {
                        case "add":
                            try
                            {
                                int num1 = Int32.Parse(perams);
                                int num2 = Int32.Parse(perams);
                                Console.WriteLine(num1 + num2);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", perams + " are invalid", op, opnum, 102);
                            }
                            break;
                        case "sub":
                            try
                            {
                                int num1 = Int32.Parse(perams);
                                int num2 = Int32.Parse(perams);
                                Console.WriteLine(num1 - num2);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", perams + " are invalid", op, opnum, 102);
                            }
                            break;
                        case "multi":
                            try
                            {
                                int num1 = Int32.Parse(perams);
                                int num2 = Int32.Parse(perams);
                                Console.WriteLine(num1 * num2);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", perams + " are invalid", op, opnum, 102);
                            }
                            break;
                        case "divide":
                            try
                            {
                                int num1 = Int32.Parse(perams);
                                int num2 = Int32.Parse(perams);
                                Console.WriteLine(num1 / num2);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", perams + " are invalid", op, opnum, 102);
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of math", op, opnum, 104);
                            break;
                    }
                    break;
                case "script":
                    // dosen't need parser rn
                    switch (subop)
                    {
                        case "pause.time":
                            try
                            {
                                Thread.Sleep(Int32.Parse(perams));
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", perams + " is invalid", op, opnum, 102);
                            }
                            break;
                        case "pause.keywait":
                            Console.WriteLine("Script Paused! until user presses a key");
                            Console.ReadKey();
                            break;
                        case "quit":
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            System.Environment.Exit(201);
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of script", op, opnum, 104);
                            break;
                    }
                    break;
                case "@kit":
                    //@kit parser
                    try
                    {
                        kit.Set(perams, perams);
                    }
                    catch (Exception)
                    {
                        catoexception("InvaildKitDelcare", "The kit could not be delacared!", op, opnum, 300);
                    }
                    break;
                case "kit":
                    //kit parser
                    switch (subop)
                    {
                        case "set":
                            if (subop.Contains("frominput"))
                            {
                                string value = Console.ReadLine();
                                try
                                {
                                    kit.Set(perams, value);
                                }
                                catch (Exception)
                                {
                                    catoexception("InvaildKit", "The kit " + perams + " wasn't found!", op, opnum, 301);
                                }
                            }
                            else
                            {
                                try
                                {
                                    kit.Set(perams, perams);
                                }
                                catch (Exception)
                                {
                                    catoexception("InvaildKit", "The kit " + perams + " wasn't found!", op, opnum, 301);
                                }
                            }
                            break;
                    }
                    break;
                case "if":
                    //to be made
                    break;
            default:
                    catoexception("InvalidOperation", "\"This Operation was not recognized.\"", op, opnum, 100);
            break;
        }
    }
        static void RunFile(string fileName)
        {
            if (fileName != null)
            {
                string fileExt = System.IO.Path.GetExtension(fileName);
                if (fileExt == ".cato")
                {
                    if (File.Exists(fileName))
                    {
                        string[] readText = File.ReadAllLines(fileName);
                        string whole = "";
                        readText.Each((str, n) =>
                        {
                            whole = whole + str; 
                        });
                        Run(whole);
                    }
                    else
                    {
                        Console.WriteLine("File doesn't exist.");
                        System.Environment.Exit(404);
                    }
                }
                else
                {
                    Console.WriteLine("File not a catoscript file");
                    System.Environment.Exit(405);
                }
            }
            else
            {
                Console.WriteLine("Please write a file name.");
                System.Environment.Exit(101);
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
                        case "version":
                            Console.WriteLine(CatoData.version);
                            System.Environment.Exit(0);
                            break;

                        case "pur":
                            if (args[1] != null)
                            {
                                // add code to run pur cmds
                            }
                            else
                            {
                                pur();
                            }
                            break;

                        case "":
                            cli();
                            break;
                        case "start":
                            start();
                            break;

                        case "help":
                            Console.WriteLine("----------CatoScript Help-------------------");
                            Console.WriteLine("<anything>.cato - Runs the file");
                            Console.WriteLine("run <anything>.cato - Runs the file");
                            Console.WriteLine("version/ver - shows version of catoscript");
                            Console.WriteLine("pur - open PUR CLI");
                            Console.WriteLine("eval - test a line of code");
                            Console.WriteLine("--------------------------------------------");
                            System.Environment.Exit(0);
                            break;

                        case "eval":
                            Console.Clear();
                            string eval = Console.ReadLine();
                            Run(eval);
                            Console.WriteLine("==========EVAL RAN!========");
                            System.Environment.Exit(0);
                            break;
                        case "run":
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            RunFile(args[1]);
                            Console.WriteLine("Cato File finnished running. Press any key to retrun to CLI");
                            Console.ReadKey();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("-------- CatoScript " + CatoData.version + " ----------");
                            Console.WriteLine("Cato CLI ready!");
                            break;
                        default:
                            if (args[0].Contains(".cato"))
                            {
                                RunFile(args[0]);
                            }
                            else
                            {
                            Console.Clear();
                            Console.WriteLine("Command "+ args[0] + " not found please use `cato help`");
                            System.Environment.Exit(0);
                            break;
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
                Console.WriteLine("Execution ended! Press any key to close CatoScript...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
        }
    }
}
