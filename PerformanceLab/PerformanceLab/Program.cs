using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;

namespace PerformanceLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Alternativ 1: Klocka algoritm\nAlternativ 2: Vänd om lista");
            int inp = Convert.ToInt32(Console.ReadLine());
            switch (inp)
            {
                case 1:
                    OccurenceCounter.Setup();
                    break;
                case 2:
                    Reverse.Setup();
                    break;
            }
        }
    }
}
