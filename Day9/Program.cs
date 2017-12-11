using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText(@".\input.txt");
            var noExcl = RemoveExclamationMarks(input);
            var cleanInput = RemoveGarbage(noExcl);
            
            Console.WriteLine($"Day 9: Assignment 1: The groups generate {CalculateGroups(cleanInput.Item1)} points");
            Console.WriteLine($"Day 9: Assignment 2: The number of removed garbage chars is: {cleanInput.Item2}");

        }

        static int CalculateGroups(string line)
        {
            int value = 0;
            int currentDepth = 0;
            for (int i = 0; i < line.Length; i++)
            {
               switch(line[i])
                {
                    case '{':
                        currentDepth++;
                        value += currentDepth;
                        break;
                    case '}':
                        currentDepth--;
                        break;
                    case ',':
                        break;
                }
            }
            return value;
        }

        static string RemoveExclamationMarks(string line)
        {
            int exPos = line.IndexOf('!');
            while (exPos > -1)
            {
                line = line.Remove(exPos, 2);
                exPos = line.IndexOf('!');
            }
            return line;
        }
        static Tuple<string, int> RemoveGarbage(string line)
        {

            int removedChars = 0;
            int garbageStart = line.IndexOf('<');
            while (garbageStart > -1)
            {
                int garbageEnd = line.IndexOf('>', garbageStart + 1);
                removedChars += garbageEnd - garbageStart - 1;
                line = line.Remove(garbageStart, garbageEnd - garbageStart + 1);
                garbageStart = line.IndexOf('<');
            }
            return new Tuple<string,int>(line, removedChars);
        }
    }
}
