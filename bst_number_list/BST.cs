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
    public class Node
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

        //private static bool Comparison(ref Node lhs, ref Node rhs)
        //{
        //    if (lhs == null || rhs == null)
        //    {
        //        //return false
        //    }
        //}

        public static bool operator ==(Node lhs, Node rhs)
        {
            if (lhs == rhs)
            {
                return true;
            }
            return lhs.Data.Equals(rhs.Data);
        }

        public static bool operator !=(Node lhs, Node rhs)
        {
            return lhs.Data != rhs.Data;
        }

        public static bool operator >(Node lhs, Node rhs)
        {
            return lhs.Data > rhs.Data;
        }

        public static bool operator <(Node lhs, Node rhs)
        {
            return lhs.Data < rhs.Data;
        }

        public static bool operator >=(Node lhs, Node rhs)
        {
            return lhs.Data >= rhs.Data;
        }

        public static bool operator <=(Node lhs, Node rhs)
        {
            return lhs.Data <= rhs.Data;
        }

        // Returns the string's Data field when Node.ToString() is called
        public override string ToString()
        {
            return Data.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /*
     * BST: Binary Search Tree
     */
    class BST : BinTree<int>
    {
        // Private Data Fields
        private Node head = null;
        private int nodeCount = 0;

        // Constructor from array of integers
        public BST(int[] numberList)
        {
            foreach (int val in numberList)
            {
                this.Insert(val);
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

        // Public method to print interesting statistics about the BST-- namely, number of nodes, levels, and minimum level count (theoretically)
        public void PrintTreeStatistics()
        {
            PrintNumberOfNodes();
            PrintNumberOfLevels();
            PrintMinimumLevelCount();
        }

        // Public method to print the contents of the BST inorder
        public override void InOrder()
        {
            Console.Write("Tree contents: ");
            PrintInorderRecursive(ref head);
            Console.Write('\n');   // add in new line
        }

        public override void PreOrder()
        {
            throw new NotImplementedException();
        }

        public override void PostOrder()
        {
            throw new NotImplementedException();
        }

        public override void Insert(int val)
        {
            Node newNode = new Node(val);       // Create the new node
            Console.WriteLine(newNode);
            this.InsertHelper(ref newNode, ref head);                           // Insert in appropriate location
        }

        // Internal insert helper
        private void InsertHelper(ref Node newNode, ref Node tree)
        {
            if (tree == null)
            {
                tree = newNode;
                ++nodeCount;
            }
            else if (newNode == tree)
            {
                return;
            }
            else if (tree < newNode)       // newNode belongs in right subtree
            {
                InsertHelper(ref newNode, ref tree.Right);
            }
            else if (tree > newNode)        // newNode belonds in left subtree
            {
                InsertHelper(ref newNode, ref tree.Left);
            }
            //else
            //{
            //    newNode = null;
            //}
        }

        public override bool Contains(int val)
        {
            throw new NotImplementedException();
        }
    }
}
