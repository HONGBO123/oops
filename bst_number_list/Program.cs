/*
 * Assignment #4: Abstract Tree in Console
 * Author: Kyler Little
 * Last Updated: 2/20/2018
 * Description:
 *      This program asks the user to enter space-separated numbers between 0 and 100.
 *      The program will then insert the values into a binary search tree (which inherits
 *      from an abstract binary tree class) and prints off various interesting statistics
 *      about the tree. For instance, the program will "sort" the numbers by doing an
 *      inorder traversal.
 * 
 */ 




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
         * ParseToIntArray: accepts a string of intergers as input formatted like "Num1 Num2 Num3 ..."
         *                  if any of the numbers cannot be converted due to overflow or formatting, then 'null' is returned and
         *                  a helpful error message is printed.
         */
        public static int[] ParseToIntArray(string line)
        {
            string[] tokens = line.Split(' ');         // Split line based on whitespace delimiter.
            int[] arr = new int[tokens.Length];          // allocate new array based on number of tokens (numbers)
            for (int i = 0; i < arr.Length; ++i)
            {
                try
                {
                    int number = int.Parse(tokens[i]);      // convert number to int; throws exception if error
                    arr[i] = number;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Unable to convert '{0}'.", tokens[i]);
                    return null;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("'{0}' is out of range of the Int32 type.", tokens[i]);
                    return null;
                }
                catch
                {
                    Console.WriteLine("'{0}' is an invalid input.", tokens[i]);
                }
            }
            return arr;   
        }
    }

    class BSTProgram
    {
        static void Main(string[] args)
        {
            BST<int> binarySearchTree = null;      // declare BST

            Console.WriteLine("Enter a collection of numbers in the range [0, 100], separated by spaces:");
            int[] numberList = StringParser.ParseToIntArray(Console.ReadLine());          // process user input
            if (numberList != null)          // if no errors occurred, create a BST
            {
                binarySearchTree = new BST<int>(numberList);         // create BST; ignore duplicates
            }
            binarySearchTree.InOrder();
            binarySearchTree.PrintTreeStatistics();
            Console.WriteLine("Done.");
        }
    }
}
