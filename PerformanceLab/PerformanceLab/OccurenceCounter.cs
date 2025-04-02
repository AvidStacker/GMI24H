using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PerformanceLab
{
    static class OccurrenceCounter
    {
        public static void Setup()
        {
            Console.WriteLine("\nAnge storleken på listan:");
            int count = int.Parse(Console.ReadLine());

            Console.WriteLine("\nAnge minsta värde för nummerslumpgenereringen (t.ex. 1):");
            int minValue = int.Parse(Console.ReadLine());

            Console.WriteLine("\nAnge största värde för nummerslumpgenereringen (t.ex. 100):");
            int maxValue = int.Parse(Console.ReadLine());

            Console.WriteLine("\nAnge ett seed (eller tryck på Enter för att generera ett slumpmässigt seed):");
            string seedInput = Console.ReadLine();

            int seed;
            Random rand;

            if (int.TryParse(seedInput, out seed))
            {
                rand = new Random(seed);
                Console.WriteLine($"Använt seed: {seed}");
            }
            else
            {
                seed = new Random().Next();
                rand = new Random(seed);
                Console.WriteLine($"Inget seed angavs. Ett slumpmässigt seed genererades: {seed}");
            }

            int[] randomNumbers = GetPopulatedArray(count, minValue, maxValue, rand);
            Console.WriteLine($"\nPopulerad lista: " + string.Join(", ", randomNumbers));

            Console.WriteLine("\nAnge ett nummer som du vill räkna antalet förekomster av:");
            int numberToCount = int.Parse(Console.ReadLine());

            MeasureTime(randomNumbers, numberToCount);
        }

        private static void MeasureTime(int[] randomNumbers, int numberToCount)
        {
            DateTime startTime = DateTime.Now;
            int occurrenceCount = GetOccurrences(randomNumbers, numberToCount);
            DateTime stopTime = DateTime.Now;
            TimeSpan elapsed = stopTime - startTime;

            Console.WriteLine("\nKlockad tid:");
            Console.WriteLine(elapsed.ToString());
            Console.WriteLine($"Sekunder: {elapsed.TotalSeconds}");
            Console.WriteLine($"Millisekunder: {elapsed.TotalMilliseconds}");
            Console.WriteLine($"\nNummer {numberToCount} förekommer {occurrenceCount} gånger.");

            // Save data to CSV with the desired file name
            string filePath = SaveToCsv("PerformanceCounterData.csv", elapsed, occurrenceCount);
            Console.WriteLine($"Data sparad i: {filePath}");
        }

        public static int[] GetPopulatedArray(int count, int minValue, int maxValue, Random rand)
        {
            return PopulateArray(count, minValue, maxValue, rand);
        }

        internal static int[] PopulateArray(int count, int minValue, int maxValue, Random rand)
        {
            return Enumerable.Range(1, count)
                             .Select(_ => rand.Next(minValue, maxValue + 1))
                             .ToArray();
        }

        public static int GetOccurrences(int[] randomNumbers, int num)
        {
            return CalOccurrences(randomNumbers, num);
        }

        private static int CalOccurrences(int[] randomNumbers, int num)
        {
            int count = 0;
            foreach (int number in randomNumbers)
            {
                if (number == num)
                {
                    count++;
                }
            }
            return count;
        }

        // Updated SaveToCsv method that accepts fileName as an argument
        internal static string SaveToCsv(string fileName, TimeSpan elapsed, int? occurrenceCount = null)
        {
            string projectDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(projectDirectory, fileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Kategori;Värde");
                writer.WriteLine($"Förfluten Tid;{elapsed}");
                writer.WriteLine($"Sekunder;{elapsed.TotalSeconds}");
                writer.WriteLine($"Millisekunder;{elapsed.TotalMilliseconds}");
                writer.WriteLine();

                if (occurrenceCount.HasValue)
                {
                    writer.WriteLine("Förekomster av nummer");
                    writer.WriteLine($"{occurrenceCount.Value}");
                }
            }

            return filePath;
        }
    }
}
