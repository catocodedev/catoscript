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
                default:
                    Console.WriteLine("Nothing. . .");
                    break;
            }
            Console.Write("Press any key to close CatoScript...");
            Console.ReadKey();
        }
    }
}