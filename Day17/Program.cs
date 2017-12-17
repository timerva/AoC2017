using System;
using System.Collections.Generic;

namespace Day17
{
    public class Program
    {
        static LinkedList<int> circularBuffer = new LinkedList<int>();
        static void Main(string[] args)
        {
            int input = 335;
            //input = 3;
            var currentNode = circularBuffer.AddFirst(0);
            
            for (int i = 1; i <= 2017; i++)
            {
                for (int j = 0; j < input; j++)
                {
                    currentNode = currentNode.Next ?? currentNode.List.First;
                }
                currentNode = circularBuffer.AddAfter(currentNode, i);
            }
            var resultNode = currentNode.Next ?? circularBuffer.First;
            Console.WriteLine($"Day 17 - Assignment 1: The value of the node next to the last inserted is: {resultNode.Value}");



            int result2 = -1;
            int nextPos = 1;
            for (int i = 1; i <= 50000000; i++)
            {
                //Räkna fram vilken pos detta egentligen är, behöver inte den länkade listan i detta fall då 
                //jag inte behöver någon mer pos än  [1] då 0-an alltid ligger först.
                var pos  = ((input + nextPos) % i);
                if(pos == 0)
                {
                    //Eftersom pos är 0 så är det alltså detta i-vörde det som just nu ligger direkt efter 0
                    result2 = i;
                }
                nextPos = pos + 1;
            }
            Console.WriteLine($"Day 17 - Assignment 2: The value of the node next to 0 is: {result2}");

        }


    }

    public static class Ext
    {
        public static int IndexOf<T>(this LinkedList<T> list, T item)
        {
            var count = 0;
            for (var node = list.First; node != null; node = node.Next, count++)
            {
                if (item.Equals(node.Value))
                    return count;
            }
            return -1;
        }
    }
}
