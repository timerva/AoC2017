using System;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@".\Input.txt");
            int rowDiff = 0;
            int checksum = 0;
            foreach (var line in lines)
            {
                var numbers = line.Split('\t').Select(int.Parse);
                rowDiff = numbers.Max() - numbers.Min();
                checksum += rowDiff;
            }
            Console.WriteLine($"Day2 - Assingment 1: The checksum is: {checksum}");

            int totalResult = 0;
            foreach (var line in lines)
            {
                int rowResult = 0;
                //Eftersom det endast är möjligt att få modulus = 0 om man delar ett större tal med ett mindre, så länge det är postivia tal kan vi sortera
                var numbers = line.Split('\t').Select(int.Parse).OrderByDescending(p => p).ToList();
                
                for (int i = 0; i < numbers.Count(); i++)
                {
                    for (int j = i + 1; j < numbers.Count(); j++)
                    {
                        if (CheckEvenDivide(numbers[i], numbers[j]))
                        {
                            rowResult += numbers[i] / numbers[j];
                        }
                    }
                }
                totalResult += rowResult;
            }
            Console.WriteLine($"Day2 - Assingment 2: The result is: {totalResult}");
        }
        static bool CheckEvenDivide(int nrOne, int nrTwo)
        {
            return nrOne % nrTwo == 0;
        }

    }
}