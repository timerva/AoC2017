using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day16
{
    class Program
    {
        static char[] programOrder = "abcdefghijklmnop".ToCharArray();
        //static char[] programOrder = "abcde".ToCharArray();

        static void Main(string[] args)
        {
            var instructions = File.ReadAllText(@".\input.txt").Split(',');
            PerformInstructions(instructions);           
            var resultOne = string.Join(string.Empty, programOrder);
            Console.WriteLine($"Day 16 - Assignment 1: The order after the dance is: {resultOne}");


            //Eftersom det finns en begränsad mängd med kombinationer som kan uppstå i programordningen så letar 
            //jag upp när en program ordning återkommer igen i dansen. Man kan då sedan köra 1000000000 % antalet danser mellan mönster 
            // för att då inte behöva köra en biljon dancer (Max antal kombinationer är ju 16!

            var patternDetected = false;
            int nrOfDancesForPattern = 0;
            while (!patternDetected)
            {
                PerformInstructions(instructions);
                if (resultOne.Equals(string.Join(string.Empty, programOrder)))
                {
                    patternDetected = true;
                }
                nrOfDancesForPattern++;
            }

            //Eftersom vi redan har kört 1 steg så minskar vi resten med 1
            for (int i = 0; i < 1000000000 % nrOfDancesForPattern -1; i++)
            {
                PerformInstructions(instructions);
            }

            var resultTwo = string.Join(string.Empty, programOrder);
            Console.WriteLine($"Day 16 - Assignment 2: The order after the dance is: {resultTwo}");
        }

        private static void PerformInstructions(string[] instructions)
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                switch (instructions[i][0])
                {
                    case 's':
                    case 'S':
                        Spin(instructions[i]);
                        break;
                    case 'x':
                    case 'X':
                        SwapPos(instructions[i]);
                        break;
                    case 'p':
                    case 'P':
                        SwapPosByName(instructions[i]);
                        break;

                }
            }
        }

        private static void SwapPos(string v)
        {
            int[] swapPos = v.Substring(1).Split('/').Select(int.Parse).OrderBy(p => p).ToArray();
            var char1 = programOrder[swapPos[0]];
            var char2 = programOrder[swapPos[1]];

            programOrder[swapPos[0]] = char2;
            programOrder[swapPos[1]] = char1;

        }

        private static void SwapPosByName(string v)
        {
            var char1 = v[1];
            var char2 = v[3];
            int indexOf1 = Array.IndexOf(programOrder, char1);
            int indexOf2 = Array.IndexOf(programOrder, char2);
            programOrder[indexOf1] = char2;
            programOrder[indexOf2] = char1;
        }

        private static void Spin(string v)
        {
            int antalFlytt = int.Parse(v.Substring(1));
            char[] endArray = new char[programOrder.Length];
            Array.Copy(programOrder, programOrder.Length - antalFlytt, endArray, 0, antalFlytt);
            Array.Copy(programOrder, 0, endArray, antalFlytt , programOrder.Length - antalFlytt);
            programOrder = endArray;
        }
    }


}
