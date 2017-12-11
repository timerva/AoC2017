using System;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var steps = File.ReadAllText(@".\input.txt").Trim().Split(',');
            
            Point currentPosPoint = new Point(0, 0, 0);
            Point startPoint = new Point(0, 0, 0);
            int maxDistance = 0;
            foreach (var step in steps)
            {
                currentPosPoint = MovePoint(currentPosPoint, step);
                int tempDistance = GetDistance(startPoint, currentPosPoint);
                if (tempDistance > maxDistance)
                    maxDistance = tempDistance;
            }

            Console.WriteLine($"Day 11: Assignment 1: {currentPosPoint.X}:{currentPosPoint.Y}:{currentPosPoint.Z} Distance to start: {GetDistance(startPoint, currentPosPoint)}");
            Console.WriteLine($"Day 11: Assignment 2: Max distance from start: {maxDistance}");
        }

        public class Point
        {
            public int X, Y, Z;

            public Point(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        static Point MovePoint(Point point, string direction)
        {
            switch (direction)
            {
                case "se":
                    point.X++;
                    point.Y--;
                    break;
                case "s":
                    point.Y--;
                    point.Z++;
                    break;
                case "sw":
                    point.X--;
                    point.Z++;
                    break;
                case "ne":
                    point.X++;
                    point.Z--;
                    break;
                case "n":
                    point.Y++;
                    point.Z--;
                    break;
                case "nw":
                    point.X--;
                    point.Y++;
                    break;
            }
            return point;

        }
        static int GetDistance(Point p1, Point p2)
        {
            int dx = Math.Abs(p2.X - p1.X);
            int dy = Math.Abs(p2.Y - p1.Y);
            int dz = Math.Abs(p2.Z - p1.Z);
            //Eftersom dx + dy + dz = 0 så kan man lika gärna säga att man tar högsta värdet av de tre 
            //return (dx + dy + dz) / 2;

            return Math.Max(dx, Math.Max(dy, dz));
        }
    }
}
