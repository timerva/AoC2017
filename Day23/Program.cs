using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadLines(@".\input.txt").Select(p => p.Split(' ')).Select(p => new Instruction { Type = (InstructionType)Enum.Parse(typeof(InstructionType), p[0]), Register = p[1], Action = p.Length == 3 ? p[2] : string.Empty, Executed = false }).ToArray();

            var pgm = new Pgm(instructions);
            while (true)
            {
                ProcessInstruction(pgm);
                if (pgm.InstructionPos >= instructions.Length)
                    break;
            }
            Console.WriteLine($"Day 23 - Assignment 1: The number of mul is: {pgm.NumberOfMul}");
            Console.WriteLine(RunPartTwo());
          //  Console.WriteLine($"Day 23 - Assignment 2: The value of the H register is: {Part2()}");
        }

        static int RunPartTwo()
        {
            int a = 1, b = 57, c, d, e, f, g, h=0;
            c = b;
            if (a != 0)
            {
                b = (b * 100) - 100000;
                c = b + 17000;
            }

            while(true)
            {
                f = 1;
                d = 2;
                do
                {
                    e = 2;
                    do
                    {
                        g = (d * e) - b;
                        if (g != 0)
                        {
                            f = 0;
                        }
                        e++;
                        g = e - b;
                    } while (g != 0);

                    d++;
                    g = d - b;
                } while (g != 0);
                if (f == 0)
                    h++;
                g = b - c;
                if (g == 0)
                    return h;
                b -= 17;
            }

        }

        static void ProcessInstruction(Pgm pgm)
        {
            var instruction = pgm.Instructions[pgm.InstructionPos];
            var registers = pgm.Registers;
            instruction.Executed = false;
            if (!instruction.Register.IsNumeric() && !registers.ContainsKey(instruction.Register))
            {
                registers.Add(instruction.Register, 0);
            }
            long? actionValue = 0;
            if (instruction.Action.IsNumeric())
                actionValue = int.Parse(instruction.Action);
            else
            {
                if (string.IsNullOrEmpty(instruction.Action))
                { actionValue = null; }
                else
                {
                    actionValue = registers[instruction.Action];
                }
            }

            switch (instruction.Type)
            {
                case InstructionType.sub:
                    registers[instruction.Register] -= actionValue.Value;
                    break;
                case InstructionType.jnz:
                    if ((instruction.Register.IsNumeric() && int.Parse(instruction.Register) != 0) || registers[instruction.Register] != 0)
                    {
                        pgm.InstructionPos += actionValue.Value;
                        return;
                    }
                    break;
                case InstructionType.mul:
                    pgm.NumberOfMul++;
                    registers[instruction.Register] *= actionValue.Value;

                    break;
                case InstructionType.set:
                    registers[instruction.Register] = actionValue.Value;
                    break;
            }
            pgm.InstructionPos++;
        }
    }

    public static class StringExt
    {
        public static bool IsNumeric(this string text)
        {
            return double.TryParse(text, out double test);
        }
    }

    public class Pgm
    {
        public Dictionary<string, long> Registers;
        public Instruction[] Instructions;

        public long InstructionPos = 0;
        public long NumberOfMul = 0;
        public Pgm(Instruction[] instructions)
        {
            Registers = new Dictionary<string, long>();
            Instructions = instructions;
        }
    }


    public class Instruction
    {
        public InstructionType Type { get; set; }
        public string Register { get; set; }
        public string Action { get; set; }
        public bool Executed { get; set; }
    }


    public enum InstructionType
    {
        set,
        sub,
        mul,
        jnz
    }
}
