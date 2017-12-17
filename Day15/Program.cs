using System;
using System.Collections;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            long A = 634;
            long B = 301;
            //A = 65;
            //B = 8921;
            int nrOfMatches = 0;
            for (int i = 0; i < 40000000; i++)
            {

                A = Calculate(A, 16807);
                B = Calculate(B, 48271);
                if (GetLowest16Bits(A) == GetLowest16Bits(B))
                    nrOfMatches++;
            }

            Console.WriteLine($"Day 15: Assignment 1: The number of macthes are: {nrOfMatches}");

            A = 634;
            B = 301;
            nrOfMatches = 0;

            for (int i = 0; i < 5000000; i++)
            {

                long temp = 1;
                while(temp % 4 != 0)
                {
                    
                    A = Calculate(A, 16807);
                    temp = A;
                }

                temp = 1;
                while (temp % 8 != 0)
                {
                    B = Calculate(B, 48271);
                    temp = B;
                }
                if (GetLowest16Bits(A) == GetLowest16Bits(B))
                    nrOfMatches++;
            }

            Console.WriteLine($"Day 15: Assignment 2: The number of macthes are: {nrOfMatches}");

        }

        static long Calculate(long value, int factor)
        {
            return (value * factor) % 2147483647;
        }



        static int GetLowest16Bits(long value)
        {
            return (int) (value & 0xffff);
        }
    }
}
