using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {

        static void Main(string[] args)
        {
            var instructions = File.ReadLines(@".\input.txt").Select(p => p.Split(' ')).Select(p => new Instruction { Type = (InstructionType)Enum.Parse(typeof(InstructionType), p[0]), Register = p[1], Action = p.Length == 3 ? p[2] : string.Empty, Executed = false }).ToArray();
            var result1 = Assignment1(instructions);
            Console.WriteLine($"Day 18 - Assignment 1: The value of the recovered frequency is: {result1}");
            Console.WriteLine($"Day 18 - Assignment 2: The number of sent in program2 is: {Assignment2(instructions)}");
        }

        static long Assignment1(Instruction[] instructions)
        {
            bool endLoop = false;
            long recoveredFreq = 0;
            var pgm = new Pgm((Instruction[])instructions.Clone(), 0);

            while (!endLoop)
            {
                ProcessInstruction(pgm, 1);

                if (pgm.Instructions[pgm.InstructionPos].Type == InstructionType.rcv && pgm.Instructions[pgm.InstructionPos].Executed)
                {
                    recoveredFreq = pgm.Registers[instructions[pgm.InstructionPos].Register];
                    endLoop = true;
                }
            }
            return recoveredFreq;
        }


        static long Assignment2(Instruction[] instructions)
        {
            var pgm = new Pgm((Instruction[])instructions.Clone(), 0);
            var pgm2 = new Pgm((Instruction[])instructions.Clone(), 1);
            pgm.ReceiveQueue = pgm2.SendQueue;
            pgm2.ReceiveQueue = pgm.SendQueue;
            bool deadlock = false;
            while (!deadlock) 
            {
                ProcessInstruction(pgm, 2);
                ProcessInstruction(pgm2, 2);
                deadlock = pgm.DeadLock & pgm2.DeadLock;

            }
            return pgm2.NumberOfSend;
        }



        static void ProcessInstruction(Pgm pgm, int assignment)
        {
            pgm.DeadLock = false;
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
                case InstructionType.add:
                    registers[instruction.Register] += actionValue.Value;
                    break;
                case InstructionType.jgz:
                    if ((instruction.Register.IsNumeric() && int.Parse(instruction.Register) > 0) || registers[instruction.Register] > 0)
                    {
                        pgm.InstructionPos += actionValue.Value;
                        return;
                    }
                    break;
                case InstructionType.mod:
                    registers[instruction.Register] %= actionValue.Value;
                    break;
                case InstructionType.mul:
                    registers[instruction.Register] *= actionValue.Value;
                    break;
                case InstructionType.rcv:
                    if (assignment == 1)
                    {
                        if (registers[instruction.Register] != 0)
                        {
                            instruction.Executed = true;
                            registers[instruction.Register] = pgm.LastSound;
                        }
                    }
                    else
                    {
                        if (pgm.ReceiveQueue.Count > 0)
                        {
                            registers[instruction.Register] = pgm.ReceiveQueue.Dequeue();
                        }
                        else
                        {
                            pgm.DeadLock = true;
                            return;
                        }

                    }
                    break;
                case InstructionType.set:
                    registers[instruction.Register] = actionValue.Value;
                    break;
                case InstructionType.snd:
                    if (assignment == 1)
                    {
                        pgm.LastSound = registers[instruction.Register];
                    }
                    else
                    {
                        pgm.SendQueue.Enqueue(registers[instruction.Register]);
                        pgm.NumberOfSend++;
                    }
                    break;
            }
            pgm.InstructionPos++;
        }
    }



    public static class StringExt
    {
        public static bool IsNumeric(this string text)
        {
            double test;
            return double.TryParse(text, out test);
        }
    }

    public class Pgm
    {
        public long ProgramId { get; set; }
        public Dictionary<string, long> Registers;
        public Instruction[] Instructions;

        public Queue<long> SendQueue;
        public Queue<long> ReceiveQueue;

        public long InstructionPos = 0;
        public bool DeadLock;
        public long LastSound;
        public long NumberOfSend = 0;
        public Pgm(Instruction[] instructions, int programId)
        {
            Registers = new Dictionary<string, long>();
            Registers.Add("p", programId);
            ProgramId = programId;
            SendQueue = new Queue<long>();
            ReceiveQueue = new Queue<long>();
            Instructions = instructions;
            LastSound = -1;
            DeadLock = false;
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
        add,
        mul,
        mod,
        snd,
        rcv,
        jgz
    }
}
