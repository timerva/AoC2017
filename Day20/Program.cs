using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(@".\input.txt");
            List<Point> particles = new List<Point>();

            for (int i = 0; i < input.Length; i++)
            {
                particles.Add(new Point(input[i].Split(", "), i));
            }

            var p1 = particles.OrderBy(p => p.GetAcceleration()).ThenByDescending(p => p.Velocity.GetManhattan()).First();

            Console.WriteLine($"Day 20: Assignment 1: The point that will stay closest to {{0,0,0}} is: {p1.ToString()}");

            var removeList = new List<Point>();
            int roundsWithSameAmountOfParticles = 0;
            while(true)
            { 
                particles.ForEach(p => p.MovePoint());

                foreach (var p in particles)
                {
                    var temp = p.Collide(particles);
                    removeList = temp.Union(removeList).ToList();
                }
                int nrOfParticles = particles.Count;
                removeList.ForEach(r => particles.Remove(r));
                if (nrOfParticles == particles.Count)
                    roundsWithSameAmountOfParticles++;
                else
                    roundsWithSameAmountOfParticles = 0;

                if (roundsWithSameAmountOfParticles > 100)
                    break;
            }

            Console.WriteLine($"Day 20: Assignment 2: The number of particles left is: {particles.Count}");
        }
    }



    public class Point
    {
        public int Number { get; set; }
        public Vector Pos { get; set; }
        public Vector Velocity { get; set; }
        public Vector Acceleration { get; set; }

        public int DistanceToCenter;

        private static readonly Vector centerVector = new Vector(0, 0, 0);
        public bool MyProperty { get; set; }
        public override string ToString()
        {
            return $"PointNr: {Number} Pos:{Pos}, Vel:{Velocity}, Acc:{Acceleration}";
        }
        public int GetAcceleration()
        {
            return Math.Abs(Acceleration.X) + Math.Abs(Acceleration.Y) + Math.Abs(Acceleration.Z);
        }

        public Point(int number)
        {
            Number = number;
            Pos = new Vector();
            Velocity = new Vector();
            Acceleration = new Vector();

        }

        public Point(string[] vectors, int number)
        {
            Number = number;
            Regex rex = new Regex("=<(.*)>");

            for (int i = 0; i < 3; i++)
            {
                var tt = rex.Split(vectors[i]);

                switch (tt[0])
                {

                    case "p":
                        Pos = new Vector(tt[1]);
                        break;
                    case "v":
                        Velocity = new Vector(tt[1]);
                        break;
                    case "a":
                        Acceleration = new Vector(tt[1]);
                        break;
                    default:
                        throw new Exception("Wrong vector type in input");
                }
            }
            DistanceToCenter = Pos.GetManhattan(centerVector);
        }

        public void MovePoint()
        {
            Velocity += Acceleration;
            var newPos = Pos + Velocity;
            Pos = newPos;
            DistanceToCenter = Pos.GetManhattan();
        }

        public List<Point> Collide(List<Point> points)
        {
            return points.Where(p => p.Pos == Pos && p.Number != Number).ToList();
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.Number == p2.Number;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return p1.Number != p2.Number;
        }

    }

    public class Vector
    {

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Vector))
            {
                return false;
            }

            var value = (Vector)obj;
            return Vector.Equals(this, value);
        }

        public static bool Equals(Vector v1, Vector v2)
        {
            return v1 == v2;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }

        public int GetManhattan()
        {
            return GetManhattan(new Vector());
        }

        public int GetManhattan(Vector vector)
        {
            return Math.Abs(vector.X - X) + Math.Abs(vector.Y - Y) + Math.Abs(vector.Z - Z);
        }

        public override string ToString()
        {
            return $"{X}:{Y}:{Z}";
        }
        public Vector() : this(0, 0, 0) { }
        public Vector(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector(string inputVector)
        {
            var t = inputVector.Split(',');
            X = int.Parse(t[0]);
            Y = int.Parse(t[1]);
            Z = int.Parse(t[2]);
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
