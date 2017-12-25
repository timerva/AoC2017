using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@".\input.txt");
            var components = lines.Select(s => s.Split('/')).Select(s => new Component(int.Parse(s[0]), int.Parse(s[1]))).ToImmutableList();
            Console.WriteLine($"Day 24 - Assignment 1: The strongest bridge has strength: {BuildBridgesPart1(components)}");
            var resultPart2 = BuildBridgesPart2(components);
            Console.WriteLine($"Day 24 - Assignment 2: The longest bridges is: {resultPart2.Item2} with strength: {resultPart2.Item1}");
        }

        static int BuildBridgesPart1(ImmutableList<Component> components, int connectValue = 0, int strength = 0)
        {
            var possibleNextComponents = components.Where(c => c.End1 == connectValue || c.End2 == connectValue).ToImmutableList();
            if (possibleNextComponents.Count() == 0)
                return strength;
            List<int> bridges = new List<int>();

            foreach (var nextCompontent in possibleNextComponents)
            {
                int bridgeStrength = strength + nextCompontent.End1 + nextCompontent.End2;
                int newConnectValue = 0;
                if (nextCompontent.End1 == connectValue)
                    newConnectValue = nextCompontent.End2;
                else
                    newConnectValue = nextCompontent.End1;

                var componentsLeft = components.Remove(nextCompontent);
                bridges.Add(BuildBridgesPart1(componentsLeft, newConnectValue, bridgeStrength));
            }
            return bridges.Max();   
        }

        static Tuple<int,int> BuildBridgesPart2(ImmutableList<Component> components, int connectValue = 0, int strength = 0, int length = 0)
        {
            var possibleNextComponents = components.Where(c => c.End1 == connectValue || c.End2 == connectValue).ToImmutableList();
            if (possibleNextComponents.Count() == 0)
                return new Tuple<int, int>(strength, length);
            List<Tuple<int,int>> bridges = new List<Tuple<int,int>>();

            foreach (var nextCompontent in possibleNextComponents)
            {
                int bridgeStrength = strength + nextCompontent.End1 + nextCompontent.End2;
                int bridgeLenght = length + 1;
                int newConnectValue = 0;
                if (nextCompontent.End1 == connectValue)
                    newConnectValue = nextCompontent.End2;
                else
                    newConnectValue = nextCompontent.End1;

                var componentsLeft = components.Remove(nextCompontent);
                bridges.Add(BuildBridgesPart2(componentsLeft, newConnectValue, bridgeStrength, bridgeLenght));
            }
            return bridges.OrderByDescending(b => b.Item2).ThenByDescending(b => b.Item1).First();

        }
    }

    public class Component
    {
        public int End1 { get; set; }
        public int End2 { get; set; }
        public bool Connected { get; set; }
        public Component(int end1, int end2)
        {
            End1 = end1;
            End2 = end2;
            Connected = false;
        }
    }
}
