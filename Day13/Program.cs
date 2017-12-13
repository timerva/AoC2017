using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var fwLayers = File.ReadAllLines(@".\Input.txt")
                    .Select(line => line.Split(':'))
                    .Select(split => 
                                new Tuple<int, int>(int.Parse(split[0]), int.Parse(split[1]))
                           ).ToList();

            var firewall = new Firewall(fwLayers);

            
            int severity = 0;
            for (int packetPos = 0; packetPos <= firewall.MaxDepth; packetPos++)
            {
                if(firewall.Layers[packetPos].CheckIfCaught(packetPos))
                {
                    severity += packetPos * firewall.Layers[packetPos].RangeSize;
                }
            }

            Console.WriteLine($"Day 13 - Assignment 1: Severity is: {severity}");

            severity = 1;
            //Ingen ide att börja på noll då är alltid första upptagen
            int delay = 1;
            bool caught = true;
            while (caught)
            {
                caught = firewall.CheckFirewall(delay);
                if (caught)
                    delay++;
            }

            Console.WriteLine($"Day 13 - Assignment 2: Delay is: {delay}");

        }
    }

    public class Firewall
    {
        public List<FirewallLayer> Layers;
        public int MaxDepth { get; private set; }
        public Firewall(List<Tuple<int,int>> fwLayers)
        {
            MaxDepth = fwLayers.Max(fw => fw.Item1);
            Layers = new List<FirewallLayer>(MaxDepth);
            for (int i = 0; i <= MaxDepth; i++)
            {
                if (fwLayers.Any(fw => fw.Item1 == i))
                {
                    Layers.Add(new FirewallLayer(fwLayers.Single(fw => fw.Item1 == i)));
                }
                else
                {
                    Layers.Add(new FirewallLayer(i, 0));
                }
            }
        }

        public bool CheckFirewall(int delay)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                if (Layers[i].CheckIfCaught(i + delay))
                    return true;
            }
            return false;
        }
    }

    public class FirewallLayer
    {
        public int ScannerInFirstPos { get; private set; }
        public bool CheckIfCaught(int pos)
        {
            return (RangeSize > 0) && (pos % ScannerInFirstPos == 0); 
        }

        public readonly int RangeSize;
        public readonly int Depth;
        
        public FirewallLayer(Tuple<int, int> fwLayer) : this(fwLayer.Item1, fwLayer.Item2)
        { }
        
        public FirewallLayer(int layerDepth, int layerRange)
        {
            RangeSize = layerRange;
            Depth = layerDepth;
            if (RangeSize > 0)
            {
                ScannerInFirstPos = (RangeSize - 1) * 2;
            }
        }
    }
}
