using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Day7
{
    class Program
    {
        static List<Pgm> tree = new List<Pgm>();
        
        static void Main(string[] args)
        {   
            var lines = File.ReadAllLines(@".\input.txt");
            foreach (var line in lines)
            {
                var childrenSplit = line.Split("->").Select(p => p.Trim()).ToArray();
                var nameWeightSplit = childrenSplit[0].Split(' ').Select(p => p.Trim().Replace("(", "").Replace(")", "")).ToArray();
                Pgm pgm = tree.SingleOrDefault(p => p.Name == nameWeightSplit[0]);
                if(pgm == null)
                {
                    pgm = new Pgm(nameWeightSplit[0], int.Parse(nameWeightSplit[1]));
                    tree.Add(pgm);
                }
                if(childrenSplit.Count() == 2)
                {
                    var childrens = childrenSplit[1].Replace(" ", "").Split(',');
                    pgm.ChildrensNames.AddRange(childrens);
                }
            }

            var parents = tree.Where(p => p.ChildrensNames.Count > 0).ToList();
            foreach(var p in parents)
            {
                foreach(var child in p.ChildrensNames)
                {
                    var ch = tree.Single(c => c.Name == child);
                    ch.ParentName = p.Name;
                    ch.Parent = p;
                    p.Childrens.Add(ch);
                }
            }

            var topPgm = tree.Single(p => p.ParentName == p.Name);
            GetWeight(topPgm);

            Console.WriteLine($"Day 7: Assingment 1:  The bottom program name is: {topPgm.Name}");
            FindUnbalanced(topPgm, 0);

        }

        static void FindUnbalanced(Pgm pgm, int diff)
        {
            var child = pgm.Childrens.GroupBy(c => c.TotalWeight).Select(p => new { Count = p.Count(),Weight = p.Key, Pgms = p.ToList() }).OrderByDescending(c => c.Count).ToList();
            Pgm unbalancedChild = null;
            if (child.Count() > 1)
            {
                unbalancedChild = child.Where(p => p.Count == 1).Select(p => p.Pgms.First()).Single();
                //Console.WriteLine($"{child[0].Count} : {child[0].Weight}");
                //Console.WriteLine($"{child[1].Count} : {child[1].Weight}");
                int newDiff = child[0].Weight - child[1].Weight;
                //Console.WriteLine($"{unbalancedChild.Name} {unbalancedChild.TotalWeight} Diff: {newDiff}");
                FindUnbalanced(unbalancedChild, newDiff);
                
            }
            else
            {
                Console.WriteLine("Day 7: Assignment two:");
                Console.WriteLine($"Pgm: {pgm.Name} with currentweigt: {pgm.Weight} needs to be: {pgm.Weight + diff}");
                //foreach (var item in child)
                //{
                //    Console.WriteLine($"{item.Count} : {item.Weight}");
                //}
            }
        }

        static void PrintProgram(Pgm pgm, int depth)
        {
            string tabString = new String(' ', depth);
            Console.Write(tabString);
            Console.WriteLine($"{pgm.Name} ({pgm.Weight}) (CW={pgm.ChildWeight}) (TOTAL={pgm.TotalWeight})");
            foreach(var c in pgm.ChildrensNames)
            {
                Console.Write(tabString +"-> ");
                PrintProgram(tree.Single(p=>p.Name == c), depth + 1);
            }
        }

        static int GetWeight(Pgm pgm)
        {
            int childWeight = 0;
            foreach(var p in pgm.ChildrensNames)
            {
                childWeight += GetWeight(tree.Single(c => c.Name == p));
            }
            pgm.ChildWeight = childWeight;
            return pgm.TotalWeight;
        }
    }

    public class Pgm
    {
        public Pgm(string name, int weight)
        {
            Name = name;
            Weight = weight;
            ParentName = name;
            Parent = null;
            Childrens = new List<Pgm>();
            ChildrensNames = new List<string>();
            ChildWeight = 0;
        }
                
        public Pgm Clone()
        {
            return (Pgm)this.MemberwiseClone();
        }

        public List<Pgm> Childrens { get; set; }
        public Pgm Parent { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }

        public int TotalWeight { get { return ChildWeight + Weight; } }

        public int ChildWeight { get; set; }
        public List<string> ChildrensNames { get; set; }
        public string ParentName { get; set; }
    }
}
