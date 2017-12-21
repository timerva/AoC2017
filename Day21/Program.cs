using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            string startMatrix = ".#./..#/###";

            var input = File.ReadAllLines(@".\input.txt");
            List<Rule> rules = input.Select(s => new Rule(s)).ToList();



            var matrix = new Matrix(3, startMatrix);
            

            for (int i = 0; i < 5; i++)
            {
                matrix = matrix.GenerateNewMatrixFromRules(rules);   
            }

            Console.WriteLine($"Day 21 - Assignment 1: The number of pixels on is: {matrix.NumberOfPixelsOn}");

            for (int i = 0; i < 13; i++)
            {
                matrix = matrix.GenerateNewMatrixFromRules(rules);
            }
            Console.WriteLine($"Day 21 - Assignment 2: The number of pixels on is: {matrix.NumberOfPixelsOn}");

        }
    }

    public class Rule
    {
        public char[,] Pattern;
        public char[,] Enhancment;
        public Rule(string ruleRow)
        {
            var split1 = ruleRow.Split(" => ");
            var splitPattern = split1[0].Split('/');
            var splitEnhancment = split1[1].Split('/');
            Pattern = new char[splitPattern.Length, splitPattern.Length];
            Enhancment = new char[splitEnhancment.Length, splitEnhancment.Length];
            for (int row = 0; row < splitPattern.Length; row++)
            {
                for (int i = 0; i < splitPattern.Length; i++)
                {
                    Pattern[i, row] = splitPattern[row][i];
                }
            }

            for (int row = 0; row < splitEnhancment.Length; row++)
            {
                for (int i = 0; i < splitEnhancment.Length; i++)
                {
                    Enhancment[i, row] = splitEnhancment[row][i];
                }
            }
        }

    }

    public class Matrix
    {
        public int Size { get; set; }

        private char[,] _matrix;

        
        public int NumberOfPixelsOn
        {
            get;set;
        }

        public char[,] Data
        {
            get { return _matrix; }
            set { _matrix = value; ; }
        }

        public Matrix(int size, string pattern)
        {
            Size = size;
            NumberOfPixelsOn = 0;
            Data = new char[size, size];
            var split = pattern.Split('/');

            for (int row = 0; row < size; row++)
            {
                for (int i = 0; i < size; i++)
                {
                    Data[i, row] = split[row][i];
                    if (Data[i, row] == '#')
                        NumberOfPixelsOn++;
                }
            }

        }

        public Matrix(int size)
        {
            Size = size;
            Data = new char[size, size];
            NumberOfPixelsOn = 0;
        }

        private char[,] MatchesRule(int size, int x, int y, List<Rule> rules)
        {
            char[,] tempdata = GetSubData(x, y, size);


            foreach (var rule in rules)
            {
                if (tempdata.GetLength(0) != rule.Pattern.GetLength(0))
                    continue;
                if (Matrix.MatchRule(tempdata, rule.Pattern))
                    return rule.Enhancment;

                for (int i = 0; i < 3; i++)
                {
                    tempdata = Matrix.Transform(tempdata, Direction.Clockwise);
                    if (Matrix.MatchRule(tempdata, rule.Pattern))
                        return rule.Enhancment;

                }
                tempdata = Matrix.Transform(tempdata, Direction.FlipVertical);
                for (int i = 0; i < 4; i++)
                {
                    tempdata = Matrix.Transform(tempdata, Direction.Clockwise);
                    if (Matrix.MatchRule(tempdata, rule.Pattern))
                        return rule.Enhancment;
                }
            }
            return null;
        }

        public static bool MatchRule(char[,] data, char[,] pattern)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    if (data[i, j] != pattern[i, j])
                        return false;
                }
            }
            return true;
        }

        private char[,] GetSubData(int x, int y, int size)
        {
            char[,] tempData = new char[size, size];
            for (int row = 0; row < size; row++)
            {
                for (int kol = 0; kol < size; kol++)
                {
                    tempData[kol, row] = Data[x * size + kol, y * size + row];
                }
            }
            return tempData;
        }

        private static void SetSubData(int x, int y, int size, char[,] data, Matrix matrix)
        {
            for (int row = 0; row < size; row++)
            {
                for (int kol = 0; kol < size; kol++)
                {
                    matrix.Data[x * size + kol, y * size + row] = data[kol, row];
                    if (data[kol, row] == '#')
                        matrix.NumberOfPixelsOn++;
                }
            }
        }


        public Matrix GenerateNewMatrixFromRules(List<Rule> rules)
        {
            int subSize = 0;
            int newSize = 0;
            if (Size % 2 == 0)
            {
                subSize = 2;
                newSize = Size / 2 * (subSize + 1);
            }
            else
            {
                subSize = 3;
                newSize = Size / 3 * (subSize + 1);
            }

            var newMatrix = new Matrix(newSize);

            for (int y = 0; y < Size / subSize; y++)
            {
                for (int x = 0; x < Size / subSize; x++)
                {
                    foreach (var rule in rules)
                    {
                        var enhancement = MatchesRule(subSize, x, y, rules);
                        if (enhancement != null)
                        {
                            SetSubData(x, y, subSize +1, enhancement, newMatrix);
                            break;
                        }
                    }
                }

            }
            return newMatrix;
        }


        public static char[,] Transform(char[,] matrix, Direction direction)
        {
            var newMatrix = (char[,])matrix.Clone();
            var Size = matrix.GetLength(0);

            switch (direction)
            {
                case Direction.Clockwise:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            newMatrix[i, j] = matrix[Size - j - 1, i];
                        }
                    }
                    break;
                case Direction.CounterClockwise:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            newMatrix[i, j] = matrix[j, Size - i - 1];
                        }
                    }

                    break;
                case Direction.FlipVertical:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            newMatrix[Size - i - 1, j] = matrix[i, j];
                        }
                    }
                    break;
                case Direction.Horizontal:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            newMatrix[i, Size - j - 1] = matrix[i, j];
                        }
                    }
                    break;

            }
            return newMatrix;
        }

        public enum Direction
        {
            Clockwise,
            CounterClockwise,
            FlipVertical,
            Horizontal
        }
    }
}
