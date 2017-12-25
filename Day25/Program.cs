using System;
using System.Collections.Generic;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            TurningMachine tm = new TurningMachine(12994925);
            int checksum = tm.Run();
            Console.WriteLine($"Day 25 - Assignment 1: The checksum is: {checksum}");
        }
    }

    public class TurningMachine
    {
        private LinkedList<int> Tape;
        private LinkedListNode<int> CurrentPos;
        public States NextState { get; set; }
        public long Step { get; set; }
        public long RunNrOfSteps { get; set; }
        public TurningMachine(int numberOfSteps)
        {
            Tape = new LinkedList<int>();
            Tape.AddFirst(0);
            NextState = States.A;
            Step = 0;
            CurrentPos = Tape.First;
            RunNrOfSteps = numberOfSteps;
        }

        public int Run()
        {
            while(Step < RunNrOfSteps)
            {
                switch (NextState)
                {
                    case States.A:
                        PerformA();
                        break;
                    case States.B:
                        PerformB();
                        break;
                    case States.C:
                        PerformC();
                        break;
                    case States.D:
                        PerformD();
                        break;
                    case States.E:
                        PerformE();
                        break;
                    case States.F:
                        PerformF();
                        break;
                }
                Step++;
            }
            return Tape.Count(t => t == 1); 
        }

        public void PerformA()
        {
            if (CurrentPos.Value == 0)
            {
                CurrentPos.Value = 1;
                MoveRight();
                NextState = States.B;
            }
            else
            {
                CurrentPos.Value = 0;
                MoveLeft();
                NextState = States.F;
            }
        }
        public void PerformB()
        {
            if (CurrentPos.Value == 0)
            {
                //CurrentPos.Value = 1;
                MoveRight();
                NextState = States.C;
            }
            else
            {
                CurrentPos.Value = 0;
                MoveRight();
                NextState = States.D;
            }
        }
        public void PerformC()
        {
            if (CurrentPos.Value == 0)
            {
                CurrentPos.Value = 1;
                MoveLeft();
                NextState = States.D;
            }
            else
            {
                //CurrentPos.Value = 0;
                MoveRight();
                NextState = States.E;
            }
        }
        public void PerformD()
        {
            if (CurrentPos.Value == 0)
            {
                //CurrentPos.Value = 1;
                MoveLeft();
                NextState = States.E;
            }
            else
            {
                CurrentPos.Value = 0;
                MoveLeft();
                NextState = States.D;
            }
        }
        public void PerformE()
        {
            if (CurrentPos.Value == 0)
            {
                //CurrentPos.Value = 1;
                MoveRight();
                NextState = States.A;

            }
            else
            {
                //CurrentPos.Value = 0;
                MoveRight();
                NextState = States.C;
            }
        }
        public void PerformF()
        {
            if (CurrentPos.Value == 0)
            {
                CurrentPos.Value = 1;
                MoveLeft();
                NextState = States.A;
            }
            else
            {
                //CurrentPos.Value = 0;
                MoveRight();
                NextState = States.A;
            }
        }

        private void MoveRight()
        {
            if (CurrentPos.Next == null)
                Tape.AddLast(0);
            CurrentPos = CurrentPos.Next;
        }

        private void MoveLeft()
        {
            if (CurrentPos.Previous == null)
                Tape.AddFirst(0);
            CurrentPos = CurrentPos.Previous;
        }
        
        public enum States
        {
            A,
            B,
            C,
            D,
            E,
            F
        }

    }
}
