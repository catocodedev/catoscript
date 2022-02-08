using System.Net;
using System.Runtime.InteropServices;
using Pastel;

namespace cato
{
    public class CatoData
    {
        public static string version = "Dev0.1.4";
        public static string purver = "Dev0.1.1";
        public string OS = "null";
        public string debug = "False";       
    }
    public class Kits
    {
        private static Dictionary<string, string> kits = new Dictionary<string, string>();
        public static void Set(string key, string value)
        {
            if (kits.ContainsKey(key))
            {
                kits[key] = value;            }
            else
            {
                kits.Add(key, value);            }
        }
        public static string Get(string key)
        {
            string result = null;

            if (kits.ContainsKey(key))
            {
                result = kits[key];
            }

            return result;
        }
        public static void Remove(string key)
        {
            kits.Remove(key);
        }
        public static void Clear()
        {
            kits.Clear();
        }
    }
    internal static class Cato
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
            if (File.Exists("debug.catlog"))
            {
                delog(type + "Execption: " + info, opnum, errornum);
            }
                Console.WriteLine(type +"Execption: "+ info + " | " + op + "(Operation:"+ opnum +")");
            Console.WriteLine("ERROR CODE : " + errornum);
            Console.WriteLine("more info https://github.com/catoscript/cato/wiki/Errors#" + errornum);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

        }
        static void delog(string text, int linenum,int errnum)
        {
            if (File.Exists("debug.catlog"))
            {
                try
                {
                    using StreamWriter filee = new("debug.catlog", append: true);
                    if (errnum == 0)
                    {
                        filee.WriteLine(text);
                    }
                    else
                    {
                        filee.WriteLine("line("+ linenum +") | "+ text + " | " + errnum);
                    }
                }
                catch (Exception ex)
                {
                    catoexception("FileWriteError", "debug.catlog" + " could not be written too! details: " + ex, "writing to debug log", linenum, 407);
                }
            }
            else
            {
                catoexception("FileNotFound", "debug.catlog" + " could not be found!", "writing to debug log", linenum, 404);
            }
        }
        static void start()
        {
            string runner = "cmd";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                runner = "bash";
            }

            Console.WriteLine("Starting main.cato ...");
            try
            {

                    // the /c will quit
                    System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(runner, "/c " + "cato main.cato");
                procStartInfo.RedirectStandardOutput = false;
                procStartInfo.UseShellExecute = true;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception objException)
            {
                catoexception("SystemRunError", objException.ToString(), "cato spawn", 0, 700);
            }
            Console.WriteLine("Project Ran!");
        }
        static void Run(string run)
        {
            Kits.Clear();
            string tmp = "tmp";
            string topop = "";
            string subop = "";
            string perams = "";
            string subo = "";
            int opnum = 0;
            string[] subs = run.Split(';');
            foreach (string sub in subs)
            {
                subo = sub.Trim();
                opnum++;
                if (subo.StartsWith("%") || subo == String.Empty)
                { 
                    topop = "%";
                    subop = "%";
                    perams = "%";
                }
                else {
                    try
                    {
                        var pieces = subo.Split(new[] { '.' }, 2);
                        topop = pieces[0];
                        subop = pieces[1].GetUntilOrEmpty(" ");
                    }
                    catch (Exception ex) {
                        catoexception("OperationParseFail", "Run Parser could not parse the Operation to run! Please check the operation and refer to docs.", subo, opnum, 600);
                        Console.WriteLine(ex);
                    }
                    if (subo.EndsWith("#"))
                {
                    perams = "#";
                }
                else
                {
                        try
                        {
                            var pies = subo.Split(new[] { '|' }, 2);
                            perams = pies[1].GetUntilOrEmpty("|");
                        }
                        catch (Exception)
                        {
                            catoexception("PeramParseFail", "Run Parser could not parse perams to run! Please check the operation and refer to docs.", perams, opnum, 601);
                        }
                    }
            }
                // Console.WriteLine(topop + " | " + subop + " | " + perams);
                Execute(subo, topop, subop, perams, opnum);
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
        public static void spawn()
        {
            try
            {
                string runner = "cmd";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    runner = "bash";
                }
                // the /c will quit
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(runner, "/c " + "cato")
                    {
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        // Do not create the black window.
                        CreateNoWindow = true
                    };
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception objException)
            {
                catoexception("SystemRunError", objException.ToString(), "cato spawn", 0, 700);
            }
            Console.WriteLine("Cato despawned!");
        }
        public static void arun()
        {
            string run = "run";
            string stat = "run";
            try
            {
                RunFile("main.cato");
            }
            catch (Exception)
            {
                Console.WriteLine("main.cato failed to run!");
            }
            using var watcher = new FileSystemWatcher(Directory.GetCurrentDirectory());
            watcher.Filter = "main.cato";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += OnChanged;
                static void OnChanged(object sender, FileSystemEventArgs e)
                {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                if (e.ChangeType != WatcherChangeTypes.Changed)
                    {
                        return;
                    }
                    Thread.Sleep(400);
                try
                {
                    RunFile("main.cato");
                }
                catch(Exception)
                {
                    Console.WriteLine("main.cato failed to run!");
                    Thread.Sleep(4000);
                }
                }
            while (run == "run")
            {
            
            }
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
                        try
                        {
                            RunFile(tmp1);
                        }catch (Exception ex)
                        {
                            Console.WriteLine("Failed to run file " + tmp1);
                        }
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
                        Console.WriteLine("eval - test code");
                        Console.WriteLine("start - start your catoscript project");
                        Console.WriteLine("activate - activate cato live run");
                        Console.WriteLine("--------------------------------------------");
                        break;
                    case "start":
                        start();
                        break;
                    case "spawn":
                        spawn();
                        break;
                    case "activate":
                        arun();
                        break;
                    case "info":
                        Console.WriteLine("Catoscript - " + CatoData.version);
                        Console.WriteLine("Pur - " + CatoData.purver);
                        Console.WriteLine("Install Dir - " + Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
                        Console.WriteLine("Github - https://github.com/catocodedev/catoscript");
                        Console.WriteLine("Running OS - " + RuntimeInformation.OSDescription + "|" + RuntimeInformation.OSArchitecture);
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
                        Console.WriteLine("init - setup a CatoScript Project");
                        break;
                    case "init":
                        if (!System.IO.File.Exists("main.cato"))
                        {
                            Console.WriteLine("Creating main cato file");
                            using (System.IO.FileStream fs = System.IO.File.Create("main.cato"))
                            {

                            }
                            using StreamWriter filee = new("main.cato", append: true);
                            string tmp = "console.send |" + '"' + "Hello, cato" + '"' + "|;";
                            filee.WriteLine(tmp);
                        }
                        else
                        {
                            Console.WriteLine("main file already exists!");
                        }
                        if (!System.IO.File.Exists("debug.catlog"))
                        {
                            Console.WriteLine("Creating debug log file");
                            using (System.IO.FileStream fs = System.IO.File.Create("debug.catlog"))
                            {

                            }
                            using StreamWriter filee = new("debug.catlog", append: true);
                            filee.WriteLine("Project inited with pur " + CatoData.purver);
                            filee.WriteLine("Project made in CatoScript " + CatoData.version);
                        }
                        else
                        {
                            Console.WriteLine("debug log already exists!");
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
        public static void Execute(string op, string topop, string subop, string perams, int opnum)
        {
            int peramnum = 0;
            // generic parser
            var parsed = perams.Split(new[] { ',' });
            if (!perams.Contains("("))
            {
                try
                {
                    parsed = perams.Split(new[] { ',' });

                }
                catch (Exception ex)
                {
                    catoexception("PeramParseFail", "Console Parser could not parse |" + perams + "| to run!", op, opnum, 611);
                }
            }
            if (perams.StartsWith("#"))
            {
                parsed[0] = "#";
            }
            foreach (string s in parsed)
            {
                peramnum++;
            }
            switch (topop)
            {
                case "%":
                    // nothing because comment
                    break;
                case "console":
                    var text = "";
                    if (parsed[0].StartsWith('"'))
                    {
                        try
                        {
                            text = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                        }
                        catch (Exception ex)
                        {
                            catoexception("PeramParseFail", "Console Parser could not parse |" + perams + "| to run!", op, opnum, 611);
                        }
                    }
                    switch (subop)
                    {
                        case "send":
                            if (text != String.Empty)
                            {
                                Console.WriteLine(text);
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \nconsole.send can not send an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "send.color":
                            string color = "";
                            try
                            {
                                color = parsed[1].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                                int num = Int32.Parse(color, System.Globalization.NumberStyles.HexNumber);
                            }
                            catch (Exception ex)
                            {
                                catoexception("PeramParseFail", "Console Parser could not parse |" + perams + "| to run!", op, opnum, 611);
                            }
                            if (text != String.Empty && color != String.Empty)
                            {
                                Console.WriteLine(text.Pastel("#" + color));
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \nconsole.send can not send an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "clean":
                            Console.Clear();
                            break;
                        case "overwrite":
                            int pos = 1;
                            if (peramnum == 2)
                            {
                                try
                                {
                                    pos = Int32.Parse(parsed[1]);
                                } catch (Exception ex)
                                {
                                    catoexception("Invaild Option", parsed[1] + " Option for uppos was not vaild", op, opnum, 401);
                                }
                            }
                            if (text != String.Empty)
                            {
                                try
                                {
                                    Console.SetCursorPosition(0, Console.CursorTop - pos);
                                    Console.WriteLine(text);
                                    Console.SetCursorPosition(0, Console.CursorTop + pos - 1);
                                } catch (Exception ex)
                                {
                                    catoexception("Invaild Option", "Option for pos was not vaild! \npos can not be out of range!", op, opnum, 401);
                                }
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \noverwrite.up can not overwrite with an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "cursor.pos.y":
                            pos = 1;
                            try
                            {
                                pos = Int32.Parse(parsed[0]);
                            }
                            catch (Exception ex)
                            {
                                catoexception("Invaild Option", parsed[0] + " Option for uppos was not vaild", op, opnum, 401);
                            }
                            try
                            {
                                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + pos);
                            } catch (Exception ex)
                            {
                                catoexception("Invaild Option", "Option for pos was not vaild! \npos can not be out of range!", op, opnum, 401);
                            }
                            break;
                        case "cursor.pos.x":
                            pos = 1;
                            try
                            {
                                pos = Int32.Parse(parsed[0]);
                            }
                            catch (Exception ex)
                            {
                                catoexception("Invaild Option", parsed[0] + " Option for uppos was not vaild", op, opnum, 401);
                            }
                            try
                            {
                                Console.SetCursorPosition(Console.CursorLeft + pos, Console.CursorTop);
                            } catch (Exception ex)
                            {
                                catoexception("Invaild Option", "Option for pos was not vaild! \npos can not be out of range!", op, opnum, 401);
                            }
                            break;
                        case "cursor.size":
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            {
                                pos = 1;
                                try
                                {
                                    pos = Int32.Parse(parsed[0]);
                                }
                                catch (Exception ex)
                                {
                                    catoexception("Invaild Option", parsed[0] + " Option for uppos was not vaild", op, opnum, 401);
                                }
                                Console.CursorSize = pos;
                            }
                            else
                            {
                                catoexception("Invaild Platform", "cursor.size is not supported on your platform!", op, opnum, 900);
                            }
                            break;
                        case "cursor.vis":
                            bool vis = true;
                            try
                            {
                                vis = bool.Parse(parsed[0]);
                            }
                            catch (Exception ex)
                            {
                                catoexception("Invaild Option", parsed[0] + " Option for uppos was not vaild", op, opnum, 401);
                            }
                            Console.CursorVisible = vis;
                            break;
                        case "object.load":
                            // object parser
                            var meowobject = perams.Split(new string[] { "(", ")" }, 3, StringSplitOptions.None)[1];
                            string[] subs = meowobject.Split(',');
                            string text1 = "";
                            string text2 = "";
                            try
                            {
                                text1 = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                                text2 = parsed[1].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            }
                            catch (Exception)
                            {
                                catoexception("ObjectInputError", "no \"\" found in text properties! however a invaild value was passed", op, opnum, 801);
                            }
                            int objnum = 0;
                            foreach (string s in subs)
                            {
                                parsed[objnum] = s;
                                objnum++;
                            }
                            if (objnum > 2 && objnum < 5) {
                                int delay = 0;
                                int amount = 101;
                                if (parsed[0] != String.Empty && parsed[1] != String.Empty && parsed[2] != String.Empty)
                                {
                                    try
                                    {
                                        delay = Int32.Parse(parsed[2]);
                                        if (objnum == 4)
                                        {
                                            if (parsed[3] != String.Empty) {
                                                try
                                                {
                                                    amount = Int32.Parse(parsed[3]) + 1;
                                                }
                                                catch (FormatException)
                                                {
                                                    catoexception("ObjectInputError", "property AMOUNT should be int! however a invaild value was passed", op, opnum, 801);
                                                }
                                            }

                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        catoexception("ObjectInputError", "property DELAY should be int! however a invaild value was passed", op, opnum, 801);
                                    }
                                    Console.WriteLine("");
                                    for (int i = 0; i < amount; i++)
                                    {
                                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                                        Console.WriteLine(text1 + i + text2);
                                        Thread.Sleep(delay);
                                    }
                                }
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \nobject.load can not have an empty string.\"", op, opnum, 101);
                            }

                            break;
                        case "set.back.color":
                            if (text != String.Empty)
                            {
                                string backcolor = text;
                                if (Enum.TryParse(backcolor, out ConsoleColor background))
                                {
                                    Console.BackgroundColor = background;
                                }
                                else
                                {
                                    catoexception("Invaild Option", "Option for console color was not vaild! \nColor can not be set to " + perams + "", op, opnum, 401);
                                }
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \ntext.color can not be an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "set.text.color":
                            {
                                if (text != String.Empty)
                                {
                                    if (Enum.TryParse(text, out ConsoleColor textc))
                                    {
                                        Console.ForegroundColor = textc;
                                    }
                                    else
                                    {
                                        catoexception("Invaild Option", "Option for console color was not vaild! \nColor can not be set to " + perams + "", op, opnum, 401);
                                    }
                                }
                                else
                                {
                                    catoexception("NullPeram", "\"Peram was not set to non-null value \ntext.color can not be an empty string.\"", op, opnum, 101);
                                }

                            }
                            break;
                        case "wait.key":
                            if (text != String.Empty)
                            {
                                Console.WriteLine(text);
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \nwait.key can not send an empty string.\"", op, opnum, 101);
                            }
                            Console.ReadKey();
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + " Is not a vaild SubOperation of console", op, opnum, 104);
                            break;
                    }
                    break;
                case "debug":

                    switch (subop)
                    {
                        case "throw":
                            text = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            catoexception("UserGenerated", text, op, opnum, 200);
                            break;
                        case "get.OS":
                            Console.WriteLine(RuntimeInformation.OSDescription + "|" + RuntimeInformation.OSArchitecture);
                            break;
                        case "get.dir":
                            Console.WriteLine(Directory.GetCurrentDirectory());
                            break;
                        case "log":
                            text = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            delog(text, opnum, 0);
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
                                int min = Int32.Parse(parsed[0]);
                                int max = Int32.Parse(parsed[1]);
                                int value = engine.Next(min, max);
                                Console.WriteLine(value);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", parsed[0] + "|" + parsed[1] + " are invalid", op, opnum, 102);
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of random", op, opnum, 104);
                            break;
                    }
                    break;
                case "math":
                    switch (subop)
                    {
                        case "add":
                            try
                            {
                                int num1 = Int32.Parse(parsed[0]);
                                int num2 = Int32.Parse(parsed[1]);
                                Console.WriteLine(num1 + num2);
                            }
                            catch (FormatException)
                            {
                                catoexception("InvalidInput", parsed[0] + "|" + parsed[1] + " are invalid", op, opnum, 102);
                            }
                            break;
                        case "sub":
                            try
                            {
                                int num1 = Int32.Parse(parsed[0]);
                                int num2 = Int32.Parse(parsed[1]);
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
                                int num1 = Int32.Parse(parsed[0]);
                                int num2 = Int32.Parse(parsed[1]);
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
                                int num1 = Int32.Parse(parsed[0]);
                                int num2 = Int32.Parse(parsed[1]);
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
                                Thread.Sleep(Int32.Parse(parsed[0]));
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
                        case "run":
                            string filee = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            try
                            {
                                RunFile(filee);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine(filee + "failed to run!");
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of script", op, opnum, 104);
                            break;
                    }
                    break;
                case "@kit":
                    //@kit parser
                    switch (subop)
                    {
                    case "make":
                    try
                    {
                        Kits.Set(parsed[0], parsed[1]);
                    }
                    catch (Exception)
                    {
                        catoexception("InvaildKitDelcare", "The kit could not be delacared!", op, opnum, 300);
                    }
                            break;
                        case "remove":
                            Kits.Remove(parsed[0]);
                    break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of @kit", op, opnum, 104);
                            break;

                    }
                    break;
                case "kit":
                    //kit parser
                    switch (subop)
                    {
                        case "set":
                                try
                                {
                                    Kits.Set(parsed[0],parsed[1]);
                                }
                                catch (Exception)
                                {
                                    catoexception("InvaildKit", "The kit " + parsed[0] + " wasn't found!", op, opnum, 301);
                                }
                            break;
                        case "get":
                            Console.WriteLine(Kits.Get(parsed[0]));
                            break;
                        case "set.from.input":
                            string value = Console.ReadLine();
                            try
                            {
                                Kits.Set(parsed[0], value);
                            }
                            catch (Exception)
                            {
                                catoexception("InvaildKit", "The kit " + parsed[0] + " wasn't found!", op, opnum, 301);
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of kit", op, opnum, 104);
                            break;
                    }

                    break;
                case "if":
                    //to be made
                    break;
                case "sys":
                    text = "tmp";
                    if (!perams.EndsWith("#"))
                    {
                        try
                        {
                            text = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                        }
                        catch (Exception ex)
                        {
                            catoexception("PeramParseFail", "Console Parser could not parse |" + perams + "| to run!", op, opnum, 600);
                        }
                    }
                    switch (subop)
                    {
                        case "run":
                            bool wait = true;
                            if (peramnum == 2)
                            {
                                try
                                {
                                    wait = Boolean.Parse(parsed[1]);
                                }
                                catch (Exception ex)
                                {
                                    catoexception("Invaild Option", parsed[1] + " Option for wait was not vaild", op, opnum, 401);
                                }
                            }
                            
                                try
                            {
                                string runner = "cmd";
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                                {
                                    runner = "bash";
                                }
                                // the /c will quit
                                System.Diagnostics.ProcessStartInfo procStartInfo =
                                    new System.Diagnostics.ProcessStartInfo(runner,"/c " + text);
                                procStartInfo.RedirectStandardOutput = false;
                                procStartInfo.UseShellExecute = true;
                                // Do not create the black window.
                                procStartInfo.CreateNoWindow = true;
                                // Now we create a process, assign its ProcessStartInfo and start it
                                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                                proc.StartInfo = procStartInfo;
                                proc.Start();
                                if (wait == true)
                                {
                                    proc.WaitForExit();
                                }
                                else
                                {
                                    proc.Close();
                                } 
                            }
                            catch (Exception objException)
                            {
                                catoexception("SystemRunError", objException.ToString(), op, opnum, 700);
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of sys", op, opnum, 104);
                            break;
                    }
                    break;
                case "file":
                    var file = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                    switch (subop)
                    {
                        case "dump":
                            if (File.Exists(file))
                            {
                                string[] readText = File.ReadAllLines(file);
                                string whole = "";
                                readText.Each((str, n) =>
                                {
                                    Console.WriteLine(str);
                                });
                            }
                            else
                            {
                                catoexception("FileNotFound", file + " could not be found!", op, opnum, 404);
                            }
                            break;
                        case "create":
                            if (!System.IO.File.Exists(file))
                            {
                                using (System.IO.FileStream fs = System.IO.File.Create(file))
                                {

                                }
                            }
                            else
                            {
                                catoexception("FileExsits", file + " already exists!", op, opnum, 406);
                            }
                            break;
                        case "remove":
                            File.Delete(file);
                            break;
                        case "write.append":
                            if (File.Exists(file))
                            {
                                text = parsed[1].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                                try
                                {
                                    using StreamWriter filee = new(file, append: true);
                                    filee.WriteLine(text);
                                }
                                catch (Exception ex)
                                {
                                    catoexception("FileWriteError", file + " could not be written too! details: " + ex, op, opnum, 407);
                                }
                            }
                            else
                            {
                                catoexception("FileNotFound", file + " could not be found!", op, opnum, 404);
                            }
                            break;
                        case "write.all":
                            if (File.Exists(file))
                            {
                                text = parsed[1].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                                try
                                {
                                    File.WriteAllText(file, text);
                                }
                                catch (Exception ex)
                                {
                                    catoexception("FileWriteError", file + " could not be written too! details: " + ex, op, opnum, 407);
                                }
                            }
                            else
                            {
                                catoexception("FileNotFound", file + " could not be found!", op, opnum, 404);
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of file", op, opnum, 104);
                            break;
                    }
                    break;
                        case "http":
                            string link = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            switch (subop)
                                {
                                    case "get":
                                    // add http req code
                                    break;
                        case "download":
                            string output = parsed[1].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            try
                            {
                                WebClient myWebClient = new WebClient();
                                myWebClient.DownloadFile(link, output);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                            while (File.Exists(output) == false)
                            {

                            }
                            break;
                                }
                            break;
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
                        try
                        {
                            Run(whole);
                        }catch (Exception ex)
                        {
                            Console.WriteLine("RunFile failure!");
                        }
                        }
                    else
                    {
                        catoexception("FileNotFound", fileName + " could not be found!", fileName, 0, 404);
                    }
                }
                else
                {
                    catoexception("FileWrongType", fileName + " is not a catoscript file!", fileName, 0, 405);
                }
            }
            else
            {
                catoexception("NullPeram", "\"Peram was not set to non-null value \nRun can not run an empty file.\"", fileName, 0, 101);
            }
        }
        static void Main(String[] args)
        {
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
                                switch (args[1])
                                {
                                    case "get":
                                        // Client.DownloadFile("https://script.cato.fun/pkgs/" + args[2] + "/data/" + args[2] + ".catop", "./logo.png");
                                        break;
                                    case "help":
                                        Console.WriteLine("------Pur Help-------");
                                        Console.WriteLine("quit - retrun to CatoScript CLI");
                                        Console.WriteLine("init - setup a CatoScript Project");
                                        break;
                                    case "init":
                                        if (!System.IO.File.Exists("main.cato"))
                                        {
                                            Console.WriteLine("Creating main cato file");
                                            using (System.IO.FileStream fs = System.IO.File.Create("main.cato"))
                                            {

                                            }
                                            using StreamWriter filee = new("main.cato", append: true);
                                            string tmp = "console.send |"+'"'+"Hello, cato"+'"'+"|;";
                                            filee.WriteLine(tmp);
                                        }
                                        else
                                        {
                                            Console.WriteLine("main file already exists!");
                                        }
                                        if (!System.IO.File.Exists("debug.catlog"))
                                        {
                                            Console.WriteLine("Creating debug log file");
                                            using (System.IO.FileStream fs = System.IO.File.Create("debug.catlog"))
                                            {

                                            }
                                            using StreamWriter filee = new("debug.catlog", append: true);
                                            filee.WriteLine("Project inited with pur " + CatoData.purver);
                                            filee.WriteLine("Project made in CatoScript " + CatoData.version);
                                        }
                                        else
                                        {
                                            Console.WriteLine("debug log already exists!");
                                        }
                                        Console.WriteLine("INIT Done!");
                                        break;
                                    case "quit":
                                        cli();
                                        break;
                                        break;
                                    default:
                                        Console.WriteLine("Unknown Command! Try help");
                                        break;
                                }
                            }
                            else
                            {
                                pur();
                            }
                            break;

                        case "":
                            cli();
                            break;
                        case "cli":
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
                            Console.WriteLine("eval - test code");
                            Console.WriteLine("start - start your catoscript project");
                            Console.WriteLine("activate - activate cato live run");
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
                            if (args[2] != string.Empty)
                            {
                                switch (args[2])
                                {
                                    case "-debug":
                                        //allow ddebug
                                        break;
                                    default:
                                        //nothing lel
                                        break;
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            try
                            {
                                RunFile(args[1]);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine(args[1] + "failed to run!");
                            }
                            Console.WriteLine("Cato File finnished running. Press any key to retrun to CLI");
                            Console.ReadKey();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.WriteLine("-------- CatoScript " + CatoData.version + " ----------");
                            Console.WriteLine("Cato CLI ready!");
                            break;
                        case "spawn":
                            spawn();
                            break;
                        case "activate":
                            arun();
                            break;
                        case "info":
                            Console.WriteLine("Catoscript - " + CatoData.version);
                            Console.WriteLine("Pur - " + CatoData.purver);
                            Console.WriteLine("Install Dir - " + Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
                            Console.WriteLine("Github - https://github.com/catocodedev/catoscript");
                            Console.WriteLine("Running OS - " + RuntimeInformation.OSDescription + "|" + RuntimeInformation.OSArchitecture);
                            break;
                        default:
                            if (args[0].Contains(".cato"))
                            {
                                try
                                {
                                    RunFile(args[0]);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine(args[0] + "failed to run!");
                                }
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
                Console.ForegroundColor = ConsoleColor.White;
                System.Environment.Exit(0);
            }
        }
    }
}
