using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static SortedDictionary<string, int> registers = new SortedDictionary<string, int>();
        static void Main(string[] args)
        {
            var instructionLines = File.ReadAllLines(@".\Input.txt");
            List<Instruction> listInstructions = instructionLines.Select(p => new Instruction(p)).ToList();

            int maxValue = 0;

            listInstructions.Select(p => p.Register).Distinct().ToList().ForEach(R => registers.Add(R, 0));

            listInstructions.ForEach(i =>
            {
                var lhsValue = registers[i.Lhs];
                if(PerformCheck(lhsValue, i.Rhs, i.CompareOp))
                {
                    switch(i.Operation)
                    {
                        case "dec":
                            registers[i.Register] -= i.Value; 
                            break;
                        case "inc":

                            registers[i.Register] += i.Value;
                            break;
                        default:
                            Console.WriteLine("Fel plus/minus");
                            break;
                    }
                }
                if (registers[i.Register] > maxValue)
                    maxValue = registers[i.Register];

            });

            Console.WriteLine("Day 8: Assignment 1: The highest value is: " + registers.Select(p => p.Value).Max());
            Console.WriteLine("Day 8: Assignment 2: The highest value ever encountered is: " + maxValue);

        }
        
        static bool PerformCheck(int lhs, int rhs, string operation)
        {

            switch(operation)
            {
                case "!=":
                    return lhs != rhs;
                    
                case "==":
                    return lhs == rhs;
                case "<=":
                    return lhs <= rhs;

                case ">=":
                    return lhs >= rhs;

                case "<":
                    return lhs < rhs;

                case ">":
                    return lhs > rhs;
                default:
                    Console.WriteLine("Fel operand");
                    return false;
            }
        }
    }

    

    public class Instruction
    {
        public string Register { get; set; }
        public string Operation { get; set; }
        public int Value { get; set; }
        public string Lhs { get; set; }
        public string CompareOp { get; set; }
        public int Rhs { get; set; }
        public Instruction(string inputLine)
        {
            var temp = inputLine.Split(' ');
            Register = temp[0];
            Operation = temp[1];
            Value = int.Parse(temp[2]);
            Lhs = temp[4];
            CompareOp = temp[5];
            Rhs = int.Parse(temp[6]);
        }
    }
}
