using System;
namespace cato
{
    class cato
    {
        static void Main(String[] args)
        {
            string version = "Dev1.0.0";
            Console.WriteLine("---------CatoScript Dev1.0.0----------");
            Console.WriteLine("Waiting Input");
            switch (Console.ReadLine())
            {
                case "ver":
                    Console.WriteLine(version);
                    break;
                case "run":
                    string file = Console.ReadLine();
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
                        Console.WriteLine("File doesn't exsit");
                    }
                    break;
                default:
                    Console.WriteLine("Nothing. . .");
                    break;
            }
            Console.Write("Press any key to close CatoScript...");
            Console.ReadKey();
        }
    }
}