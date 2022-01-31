using System.Runtime.InteropServices;
using Pastel;

namespace cato
{
    public class CatoData
    {
        public static string version = "Dev0.1.4";
        public static string purver = "Dev0.1.1";
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
            Console.WriteLine("more info https://github.com/catoscript/cato/wiki/Errors#" + errornum);
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            System.Environment.Exit(errornum);

        }
        static void start()
        {
            Console.WriteLine("Starting main.cato ...");
            RunFile("main.cato");
        }
        static void Run(string run)
        {
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
                        Console.WriteLine("eval - test code");
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
        static void Execute(string op, string topop, string subop, string perams, int opnum)
        {
            var kit = new Kits();
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
                switch (topop)
            {
                case "%":
                    // nothing because comment
                    break;
                case "console":
                    var text = "";
                    if (!subop.Contains("object"))
                    {
                        if (parsed[0] != "#")
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
                                Console.WriteLine(text.Pastel("#"+color));
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \nconsole.send can not send an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "clean":
                            Console.Clear();
                            break;
                        case "overwrite.up":
                            if (text != String.Empty)
                            {
                                Console.SetCursorPosition(0, Console.CursorTop - 1);
                                Console.WriteLine(text);
                            }
                            else
                            {
                                catoexception("NullPeram", "\"Peram was not set to non-null value \noverwrite.up can not overwrite with an empty string.\"", op, opnum, 101);
                            }
                            break;
                        case "object.load":
                            // object parser
                            var meowobject = perams.Split(new string[] { "(",")" }, 3, StringSplitOptions.None)[1];
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
                            if (objnum > 2 && objnum < 5){
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
                    text = parsed[0].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                    switch (subop)
                    {
                        case "throw":
                            catoexception("UserGenerated", text, op, opnum, 200);
                            break;
                        case "get.OS":
                            Console.WriteLine(RuntimeInformation.OSDescription + "|" + RuntimeInformation.OSArchitecture);
                            break;
                        case "log":
                            if (File.Exists("debug.catlog"))
                            {
                                text = parsed[1].Split(new string[] { "\"" }, 3, StringSplitOptions.None)[1];
                                try
                                {
                                    using StreamWriter filee = new("debug.catlog", append: true);
                                    filee.WriteLineAsync(text);
                                }
                                catch (Exception ex)
                                {
                                    catoexception("FileWriteError", "debug.catlog" + " could not be written too! details: " + ex, op, opnum, 407);
                                }
                            }
                            else
                            {
                                catoexception("FileNotFound", "debug.catlog" + " could not be found!", op, opnum, 404);
                            }
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
                                try
                            {
                                // the /c will quit
                                System.Diagnostics.ProcessStartInfo procStartInfo =
                                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + text);
                                // The following commands are needed to redirect the standard output.
                                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                                procStartInfo.RedirectStandardOutput = true;
                                procStartInfo.UseShellExecute = false;
                                // Do not create the black window.
                                procStartInfo.CreateNoWindow = true;
                                // Now we create a process, assign its ProcessStartInfo and start it
                                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                                proc.StartInfo = procStartInfo;
                                proc.Start();
                                // Get the output into a string
                                string result = proc.StandardOutput.ReadToEnd();
                                // Display the command output.
                                Console.WriteLine(result);
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
                                    filee.WriteLineAsync(text);
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
                                File.WriteAllTextAsync(file, text);
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
                Console.ForegroundColor = ConsoleColor.White;
                System.Environment.Exit(0);
            }
        }
    }
}
