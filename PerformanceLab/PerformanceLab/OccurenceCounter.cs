using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLab
{
    public static class OccurenceCounter
    {

        public static void Setup()
        {
            int count = 10;
            Tuple<int, int> range = new Tuple<int, int>(1, 100);
            Console.WriteLine("\nAnge frö (om spalten lämnas tom slumpas listan):");
            string input = Console.ReadLine();
            int? seed = null;
            if (int.TryParse(input, out int parsedSeed))
            {
                seed = parsedSeed;
            }
            List<int> randomNumbers = PopulateList(range, count, seed);
            Console.WriteLine($"\nPopulerad lista: " + string.Join(", ", randomNumbers));
            MeasureTime(randomNumbers);
        }

        public static void MeasureTime(List<int> randomNumbers)
        {
            DateTime startTime = DateTime.Now;
            Dictionary<int, int> occurences = GetOccurrences(randomNumbers);
            DateTime stopTime = DateTime.Now;
            TimeSpan elapsed = stopTime - startTime;
            Console.WriteLine("\nKlockad tid:");
            Console.WriteLine(elapsed.ToString());
            Console.WriteLine($"Sekunder: {elapsed.Seconds}");
            Console.WriteLine($"Millisekunder: {elapsed.Milliseconds}");
            Console.WriteLine("\nAntal förekomster:");

            foreach (var pair in occurences)
            {
                Console.WriteLine($"Talet {pair.Key} förekommer {pair.Value} gånger.");
            }
        }

        public static List<int> PopulateList(Tuple<int, int> range, int count, int? seed = null)
        {
            if (!seed.HasValue)
            {
                seed = new Random().Next();
            }

            Console.WriteLine("\nFrö: " + seed.Value);
            Random rand = new Random(seed.Value);

            List<int> randomNumbers = Enumerable.Range(range.Item1, range.Item2 - range.Item1 + 1)
                                                 .Select(_ => rand.Next(range.Item1, range.Item2 + 1))
                                                 .Take(count)
                                                 .ToList();

            return randomNumbers;
        }


        public static Dictionary<int, int> GetOccurrences(List<int> randomNumbers)
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();

            foreach (int number in randomNumbers)
            {
                bool found = false;

                foreach (var pair in occurrences)
                {
                    if (pair.Key == number)
                    {
                        occurrences[pair.Key] = occurrences[pair.Key] + 1;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    occurrences[number] = 1;
                }
            }

            return occurrences;
        }


    }
}
