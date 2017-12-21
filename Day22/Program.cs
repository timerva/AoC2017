using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(@".\input.txt");
            ComputerCluster cluster = new ComputerCluster(input);

            for (int i = 0; i < 10000; i++)
            {

                if (!cluster.Nodes.ContainsKey(cluster.CurrentNode))
                {
                    cluster.Nodes.Add(cluster.CurrentNode, InfectionState.Clean);
                }

                switch (cluster.Nodes[cluster.CurrentNode])
                {
                    case InfectionState.Clean:
                        cluster.Nodes[cluster.CurrentNode] = InfectionState.Infected;
                        cluster.RotateDirection(Direction.Left);
                        cluster.NumberOfInfections++;
                        break;
                    case InfectionState.Infected:
                        cluster.Nodes[cluster.CurrentNode] = InfectionState.Clean;
                        cluster.RotateDirection(Direction.Right);
                        break;
                }
                cluster.Move();
            }

            Console.WriteLine($"Day 22 - Assignement 1: The number of infections: {cluster.NumberOfInfections}");

            cluster.InitCluster(input);
            for (int i = 0; i < 10000000; i++)
            {

                if (!cluster.Nodes.ContainsKey(cluster.CurrentNode))
                {
                    cluster.Nodes.Add(cluster.CurrentNode, InfectionState.Clean);
                }

                switch (cluster.Nodes[cluster.CurrentNode])
                {
                    case InfectionState.Clean:
                        cluster.Nodes[cluster.CurrentNode] = InfectionState.Weakened;
                        cluster.RotateDirection(Direction.Left);
                        break;
                    case InfectionState.Weakened:
                        cluster.Nodes[cluster.CurrentNode] = InfectionState.Infected;
                        cluster.NumberOfInfections++;
                        break;
                    case InfectionState.Infected:
                        cluster.Nodes[cluster.CurrentNode] = InfectionState.Flagged;
                        cluster.RotateDirection(Direction.Right);
                        break;
                    case InfectionState.Flagged:
                        cluster.Nodes[cluster.CurrentNode] = InfectionState.Clean;
                        cluster.RotateDirection(Direction.Left);
                        cluster.RotateDirection(Direction.Left);
                        break;
                }
                cluster.Move();
            }

            Console.WriteLine($"Day 22 - Assignement 2: The number of infections: {cluster.NumberOfInfections}");
        }
    }


    public class ComputerCluster
    {
        public int NumberOfInfections { get; set; }
        public Dictionary<Node, InfectionState> Nodes;
        public Node CurrentNode { get; set; }

        public Direction MoveDir { get; set; }
        public ComputerCluster(string[] input)
        {
            InitCluster(input);
        }

        public void InitCluster(string[] input)
        {
            Nodes = new Dictionary<Node, InfectionState>();
            MoveDir = Direction.Up;
            NumberOfInfections = 0;
            for (int row = 0; row < input.Length; row++)
            {
                if (input[row].IndexOf('#') > -1)
                {
                    for (int kol = 0; kol < input[row].Length; kol++)
                    {
                        if (input[row][kol].Equals('#'))
                        {
                            Nodes.Add(new Node { X = kol, Y = row }, InfectionState.Infected);
                        }
                    }
                }
            }
            CurrentNode = new Node { X = input[0].Length / 2, Y = input.Length / 2 };
        }

        public void Move()
        {

            switch (MoveDir)
            {
                case Direction.Up:
                    CurrentNode = new Node() { X = CurrentNode.X, Y = CurrentNode.Y - 1 };
                    break;
                case Direction.Right:
                    CurrentNode = new Node() { X = CurrentNode.X + 1, Y = CurrentNode.Y };
                    break;
                case Direction.Down:
                    CurrentNode = new Node() { X = CurrentNode.X, Y = CurrentNode.Y + 1 };
                    break;
                case Direction.Left:
                    CurrentNode = new Node() { X = CurrentNode.X - 1, Y = CurrentNode.Y };
                    break;
            }

        }

        public void RotateDirection(Direction direction)
        {
            int currentDir = (int)MoveDir;

            switch (direction)
            {
                case Direction.Left:
                    currentDir--;
                    break;
                case Direction.Right:
                    currentDir++;
                    break;
            }
            if (currentDir < 0)
                currentDir += 4;
            if (currentDir > 3)
                currentDir -= 4;

            MoveDir = (Direction)currentDir;
        }

    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }

        public InfectionState State { get; set; }

        public void IterateState()
        {
            int newState = (int)State;
            newState++;
            if (newState > 3)
                newState -= 4;

            State = (InfectionState)newState;
        }

        public Node()
        {
            X = 0;
            Y = 0;
            State = InfectionState.Clean;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Node))
            {
                return false;
            }

            var value = (Node)obj;
            return Node.Equals(this, value);
        }

        public static bool Equals(Node p1, Node p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public override int GetHashCode()
        {
            return ShiftAndWrap(X.GetHashCode(), 10) ^ Y.GetHashCode();
        }

        public int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            uint wrapped = number >> (32 - positions);
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }


    }
    public enum InfectionState
    {
        Clean = 0,
        Weakened = 1,
        Infected = 2,
        Flagged = 3
    }

    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}
