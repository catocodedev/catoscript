using System.Net;
using System.Runtime.InteropServices;
using Pastel;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace cato
{
    public class CatoData
    {
        public static string version = "Dev0.1.5 Experimental";
        public static string purver = "Dev0.1.2";
        public string OS = "null";     
    }
    public class Kits
    {
        private static Dictionary<string, string> kits = new Dictionary<string, string>();
        private static Dictionary<string, string> types = new Dictionary<string, string>();
        public static void Set(string key, string value, string type)
        {
            if (kits.ContainsKey(key))
            {
                kits[key] = value;            }
            else
            {
                kits.Add(key, value); 
                types.Add(key, type);
            }
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
        public static string GetType(string key)
        {
            string result = null;

            if (types.ContainsKey(key))
            {
                result = types[key];
            }

            return result;
        }
        public static void Remove(string key)
        {
            kits.Remove(key);
            types.Remove(key);
        }
        public static void Clear()
        {
            kits.Clear();
            types.Clear();
        }
    }
    public class Settings
    {
        private static Dictionary<string, string> sets = new Dictionary<string, string>();
        public static void Set(string key, string value)
        {
            if (sets.ContainsKey(key))
            {
                sets[key] = value;
            }
            else
            {
                sets.Add(key, value);
            }
        }
        public static string Get(string key)
        {
            string result = null;

            if (sets.ContainsKey(key))
            {
                result = sets[key];
            }
            else
            {
                result = null;
            }

            return result;
        }
        public static void Remove(string key)
        {
            sets.Remove(key);
        }
        public static void Clear()
        {
            sets.Clear();
        }
    }
    public class Kittens
    {
        private static Dictionary<string, string> Kitens = new Dictionary<string, string>();
        public static void Set(string key, string value)
        {
            if (Kitens.ContainsKey(key))
            {
                Kitens[key] = value;
            }
            else
            {
                Kitens.Add(key, value);
            }
        }
        public static string Get(string key)
        {
            string result = null;

            if (Kitens.ContainsKey(key))
            {
                result = Kitens[key];
            }

            return result;
        }
        public static void Remove(string key)
        {
            Kitens.Remove(key);
        }
        public static void Clear()
        {
            Kitens.Clear();
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
            if (File.Exists(Settings.Get("Debug_File")))
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
            if (File.Exists(Settings.Get("Debug_File")))
            {
                try
                {
                    using StreamWriter filee = new(Settings.Get("Debug_File"), append: true);
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
                    catoexception("FileWriteError", Settings.Get("Debug_File") + " could not be written too! details: " + ex, "writing to debug log", linenum, 407);
                }
            }
            else
            {
                catoexception("FileNotFound", Settings.Get("Debug_File") + " could not be found!", "writing to debug log", linenum, 404);
            }
        }
        public static async void httpdownload(string url, string file)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var fileInfo = new FileInfo(file);
                using (var fileStream = fileInfo.OpenWrite())
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }
        static void start(string option)
        {
                if (option == "-new")
                {
                    Console.WriteLine("Starting " + Settings.Get("Main_File") + "...");
                    try
                    {

                        // the /c will quit
                        System.Diagnostics.ProcessStartInfo procStartInfo =
                        new System.Diagnostics.ProcessStartInfo("cato " + Settings.Get("Main_File"));
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
                }
                else
                {
                    RunFile(Settings.Get("Main_File"));
                    Console.WriteLine("Project Ran!");
            }
            }
        static void Run(string run)
        {
            Kits.Clear();
            Kittens.Clear();
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
                // the /c will quit
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cato")
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
                RunFile(Settings.Get("Main_File"));
            }
            catch (Exception)
            {
                Console.WriteLine(Settings.Get("Main_File") + " failed to run!");
            }
            using var watcher = new FileSystemWatcher(Directory.GetCurrentDirectory());
            watcher.Filter = Settings.Get("Main_File");
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
                    RunFile(Settings.Get("Main_File"));
                }
                catch(Exception)
                {
                    Console.WriteLine(Settings.Get("Main_File") + " failed to run!");
                    Thread.Sleep(4000);
                }
                }
            while (run == "run")
            {
            
            }
        }
        static void cli(string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            string bs = "\\";
            Console.WriteLine("   ______      __       _____           _       __ \n" +
                "  / ____/___ _/ /_____ / ___/__________(_)___  / /_\n" +
                " / /   / __ `/ __/ __ \\\\__ \\/ ___/ ___/ / __ \\/ __/\n" +
                "/ /___/ /_/ / /_/ /_/ /__/ / /__/ /  / / /_/ / /_  \n" +
                "\\____/\\__,_/\\__/\\____/____/\\___/_/  /_/ .___/\\__/  \n" +
                "                                     /_/ \n" + CatoData.version);
            Console.WriteLine("Cato CLI ready!");
            if (input != String.Empty)
            {
                input = Console.ReadLine();
            }
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
                if (input == String.Empty)
                {
                    input = Console.ReadLine();
                }
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
                        pur("");
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
                        if (input == string.Empty)
                        {
                            start("-new");
                        }
                        start(input);
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
                input = "";
            }
        }
            static void pur(string purcmd)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
                Console.WriteLine(" ____  _  _  ____ \n" +
                    "(  _ \\/ )( \\(  _ \\ \n" +
                    " ) __ /) \\/ ( ) / \n" +
                    "(__)  \\____/(__\\_) \n" + CatoData.purver);
                Console.WriteLine("PUR CLI ready!");
            if (purcmd != String.Empty)
            {
                purcmd = Console.ReadLine();
            }
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
                if (purcmd == String.Empty)
                {
                    purcmd = Console.ReadLine();
                }
                switch (purcmd)
                {
                    case "get":
                        
                        break;
                    case "help":
                        Console.WriteLine("------Pur Help-------");
                        Console.WriteLine("quit - retrun to CatoScript CLI");
                        Console.WriteLine("init - setup a CatoScript Project");
                        break;
                    case "init":
                        if (!System.IO.File.Exists(Settings.Get("Main_File")))
                        {
                            Console.WriteLine("Creating " + Settings.Get("Main_File") + " file");
                            using (System.IO.FileStream fs = System.IO.File.Create("main.cato"))
                            {

                            }
                            using StreamWriter filee = new(Settings.Get("Main_File"), append: true);
                            string tmp = "console.send |" + '"' + "Hello, cato" + '"' + "|;";
                            filee.WriteLine(tmp);
                        }
                        else
                        {
                            Console.WriteLine(Settings.Get("Main_File") + " already exists!");
                        }
                        if (!System.IO.File.Exists(Settings.Get("Debug_File")))
                        {
                            Console.WriteLine("Creating " + Settings.Get("Debug_File") +" file");
                            using (System.IO.FileStream fs = System.IO.File.Create(Settings.Get("Debug_File")))
                            {

                            }
                            using StreamWriter filee = new(Settings.Get("Debug_File"), append: true);
                            filee.WriteLine("Project inited with pur " + CatoData.purver);
                            filee.WriteLine("Project made in CatoScript " + CatoData.version);
                        }
                        else
                        {
                            Console.WriteLine(Settings.Get("Debug_File") + " already exists!");
                        }
                        Console.WriteLine("INIT Done!");
                        break;
                    case "quit":
                        cli("");
                        break;
                    case "settings":
                        // code for adjusting and getting settings
                        break;
                    default:
                        Console.WriteLine("Unknown Command! Try help");
                        break;
                }
                Console.ReadKey();
                purcmd = "";
            }   
        }
        public static void Parse(string perams, string type)
        {

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
                        case "get.ver":
                            Console.WriteLine(CatoData.version);
                            break;
                        case "get.pur.ver":
                            Console.WriteLine(CatoData.purver);
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
                                Kits.Set(parsed[0], parsed[1],parsed[2]);
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
                                Kits.Set(parsed[0], parsed[1],parsed[2]);
                            }
                            catch (Exception)
                            {
                                catoexception("InvaildKit", "The kit " + parsed[0] + " wasn't found!", op, opnum, 301);
                            }
                            break;
                        case "get":
                            Console.WriteLine(Kits.Get(parsed[0]));
                            break;
                        case "getype":
                            Console.WriteLine(Kits.GetType(parsed[0]));
                            break;
                        case "set.from.input":
                            string value = Console.ReadLine();
                            try
                            {
                                Kits.Set(parsed[0], value, parsed[2]);
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

                            try { 
                                // the /c will quit
                                System.Diagnostics.ProcessStartInfo procStartInfo =
                                    new System.Diagnostics.ProcessStartInfo(text);
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
                    switch (subop)
                    {
                        case "get":
                            // add http req code
                            break;
                        case "download":
                            string link = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            string output = parsed[1].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                            try
                            {
                                httpdownload(link, output);
                            }
                            catch (Exception ex)
                            {
                                catoexception("HttpSrcError", "File " + output + " could not be downloaded from " + link , op, opnum, 501);
                            }
                            while (File.Exists(output) == false)
                            {

                            }
                            break;
                        case "host":
                            try
                            {
                               // soon
                            }
                            catch (Exception e)
                            {
                                catoexception("HttpHostError", "Server could not be ran", op, opnum, 502);
                            }
                            break;
                        default:
                            catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of http", op, opnum, 104);
                            break;
                    }
                    break;
                case "sound":
                    switch (subop)
                    {
                        case "beep":
                    Console.Beep();
                            break;
                default:
                    catoexception("Invaild SubOperation", subop + "Is not a vaild SubOperation of sound", op, opnum, 104);
                    break;
            }
                    break;
                    break;
            default:
                    catoexception("InvalidOperation", "\"This Operation was not recognized.\"", op, opnum, 100);
            break;
            if (Settings.Get("Debug") == "True")
                    {
                        delog("Sucessfully Ran: " + op,opnum,0);
                    }
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
            string settingfile = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "/settings.catofig";
            if (!File.Exists(settingfile)) {
                Console.WriteLine("Generating catoscript settings file . . .");
                using (System.IO.FileStream fs = System.IO.File.Create(settingfile))
                { }
                try
                {
                    using StreamWriter filee = new(settingfile, append: true);
                    filee.WriteLine("%Catoscript Global Settings File!;");
                    filee.WriteLine("Main_File: main.cato;");
                    filee.WriteLine("Debug_File: debug.catlog;");
                    filee.WriteLine("Debug: false;");
                }
                catch (Exception ex)
                {
                    catoexception("FileWriteError", settingfile + " could not be written too! details: " + ex, "Creating settings file", -1, 407);
                }
            }
            // setting processing
            string tmpset = "";
                string[] readText = File.ReadAllLines(settingfile);
                readText.Each((str, n) =>
                {
                    tmpset = tmpset + str;
                });
                string[] subs = tmpset.Split(';');
                foreach (string sub in subs)
                {
                    string trimmed = "";
                    trimmed = sub.Trim();
                    if (trimmed.StartsWith("%") || trimmed == String.Empty)
                    {

                    }
                    else
                    {
                        var pieces = trimmed.Split(new[] { ':' });
                        Settings.Set(pieces[0].Trim(), pieces[1].Trim());
                        // Console.WriteLine(Settings.Get(pieces[0]));
                    }
                    }

            if (Settings.Get("Main_File") == null || Settings.Get("Debug_File") == null || Settings.Get("Debug") == null)
            {
                catoexception("ConfigError", settingfile + " does not have the required settings for catoscript to run!", "Vaildating Settings", -1, 106);
            }
                if (args == null || args.Length == 0)
            {
                cli("");
            }
            else
            {
                try
                {
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
                    else if (args[0] == "pur")
                    {
                        pur(args[0]);
                    }
                    else
                    {
                        cli(args[0]);
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
