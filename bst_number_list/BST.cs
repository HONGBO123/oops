using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bst_number_list
{
    /*
     * Node: node class for a binary search tree node
     */
    class Node
    {
        // All variables are internal, meaning only classes within the same assembly can access them (code in different assembly cannot)
        internal Node Left; 
        internal Node Right;
        internal int Data;
        
        // Constructor
        public Node(int data)
        {
            Data = data;
            Left = null;
            Right = null;
        }

        // Returns the string's Data field when Node.ToString() is called
        public override string ToString()
        {
            return Data.ToString();
        }
    }

    /*
     * DuplicateError: defines a more readable/descriptive error for when the user enters number duplicates
     */
    class DuplicateError : ArgumentException
    {
        public DuplicateError(string message)
            : base(message)
        {
        }
    }

    /*
     * BST: Binary Search Tree
     */
    class BST
    {
        // Private Data Fields
        private Node head = null;
        private int nodeCount = 0;
        
        // Internal insert method
        private void Insert(int num, ref Node node)
        {
            if (node == null)
            {
                node = new Node(num);
                ++nodeCount;
            }
            else if (node.Data == num)
            {
                throw new DuplicateError("Duplicates aren't allowed, silly!");
            }
            else if (node.Data < num)       // num belongs in right subtree
            {
                Insert(num, ref node.Right);
            }
            else        // num belonds in left subtree
            {
                Insert(num, ref node.Left); 
            }
        }

        // Internal inorder traversal in which the processing step is printing to the screen.
        private void PrintInorderRecursive(ref Node node)
        {
            if (node != null)
            {
                PrintInorderRecursive(ref node.Left);
                Console.Write(node); Console.Write(' ');     // add space between each node
                PrintInorderRecursive(ref node.Right);
            }
        }

        // Internal method to print the number of nodes to the screen.
        private void PrintNumberOfNodes()
        {
            Console.WriteLine("\tNumber of nodes: '{0}'.", nodeCount);
        }

        // Internal method to recursively calculate the maximum depth (aka the height) of the tree
        private int MaxDepth(ref Node node)
        {
            if (node == null)    // define a null node to have height 0
            {
                return 0;
            } else
            {
                int leftDepth = MaxDepth(ref node.Left);        // recursively calculate MaxDepth of left subtree
                int rightDepth = MaxDepth(ref node.Right);       // recursively calculate MaxDepth of right subtree
                return (leftDepth > rightDepth) ? leftDepth + 1 : rightDepth + 1;        // return the LARGER of the two + 1
            }
        }

        // Internal method to print the number of levels (i.e. the height) of the BST
        private void PrintNumberOfLevels()
        {
            int numberOfLevels = MaxDepth(ref head);
            Console.WriteLine("\tNumber of levels: '{0}'.", numberOfLevels);
        }

        // Internal method which calculates the theoretical minimum number of levels of a BST with 'nodeCount' nodes
        private void PrintMinimumLevelCount()
        {
            int theoreticalMinimum = (int)Math.Floor(Math.Log(nodeCount, 2)) + 1;   
            Console.WriteLine("\tMinimum number of levels that a tree with '{0}' nodes could have: '{1}'.", nodeCount, theoreticalMinimum);
        }

        // Constructor from integer array; throws error on duplicate
        public BST(int[] numberList)
        {
            foreach (int num in numberList)
            {
                Insert(num, ref head);
            }
        }

        // Public method to print the contents of the BST inorder
        public void PrintInorder()
        {
            Console.Write("Tree contents: ");
            PrintInorderRecursive(ref head);
            Console.Write('\n');   // add in new line
        }

        // Public method to print interesting statistics about the BST-- namely, number of nodes, levels, and minimum level count (theoretically)
        public void PrintTreeStatistics()
        {
            PrintNumberOfNodes();
            PrintNumberOfLevels();
            PrintMinimumLevelCount();
        }
    }
}
