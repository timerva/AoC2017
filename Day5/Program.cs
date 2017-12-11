using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {

            var lines = File.ReadAllLines(@".\input.txt");
            var stack = lines.Select(int.Parse).ToArray();
            bool foundExit = false;
            int pos = 0;
            int exitPos = stack.Length;
            int numberOfSteps = 0;
            while (!foundExit)
            {
                int move = stack[pos];
                stack[pos]++;
                numberOfSteps++;
                pos += move;

                if (pos < 0 || pos >= exitPos)
                    foundExit = true;

            }
            Console.WriteLine($"Day 5 - Assignment 1: The number of steps is: {numberOfSteps}");

            stack = lines.Select(int.Parse).ToArray();
            foundExit = false;
            pos = 0;
            exitPos = stack.Length;
            numberOfSteps = 0;
            while (!foundExit)
            {
                int move = stack[pos];
                if (move < 3)
                    stack[pos]++;
                else
                    stack[pos]--;
                numberOfSteps++;
                pos += move;

                if (pos < 0 || pos >= exitPos)
                    foundExit = true;

            }
            Console.WriteLine($"Day 5 - Assignment 2: The number of steps in is: {numberOfSteps}");

        }
    }
}
