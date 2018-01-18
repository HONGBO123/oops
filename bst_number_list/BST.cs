using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bst_number_list
{
    class Node
    {
        internal Node Left { get; set; }
        internal Node Right { get; set; }
        public static int Data; 
        public Node(int data)
        {
            Data = data;
            Left = null;      // null reference? Does this make sense? Then I can use "?." operator to get next node
            Right = null;
        }
        // Make this a friend to BST


        /*override <, >, !=, ==, Equals, toString
         */
    }
    class BST
    {
        private Node head = null;
        

        public BST(int[] numberList)
        {
            foreach (int num in numberList)
            {
                
            }
        }

        public void PrintInorder()
        {

        }
    }
}
