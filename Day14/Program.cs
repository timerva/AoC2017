using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseName = "oundnydw";

            var nrOfSetBits = Assignment1(baseName);
            Console.WriteLine($"Day 14: Assignment 1: The number of bits set in the disk-grid is: {nrOfSetBits}");

            var nrOfRegions = Assignment2(baseName);
            Console.WriteLine($"Day 14: Assignment 2: The number of regions is: {nrOfRegions}");
        }
        static int Assignment1(string input)
        {
            var binaryHashes = GetBinaryHashes(input);
            var setBitsInhashes = 0;
            foreach (var bHash in binaryHashes)
            {
                setBitsInhashes += bHash.Count(bit => bit == '1');
            }
            return setBitsInhashes;
        }

        static string[] GetBinaryHashes(string input)
        {
            var hexHashes = Enumerable.Range(0, 128)
                                   .Select(i => $"{input}-{i}")
                                   .Select(Day10.Program.Assignment2);
            return hexHashes.Select(hash => string.Join(string.Empty, hash.Select(c => ConvertHexToBinary(c.ToString()))))
                                   .ToArray();
        }

        private static string ConvertHexToBinary(string hex)
        {
            return Convert.ToString(Convert.ToInt32(hex.ToString(), 16), 2).PadLeft(4, '0');

        }


        private static int Assignment2(string input)
        {
            //Detta är ju alla hashar som skapar, varje stägn är ju 128tecken antingen 0 eller 1
            //Sedan ör det ju bara att göra en grid som håller reda på vilka poas vi har besökt. 
            //För att se hur många regioner detta blir gör man nu en DFS sökning i bGrid med start i en 
            // punkt som har biten satt och som inte tidigare är besökt.
            //Eftersom det är en array jag skapa så loopar jag helt enkelt igenom alla posar...
            string[] binaryHashes = GetBinaryHashes(input);
            var binaryGrid = GenerateBinaryGrid(binaryHashes);

            int regions = 0;


            for (int j = 0; j < binaryGrid.GetLength(1); j++)
            {
                for (int x = 0; x < binaryGrid.GetLength(0); x++) // columns
                {
                    if (binaryGrid[x, j][1] || !binaryGrid[x, j][0])
                    {
                        continue;
                    }
                    TraverseGrid(x, j, binaryGrid);
                    regions++;
                }
            }
            return regions;
        }

        static bool[,][] GenerateBinaryGrid(string[] binaryHashes)
        {

            bool[,][] bGrid = new bool[binaryHashes[0].Length, binaryHashes.Length][];
            for (int j = 0; j < binaryHashes.Length; j++)
            {
                for (int i = 0; i < binaryHashes[j].Length; i++)
                {
                    bGrid[i, j] = new bool[2];
                    bGrid[i, j][0] = binaryHashes[j][i] == '1';
                }
            }
            return bGrid;
        }


        static void TraverseGrid(int posX, int PosY, bool[,][] binaryGrid)
        {
            binaryGrid[posX, PosY][1] = true;
            //Traversera grannar uppåt, nedåt, v'nster och höger
            if (posX > 0 && binaryGrid[posX - 1, PosY][0] && !binaryGrid[posX - 1, PosY][1])
                TraverseGrid(posX - 1, PosY, binaryGrid);
            if (posX < 127 && binaryGrid[posX + 1, PosY][0] && !binaryGrid[posX + 1, PosY][1])
                TraverseGrid(posX + 1, PosY, binaryGrid);
            if (PosY > 0 && binaryGrid[posX, PosY - 1][0] && !binaryGrid[posX, PosY - 1][1])
                TraverseGrid(posX, PosY - 1, binaryGrid);
            if (PosY < 127 && binaryGrid[posX, PosY + 1][0] && !binaryGrid[posX, PosY + 1][1])
                TraverseGrid(posX, PosY + 1, binaryGrid);
        }
    }
}
