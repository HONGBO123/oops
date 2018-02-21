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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bst_number_list
{
    /*
     * Abstract Binary Tree Class:
     *      Any derived class must override the following methods.
     */ 
    public abstract class BinTree<T>
    {
        public abstract void Insert(T val);
        public abstract bool Contains(T val);
        public abstract void InOrder();
        public abstract void PreOrder();
        public abstract void PostOrder();
    }
}
