using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    class RandomIntListGenerator
    {
        public static void Generate(ref List<int> numberList, int lowerBound, int upperBound)
        {
            // Fill the items of numberList with random integers in range: [lowerBound, upperBound].
                // Use Random.Next(lowerBound, upperBound) method to generate random int in specific range.
            Random rnd = new Random();

            for (int ctr = 1; ctr <= numberList.Capacity; ctr++)
            {
                numberList.Add(rnd.Next(lowerBound, upperBound));
                Console.Write("{0,6}", numberList.Last());
                if (ctr % 10 == 0) Console.WriteLine();    // print new line every 10 numbers
            }

        }
    }
}
