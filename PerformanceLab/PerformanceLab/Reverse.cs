using System;
using System.Linq;

namespace PerformanceLab
{
    public static class Reverse
    {
        public static void Setup()
        {
            // Asking the user for the size of the list
            Console.WriteLine("\nAnge storleken på listan:");
            int count = int.Parse(Console.ReadLine());

            // Asking for the range (minValue and maxValue) for random number generation
            Console.WriteLine("\nAnge minsta värde för nummerslumpgenereringen (t.ex. 1):");
            int minValue = int.Parse(Console.ReadLine());

            Console.WriteLine("\nAnge största värde för nummerslumpgenereringen (t.ex. 100):");
            int maxValue = int.Parse(Console.ReadLine());

            // Asking the user for the seed for reproducibility
            Console.WriteLine("\nAnge ett seed (eller tryck på Enter för att generera ett slumpmässigt seed):");
            string seedInput = Console.ReadLine();

            int seed;
            Random rand;

            // If the user provides a seed, use it; otherwise, generate a random seed
            if (int.TryParse(seedInput, out seed))
            {
                rand = new Random(seed);
                Console.WriteLine($"Använt seed: {seed}");
            }
            else
            {
                seed = new Random().Next(); // Generate a random seed
                rand = new Random(seed);
                Console.WriteLine($"Inget seed angavs. Ett slumpmässigt seed genererades: {seed}");
            }

            // Generate the list using GetPopulatedArray method from OccurrenceCounter
            int[] randomNumbers = OccurrenceCounter.GetPopulatedArray(count, minValue, maxValue, rand);
            Console.WriteLine("\nOriginal ordning: " + string.Join(", ", randomNumbers));

            // Asking if the user wants to use the slow method for reversal
            Console.WriteLine("\nVill du använda den långsamma metoden för att vända listan? (Ja/Nej)");
            string useSlowMethod = Console.ReadLine()?.ToLower();

            int[] reversedArray;
            string fileName;

            DateTime startTime = DateTime.Now;
            DateTime stopTime;
            TimeSpan elapsed;

            if (useSlowMethod == "ja")
            {
                reversedArray = ReverseOrderSlow(randomNumbers);
                Console.WriteLine("Omvänd ordning (långsam metod): " + string.Join(", ", reversedArray));

                stopTime = DateTime.Now;
                elapsed = stopTime - startTime;

                Console.WriteLine("\nTidsförbrukning för långsam metod:");
                Console.WriteLine($"Förfluten tid: {elapsed}");
                Console.WriteLine($"Sekunder: {elapsed.TotalSeconds}");
                Console.WriteLine($"Millisekunder: {elapsed.TotalMilliseconds}");

                // Set the file name for slow method
                fileName = "PerformanceReverseOrderSlowData.csv";
            }
            else
            {
                reversedArray = ReverseOrder(randomNumbers);
                Console.WriteLine("Omvänd ordning (normal metod): " + string.Join(", ", reversedArray));

                stopTime = DateTime.Now;
                elapsed = stopTime - startTime;

                Console.WriteLine("\nTidsförbrukning för normal metod:");
                Console.WriteLine($"Förfluten tid: {elapsed}");
                Console.WriteLine($"Sekunder: {elapsed.TotalSeconds}");
                Console.WriteLine($"Millisekunder: {elapsed.TotalMilliseconds}");

                // Set the file name for normal method
                fileName = "PerformanceReverseOrderNormalData.csv";
            }

            // Call SaveToCsv from OccurrenceCounter and pass the appropriate file name
            string filePath = OccurrenceCounter.SaveToCsv(fileName, elapsed, null);
            Console.WriteLine($"Data sparad i: {filePath}");
        }

        static int[] ReverseOrder(int[] randomNumbers)
        {
            int[] reversedArray = new int[randomNumbers.Length];
            for (int i = 0; i < randomNumbers.Length; i++)
            {
                reversedArray[i] = randomNumbers[randomNumbers.Length - 1 - i];
            }
            return reversedArray;
        }

        // Slow method to reverse the array
        static int[] ReverseOrderSlow(int[] values)
        {
            int nextValue = 0;
            for (int i = 1; i < values.Length; i++)
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
    }
}
