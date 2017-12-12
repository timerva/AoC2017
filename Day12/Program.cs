using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static List<Pgm> programs = new List<Pgm>();

        static void Main(string[] args)
        {


            var inputNodes = File.ReadAllLines(@".\input.txt").Select(s => s.Replace(" ", "")).ToList();
            
            for (int i = 0; i < inputNodes.Count; i++)
            {
                var tempSplit = inputNodes[i].Split(new string[] {"<->", "," }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => int.Parse(s))
                                .ToList();

                var pgm = new Pgm(tempSplit[0]);
                pgm.Neighbours.AddRange(tempSplit.GetRange(1, tempSplit.Count - 1));
                programs.Add(pgm); 
            }

            TraverseChildrens(0);
            Console.WriteLine($"Day 12 - Assignment 1: The number of programs in this group is: {programs.Count(p => p.Visited)}");

            //Eftersom uppgift ett redan har gjort grupp 1
            int numberOfGroups = 1;
            while(programs.Any(p => !p.Visited))
            {
                numberOfGroups++;
                TraverseChildrens(programs.First(p => p.Visited == false).Id);
            }
            Console.WriteLine($"Day 12 - Assignment 2: The number of groups is: {numberOfGroups}");

        }

        static void TraverseChildrens(int id)
        {
            var pgm = programs.Single(p => p.Id == id);
            pgm.Visited = true;
            foreach(var child in programs.Where( p => pgm.Neighbours.Contains(p.Id) && !p.Visited ).Select(p=>p.Id))
            {
                TraverseChildrens(child);
            }
        }

        internal class Pgm
        {
            internal int Id;
            internal bool Visited;
            internal List<int> Neighbours { get; private set; }
            internal Pgm(int id, bool visited = false)
            {
                Id = id;
                Neighbours = new List<int>();
                Visited = visited;
            }
        }

    }    



}
