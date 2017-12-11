using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "2 8 8 5 4 2 3 1 5 5 1 2 15 13 5 14".Split(' ').Select(Int32.Parse).ToArray();
            //var input = "0 2 7 0".Split(' ').Select(Int32.Parse).ToArray();
            MemoryBank mBank = new MemoryBank(input);
            List<MemoryBank> steps = new List<MemoryBank>
            {
                mBank
            };

            bool alreadyDone = false;
            var indexOfDuplicate = 0;
            while (!alreadyDone)
            {
                mBank = new MemoryBank(mBank.CloneArray());
                var currentPos = mBank.GetHighestPos();
                var startValue = mBank.Get(currentPos);
                mBank.SetArray(currentPos, 0);
                currentPos++;
                if(currentPos >= mBank.Length)
                {
                    currentPos = 0;
                }
                for (int i = 0; i < startValue; i++)
                {
                    mBank.SetArray(currentPos, mBank.Get(currentPos) + 1);
                    currentPos++;
                    if (currentPos >= mBank.Length)
                    {
                        currentPos = 0;
                    }
                }
                if (steps.Contains(mBank))
                {
                    indexOfDuplicate = steps.IndexOf(mBank);

                    alreadyDone = true;
                    break;

                }
                else
                {
                    steps.Add(mBank);
                }
            }
            
            Console.WriteLine($"Day 6: Assignment 1: It takes {steps.Count} steps");
            Console.WriteLine($"Day 6: Assignment 2: There are {steps.Count - indexOfDuplicate} steps in between");
        }
    }

    public class MemoryBank
    {
        private int _size;
        private int[] _array;
        public int Length {  get { return _size; } }
        public MemoryBank(int size)
        {
            _size = size;
            _array = new int[_size];
        }
        public int[] CloneArray()
        {
            return (int[])_array.Clone();
        }

        public MemoryBank(int[] arr)
        {

            _array = arr;
            _size = arr.Length;
        }

        public int GetHighestPos()
        {
            var maxValue = _array.Max();
            for (int i = 0; i < _size; i++)
            {
                if (_array[i] == maxValue)
                    return i;
            }
            return -1;
        }

        public void SetArray(int pos, int value)
        {
            _array[pos] = value;
        }
        public int Get(int pos)
        {
            return _array[pos];
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var rhs = obj as MemoryBank;
            if (rhs == null)
                return false;

            return this.GetHashCode() == rhs.GetHashCode();
        }

        public bool Equals(MemoryBank rhs)
        {
            if (rhs == null)
                return false;
            return this.GetHashCode() == rhs.GetHashCode();
        }

        public override int GetHashCode()
        {
            int checksum = _array.Length;
            for (int i = 0; i < _array.Length; ++i)
            {
                checksum = unchecked(checksum * 997 + _array[i]);
            }
            return checksum;
        }
    }
}
