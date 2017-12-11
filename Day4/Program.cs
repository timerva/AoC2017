using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var passPhrases = File.ReadAllLines(@".\Input.txt");
            var regexOnlyLowerCaseAndSpace = new Regex(@"^[a-z\s]+$");
            int nrOfOkPassphrases = 0;
            for (int i = 0; i < passPhrases.Count(); i++)
            {
                if (regexOnlyLowerCaseAndSpace.IsMatch(passPhrases[i]))
                {
                    var words = passPhrases[i].Split(' ');
                    var distinctWord = words.Distinct();
                    if (words.Count() == distinctWord.Count())
                    {
                        nrOfOkPassphrases++;
                    }
                }
            }
            Console.WriteLine($"Day4 - Assignment 1: The number of ok passphrases are: {nrOfOkPassphrases}");

            nrOfOkPassphrases = 0;
            for (int i = 0; i < passPhrases.Count(); i++)
            {
                if (regexOnlyLowerCaseAndSpace.IsMatch(passPhrases[i]))
                {
                    var words = passPhrases[i].Split(' ');
                    //Eftersom vi nu letar efter anagram kan man jämför orden när bokstäverna i orden är sorterade i bokstavsordning
                    var distinctWord = words.Select(p => SortCharsInString(p)).Distinct();
                    if (words.Count() == distinctWord.Count())
                    {
                        nrOfOkPassphrases++;
                    }
                }
            }

            Console.WriteLine($"Day4 - Assignment 2: The number of ok passphrases are: {nrOfOkPassphrases}");
        }

        static string SortCharsInString(string input)
        {
            return new string(input.ToCharArray().OrderBy(p => p).ToArray());
        }
    }
}
