// Kyler Little
// 11472421


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort
{
    class RandomIntListGenerator
    {
        /*
            * Fill:
            *      Fill the items of numberList with random integers in range: [lowerBound, upperBound].
            *      Use Random.Next(lowerBound, upperBound) method to generate random int in specific range.
            */
        public static void Fill(ref List<int> numberList, int lowerBound, int upperBound)
        {

            Random rnd = new Random();

            for (int ctr = 1; ctr <= numberList.Capacity; ctr++)
            {
                numberList.Add(rnd.Next(lowerBound, upperBound));
            }
        }
    }
}
