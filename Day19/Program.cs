using System;
using System.Drawing;
using System.IO;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(@".\input.txt");
            Tuple<int, int> pos = new Tuple<int, int>(input[0].IndexOf("|"), 0);
            string letters = string.Empty;
            int nrOfSteps = 1;
            Dir direction = Dir.Down;
            bool endFound = false;
            while (!endFound)
            {
                pos = GetNextPos(direction, pos);
                switch (input[pos.Item2][pos.Item1])
                {
                    case '|':
                    case '-':
                        nrOfSteps++;
                        break;
                    case '+':
                        nrOfSteps++;

                        if (direction == Dir.Down || direction == Dir.Up)
                        {
                            if (input[pos.Item2][pos.Item1 + 1] == ' ')
                            {
                                direction = Dir.Left;
                            }
                            if (input[pos.Item2][pos.Item1 - 1] == ' ')
                            {
                                direction = Dir.Right;
                            }
                        }
                        else
                        {
                            if (input[pos.Item2 + 1][pos.Item1] == ' ')
                            {
                                direction = Dir.Up;
                            }
                            if (input[pos.Item2 - 1][pos.Item1] == ' ')
                            {
                                direction = Dir.Down;
                            }
                        }

                        break;
                    case ' ':
                        endFound = true;
                        break;
                    default:
                        nrOfSteps++;
                        letters += input[pos.Item2][pos.Item1];
                        break;

                }
            }
            Console.WriteLine($"Day 19 - Assignment 1: The value of the letters in order is: {letters}");
            Console.WriteLine($"Day 19 - Assignment 2: The number of steps taken is: {nrOfSteps}");

        }

        static Tuple<int, int> GetNextPos(Dir diretion, Tuple<int, int> pos)
        {
            switch (diretion)
            {
                case Dir.Down:
                    return new Tuple<int, int>(pos.Item1, pos.Item2 + 1);
                case Dir.Up:
                    return new Tuple<int, int>(pos.Item1, pos.Item2 - 1);
                case Dir.Left:
                    return new Tuple<int, int>(pos.Item1 - 1, pos.Item2);
                case Dir.Right:
                    return new Tuple<int, int>(pos.Item1 + 1, pos.Item2);
                default:
                    return null;
            }
        }

        enum Dir
        {
            Down,
            Up,
            Left,
            Right
        }
    }
}