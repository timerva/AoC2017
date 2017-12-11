using System;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Assignment1();
            Assignment2();
        }

        static void Assignment2()
        {
            var listSize = 256;
            byte[] standardSuffix = new byte[5] { 17, 31, 73, 47, 23 };
            var input = System.Text.Encoding.ASCII.GetBytes(File.ReadAllText(@".\input.txt")).Concat(standardSuffix).ToArray();            

            int[] hashvalueList = new int[listSize];
            for (int i = 0; i < listSize; i++)
            {
                hashvalueList[i] = i;
            }
            //Loop 64 ggr
            int currentPos = 0, skipSize = 0;
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    hashvalueList = ReverseCircularArray(hashvalueList, currentPos, input[j]);
                    currentPos = (currentPos + input[j] + skipSize) % 256;
                    skipSize++;
                }
            }

            byte[] sparseHash = GetSparseHash(hashvalueList);
            string denseHash = BitConverter.ToString(sparseHash).Replace("-","");
            Console.WriteLine($"Day 10: Assignment 2: The Knot Hash is: : {denseHash}");
        }
        

        static void Assignment1()
        {
            var listSize = 256;
            var totalInput = File.ReadAllText(@".\input.txt").Split(',').Select(int.Parse).ToArray();
            int currentPos = 0, skipSize = 0;
            int[] list = new int[listSize];
            for (int i = 0; i < listSize; i++)
            {
                list[i] = i;
            }

            for (int j = 0; j < totalInput.Length; j++)
            {
                list = ReverseCircularArray(list, currentPos, totalInput[j]);
                currentPos = (currentPos + totalInput[j] + skipSize) % 256;
                skipSize++;
            }
            Console.WriteLine($"Day 10: Assignment 1: The checksum of the two first entries in the list is: {list[0] * list[1]}");

        }

        static int[] ReverseCircularArray(int[] array, int index, int length)
        {
            int[] newArr = new int[array.Length];
            int[] reverseArr = new int[length];
            array.CopyTo(newArr, 0);
            for (int i = 0; i < length; i++)
            {
                int pos = i + index;
                if (pos >= array.Length)
                    pos -= array.Length;
                reverseArr[i] = array[pos];
            }
            Array.Reverse(reverseArr);
            for (int i = 0; i < length; i++)
            {
                int pos = i + index;
                if (pos >= array.Length)
                    pos -= array.Length;
                newArr[pos] = reverseArr[i];
            }
            return newArr;
        }

        static byte[] GetSparseHash(int[] arr)
        {
            byte[] resultArray = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                int result = arr[i * 16];
                for (int j = i * 16 + 1; j < (i + 1) * 16; j++)
                {
                    result ^= arr[j];
                }
                resultArray[i] = (byte)result;
            }
            return resultArray;
        }

    }
}
