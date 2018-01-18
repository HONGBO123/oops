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
        internal Node Left;
        internal Node Right;
        public int Data; 
        public Node(int data)
        {
            Data = data;
            Left = null;      // null reference? Does this make sense? Then I can use "?." operator to get next node
            Right = null;
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        /*override <, >, !=, ==, Equals, toString
         */
    }
    class BST
    {
        private Node head = null;
        private int nodeCount = 0;
        
        private void Insert(int num, ref Node node)
        {
            if (node == null)
            {
                node = new Node(num);
                ++nodeCount;
            } else if (node.Data == num)
            {
                // throw error; return immediately
                return;
            } else if (node.Data < num)       // num belongs in right subtree
            {
                Insert(num, ref node.Right);
            } else        // num belonds in left subtree
            {
                Insert(num, ref node.Left); 
            }
        }

        private void PrintInorderRecursive(ref Node node)
        {
            if (node != null)
            {
                PrintInorderRecursive(ref node.Left);
                Console.Write(node); Console.Write(' ');     // add space between each node
                PrintInorderRecursive(ref node.Right);
            }
        }

        private void PrintNumberOfNodes()
        {
            Console.WriteLine("\tNumber of nodes: '{0}'.", nodeCount);
        }

        private int maxDepth(ref Node node)
        {
            if (node == null)
            {
                return 0;
            } else
            {
                int leftDepth = maxDepth(ref node.Left);
                int rightDepth = maxDepth(ref node.Right);
                return (leftDepth > rightDepth) ? leftDepth + 1 : rightDepth + 1;
            }
        }

        private void PrintNumberOfLevels()
        {
            int numberOfLevels = maxDepth(ref head);
            Console.WriteLine("\tNumber of levels: '{0}'.", numberOfLevels);
        }

        private void PrintMinimumLevelCount()
        {
            int theoreticalMinimum = (int)Math.Floor(Math.Log(nodeCount, 2)) + 1;   
            Console.WriteLine("\tMinimum number of levels that a tree with '{0}' nodes could have: '{1}'.", nodeCount, theoreticalMinimum);
        }

        public BST(int[] numberList)
        {
            foreach (int num in numberList)
            {
                Insert(num, ref head);
            }
        }

        public void PrintInorder()
        {
            Console.Write("Tree contents: ");
            PrintInorderRecursive(ref head);
            Console.Write('\n');   // add in new line
        }

        public void PrintTreeStatistics()
        {
            PrintNumberOfNodes();
            PrintNumberOfLevels();
            PrintMinimumLevelCount();
        }
    }
}
