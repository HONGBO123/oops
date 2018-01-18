using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bst_number_list
{
    static class StringParser
    {
        /*
         * ParseToIntArray: 
         */
        public static int[] ParseToIntArray(string line)
        {
            string[] tokens = line.Split(' ');
            int[] arr = new int[tokens.Length];
            for (int i = 0; i < arr.Length; ++i)
            {
                try
                {
                    int number = int.Parse(tokens[i]);
                    arr[i] = number;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Unable to convert '{0}'.", tokens[i]);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("'{0}' is out of range of the Int32 type.", tokens[i]);
                }
            }
            return arr;
        }
    }

    class BSTProgram
    {
        static void Main(string[] args)
        {
            BST binarySearchTree = null;
            do
            {
                Console.WriteLine("Enter a collection of numbers in the range [0, 100], separated by spaces:\n");
                int[] numberList = StringParser.ParseToIntArray(Console.ReadLine());
                binarySearchTree = new BST(numberList);
            } while (binarySearchTree != null);      // BST constructor returns null if user entered 0 numbers or number list with duplicates

            // 
            binarySearchTree.PrintInorder();
            

        }
    }
}
