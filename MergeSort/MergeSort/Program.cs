/*
 * 
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort
{
    class Program
    {
        /// <summary>
        /// Debugging function to check if list is sorted
        /// </summary>
        /// <param name="list"></param>
        static void printList(List<int> list)
        {
            foreach (int num in list)
            {
                Console.Write(num.ToString() + " ");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            // Print starting message
            Console.WriteLine("Starting tests for MergeSort vs Threaded MergeSort.");

            // Declare variables needed
            List<int> listLengths = new List<int> { 8, 64, 256, 1024 };
            long startTime = 0, endTime = 0;
            foreach (int length in listLengths)
            {
                // 1. Create the randomized list
                List<int> toBeSorted = new List<int>(length);                   // create list with initial capacity 'length'
                RandomIntListGenerator.Fill(ref toBeSorted, 0, 1000);           // randomly fill list with values from 0 to 1000
                List<int> toBeSorted2 = new List<int>(toBeSorted);              // create identical copy

                // 2. Test the different sorting methods for each 'length'
                    // declare sorting objects
                MergeSort<int> mergeSort = new MergeSort<int>();
                ThreadedMergeSort<int> threadedMergeSort = new ThreadedMergeSort<int>();
                    // print starting message
                Console.WriteLine(String.Format("Sorting for size {0}.", length));
                    // time the sort
                startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                mergeSort.Sort(toBeSorted);
                endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    // print off the sorting's time
                Console.WriteLine(String.Format("\tNormal Sort Time: {0} ms", endTime - startTime));
                    // print list for debugging to see if it's actually sorted
                //printList(toBeSorted);
                    // time the threaded sort
                startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                threadedMergeSort.Sort(toBeSorted2);
                endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    // print off the threaded sorting's time
                Console.WriteLine(String.Format("\tThreaded Sort Time: {0} ms", endTime - startTime));
                    // print list for debugging to see if it's actually sorted
                //printList(toBeSorted2);
            }
        }
    }
}
