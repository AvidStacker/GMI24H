using System;
using System.Collections.Generic;
using System.Data;
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
            List<int> randomNumbers = GetPopulatedList(range, count, seed);
            Console.WriteLine($"\nPopulerad lista: " + string.Join(", ", randomNumbers));
            MeasureTime(randomNumbers);
        }

        private static void MeasureTime(List<int> randomNumbers)
        {
            DateTime startTime = DateTime.Now;
            Dictionary<int, int> occurrences = GetOccurrences(randomNumbers);
            DateTime stopTime = DateTime.Now;
            TimeSpan elapsed = stopTime - startTime;
            Console.WriteLine("\nKlockad tid:");
            Console.WriteLine(elapsed.ToString());
            Console.WriteLine($"Sekunder: {elapsed.Seconds}");
            Console.WriteLine($"Millisekunder: {elapsed.Milliseconds}");
            Console.WriteLine("\nAntal förekomster:");

            foreach (var pair in occurrences)
            {
                Console.WriteLine($"Talet {pair.Key} förekommer {pair.Value} gånger.");
            }

            string filePath = SaveToCsv(elapsed, occurrences);

            Console.WriteLine($"Data sparad i: {filePath}");
        }

        public static List<int> GetPopulatedList(Tuple<int, int> range, int count, int? seed = null)
        {
            return PopulateList(range, count, seed);
        }

        static List<int> PopulateList(Tuple<int, int> range, int count, int? seed = null)
        {
            if (!seed.HasValue)
            {
                seed = new Random().Next();
            }

            Console.WriteLine("\nFrö: " + seed.Value);
            Random rand = new Random(seed.Value);

            // Generate 'count' random numbers within the given range
            List<int> randomNumbers = Enumerable.Range(0, count)
                                                 .Select(_ => rand.Next(range.Item1, range.Item2 + 1))
                                                 .ToList();

            return randomNumbers;
        }


        public static Dictionary<int, int> GetOccurrences(List<int> randomNumbers)
        {
            return CalOccurrences(randomNumbers);
        }

        private static Dictionary<int, int> CalOccurrences(List<int> randomNumbers)
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

        public static Dictionary<int, int> GetOccurrencesSlow(List<int> values, int n)
        {
            return CalOccurrencesSlow(values, n);
        }

        private static Dictionary<int, int> CalOccurrencesSlow(List<int> values, int n)
        {
            int nextValue = 0;
            for (int i = 1; i < n; i++)
            {
                nextValue = values[i];
                for (int j = i; j > 0; j--)
                {
                    values[j] = values[j - 1];
                }
                values[0] = nextValue;
            }
            return values;
        }

        static string SaveToCsv(TimeSpan elapsed, Dictionary<int, int> occurrences)
        {
            string projectDirectory = Directory.GetCurrentDirectory();

            string filePath = Path.Combine(projectDirectory, "Occurrences.csv");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Kategori;Värde");
                writer.WriteLine($"Förfluten Tid;{elapsed}");
                writer.WriteLine($"Sekunder;{elapsed.TotalSeconds}");
                writer.WriteLine($"Millisekunder;{elapsed.TotalMilliseconds}");
                writer.WriteLine(); // Tom rad för separering

                writer.WriteLine("Nummer;Förekomster");
                foreach (var pair in occurrences)
                {
                    writer.WriteLine($"{pair.Key};{pair.Value}");
                }
            }
            return filePath;
        }
    }
}
