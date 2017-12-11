using System;
using System.Linq;
using System.Collections.Generic;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = 312051;
            int gridSize = (int)Math.Ceiling(Math.Sqrt(input));
            var assignmentOnePoint = new Point(input);

            //Eftersom det är en 2-dimensionell grid x/y så är antalet steg man måste gå samma sak som antalet steg i x+y led
            Console.WriteLine($"Day 3 - Assigngment 1: It's {Math.Abs(assignmentOnePoint.Y) + Math.Abs(assignmentOnePoint.X)} steps");

            Grid grid = new Grid();

            //Lägg till mitten noden som har värdet 1 för uppgift 2
            grid.Add(new Point(1)
            {
                ValueAssignment2 = 1
            });

            for (int i = 1; i < input; i++)
            {
                var tempPoint = new Point(i + 1);
                //Hämta alla just nu befintliga grannars värde2 och summera dessa, sätt till nya punktens värde2
                var tempValue = grid.GetNeighboursValueAssignment2(tempPoint);
                tempPoint.ValueAssignment2 = tempValue;
                grid.Add(tempPoint);
                if (tempValue > input)
                {
                    Console.WriteLine($"Day3 - Assignment 2: The first value written that is larger than the input value is: {tempValue}");
                    break;
                }
            }
            Console.WriteLine("Klar");
        }
    }


    public class Grid
    {
        private List<Point> _points;
        public Grid()
        {
            _points = new List<Point>();
        }
        
        public void Add(Point point)
        {
            _points.Add(point);
        }

        public Point GetByXY(int x, int y)
        {
            return _points.SingleOrDefault(p => p.X == x && p.Y == y);
        }

        public int GetNeighboursValueAssignment2(Point point)
        {
            int tempValue = 0;
            tempValue += GetValueTwo(point.Y - 1, point.X - 1);
            tempValue += GetValueTwo(point.Y - 1, point.X);
            tempValue += GetValueTwo(point.Y - 1, point.X + 1);
            tempValue += GetValueTwo(point.Y, point.X - 1);
            tempValue += GetValueTwo(point.Y, point.X + 1);
            tempValue += GetValueTwo(point.Y + 1, point.X - 1);
            tempValue += GetValueTwo(point.Y + 1, point.X);
            tempValue += GetValueTwo(point.Y + 1, point.X + 1);
            return tempValue;
        }

        private int GetValueTwo(int x, int y)
        {
            var entry = _points.SingleOrDefault(p => p.Y == x && p.X == y);
            if (entry != null)
            {
                return entry.ValueAssignment2;
            }
            return 0;
        }

    }


    public class Point
    {
        private int _gridSize { get; set; }

        public Point(int value)
        {
            ValueAssignment1 = value;
            _gridSize = (int)Math.Ceiling(Math.Sqrt(value));
            //Griden kan inte vara t.ex. 4x4 då det inte finns någon mitten, alltså inga jämna gridstorelekar möjliga, se till att 
            //öka värdet till ett ojämt tal.
            if (_gridSize % 2 == 0)
            {
                _gridSize++;
            }

            this.CalculateXY();
        }
        public int Y { get; set; }
        public int X
        {
            get; set;
        }

        public int ValueAssignment1 { get; set; }
        public int ValueAssignment2 { get; set; }

        private void CalculateXY()
        {
            int StartValue = (int)Math.Pow(_gridSize - 2, 2) + 1;
            int upperRight = StartValue + _gridSize - 2;
            int upperLeft = upperRight + _gridSize - 1;
            int lowerLeft = upperLeft + _gridSize - 1;
            int lowerRight = lowerLeft + _gridSize - 1;
            int midsize = (int)Math.Floor((lowerRight - lowerLeft) / 2d);

            if (ValueAssignment1 >= StartValue && ValueAssignment1 <= upperRight)
            {
                X = (int)Math.Floor(_gridSize / 2d);
                Y = ValueAssignment1 - StartValue + 1 - midsize;
            }
            if (ValueAssignment1 > upperRight && ValueAssignment1 <= upperLeft)
            {
                Y = (int)Math.Floor(_gridSize / 2d);
                X = upperLeft - ValueAssignment1 - midsize;
            }
            if (ValueAssignment1 > upperLeft && ValueAssignment1 <= lowerLeft)
            {
                X = -(int)Math.Floor(_gridSize / 2d);
                Y = lowerLeft - ValueAssignment1 - midsize;
            }
            if (ValueAssignment1 > lowerLeft && ValueAssignment1 <= lowerRight)
            {
                X = ValueAssignment1 - lowerLeft - midsize;
                Y = -(int)Math.Floor(_gridSize / 2d);
            }
        }
    }
}
