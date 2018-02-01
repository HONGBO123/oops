using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    class UniqueCounter
    {
        /*
         * UsingDict:
         *      The algorithm is very straightforward. Hash every unique number from 'numberList' to 
         *      an internal dictionary. By 'unique,' I mean that if the number is already present
         *      in the dictionary, then it will not be added to the dictionary. Therefore, every item
         *      in the dictionary is unique.
         *      At the end, we can simply return the number of key/value pairs in the dictionary since
         *      this represents the number of unique items.
         */
        public static int UsingDict(ref List<int> numberList)
        {
            // Step #1: create dictionary with capacity of the number of items in numberList. By default, Dictionary uses
            //          linked lists for the internal collision handling mechanism. Thus, if I given an initial capacity of
            //          numberList.Count, then the load factor will never exceed 1, and I won't have to rehash.
            Dictionary<int, int> dict = new Dictionary<int, int>(numberList.Count);

            // Step #2: Hash each value in numberList to dict, so long as value isn't already in dict.
            foreach (int num in numberList)
            {
                if (!dict.ContainsValue(num))
                {
                    dict.Add(num, num);
                }
            }
            
            // Step #3: Return the count.
            return dict.Count;
        }

        /*
         * UsingBruteForce:
         *      The algorithm for this is quite straightforward. For each item in the list, check to see if any of the 
         *      remaining items in the list are the same. If we find a duplicate, then break out of the inner loop
         *      and do NOT increment our 'unique count.' If we manage to make it through the rest of the list
         *      without finding a duplicate, then we increment our 'unique count.' 
         */ 
        public static int UsingBruteForce(ref List<int> numberList)
        {
            int numUnique = 0;
            for (int i = 0; i < numberList.Count; ++i)
            {
                bool numIsUnique = true;         // assume uniqueness to start
                for (int j = i + 1; j < numberList.Count && numIsUnique; ++j)
                {
                    if (numberList[i] == numberList[j])       // not unique
                    {
                        numIsUnique = false;
                    }
                }
                if (numIsUnique) ++numUnique;
            }
            return numUnique;
        }

        /*
         * UsingSortAndCount:
         *      The algorithm works as follows:
         *      First, we sort the list (obviously).
         *      Next, we walk through the list once. For each value encountered, we check to see if we've encountered it before.
         *      If we have encountered it before, then it must have happened the last iteration since the list is sorted. If we
         *      haven't encountered it, add one to our 'unique number count' and set the last encountered number (currentNum in 
         *      the code below) to the number currently being examined.
         */ 
        public static int UsingSortAndCount(ref List<int> numberList)
        {
            // Step #1: sort the list
            numberList.Sort();

            // Step #2: count the number of unique numbers in the list
            //          Note that the initial value of currentNum is guaranteed to not occur in the list because it's
            //          below the lower bound of values in the list.
            int numUnique = 0, currentNum = Constants.listLowerBound - 1;
            foreach (int num in numberList)
            {
                if (num != currentNum)
                {
                    ++numUnique;
                    currentNum = num;
                }
            }

            return numUnique;
        }

        public static string UsingDictExplanation()
        {
            return "Explanation: The algorithm is very straightforward. Hash every unique number from 'numberList' to an internal dictionary. By 'unique,' " +
                "I mean that if the number is already present in the dictionary, then it will not be added to the dictionary. Therefore, every item " +
                "in the dictionary is unique. At the end, we can simply return the number of key/value pairs in the dictionary since this represents " +
                "the number of unique items." + Environment.NewLine +
                "Time Complexity: The algorithm has O(N) time complexity. We run through the list once. For each element in the list, we perform two O(1) " +
                "operations. In the absolute worst case (EXTREMELY unlikely), these O(1) operations take O(k) time where k is the number of elements added " +
                "so far. This only occurs if every element hashes to the same bucket. In this EXTREMELY rare situation, we repeat the O(k) operations N times, " +
                "making the time complexity O(N^2) by a simple geometric series calculation. But, we don't really need to worry about it... " +
                "In the end, we simply return Dictionary.Count, an O(1) operation. This makes the total time complexity O(N).";
        }
    }
}
