using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLab
{
    public static class Reverse
    {
        public static void Setup()
        {
            int count = 10;
            Tuple<int, int> range = new Tuple<int, int>(1, 100);
            List<int> randomNumbers = OccurenceCounter.PopulateList(range, count);

            Console.WriteLine("\nOriginal ordning: " + string.Join(", ", randomNumbers));
            Console.WriteLine("Omvänd ordning: " + string.Join(", ", ReverseOrder(randomNumbers)));
        }

        public static List<int> ReverseOrder(List<int> randomNumbers)
        {
            List<int> reversedList = new List<int>();
            for (int i = randomNumbers.Count - 1; i >= 0; i--)
            {
                reversedList.Add(randomNumbers[i]);
            }

            return reversedList;
        }
    }
}
