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
    /*
     * Node: node class for a binary search tree node
     */
    public class Node<Comparable>
    {
        // All variables are internal, meaning only classes within the same assembly can access them (code in different assembly cannot)
        internal Node<Comparable> Left; 
        internal Node<Comparable> Right;
        internal Comparable Data;
        
        // Constructor
        public Node(Comparable data)
        {
            Data = data;
            Left = null;
            Right = null;
        }

        public static bool operator ==(Node<Comparable> lhs, Node<Comparable> rhs)
        {
            if (System.Object.ReferenceEquals(lhs, rhs))           // If references match, then obviously same object. This includes if both references are null.
            {
                return true;
            }

            if ((object)lhs == null || (object)rhs == null)        // If one of references is null, returh false.
            {
                return false;
            }

            return Comparer<Comparable>.Default.Compare(lhs.Data, rhs.Data) == 0;          // Otherwise, return result of comparing Data Properties.
        }

        public static bool operator !=(Node<Comparable> lhs, Node<Comparable> rhs)
        {
            return !(lhs == rhs);       // Utilize '==' override above.
        }

        /*
         * For the following comparison operators (<, >, <=, >=), we use the Comparer class to compare
         * generic types. Comparer returns +1 if >, 0 if ==, and -1 if <.
         */
        public static bool operator >(Node<Comparable> lhs, Node<Comparable> rhs)
        {
            return Comparer<Comparable>.Default.Compare(lhs.Data, rhs.Data) > 0;
        }

        public static bool operator <(Node<Comparable> lhs, Node<Comparable> rhs)
        {
            return Comparer<Comparable>.Default.Compare(lhs.Data, rhs.Data) < 0;
        }

        public static bool operator >=(Node<Comparable> lhs, Node<Comparable> rhs)
        {
            return Comparer<Comparable>.Default.Compare(lhs.Data, rhs.Data) >= 0;
        }

        public static bool operator <=(Node<Comparable> lhs, Node<Comparable> rhs)
        {
            return Comparer<Comparable>.Default.Compare(lhs.Data, rhs.Data) <= 0;
        }

        // Returns the string's Data field when Node.ToString() is called
        public override string ToString()
        {
            return Data.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this == null)            // if either reference type is null, then return null
            {
                return false;
            }

            Node<Comparable> n = obj as Node<Comparable>;       // cast obj as Node
            if (n == null)                                      // If unable to cast, return false
            {
                return false;
            }

            return Comparer<Comparable>.Default.Compare(this.Data, n.Data) == 0;         // Otherwise, use Comparer to compare the templated Data values
        }

        // Override HashCode method
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /*
     * BST: Binary Search Tree
     */
    class BST<Comparable> : BinTree<Comparable>
    {
        // Private Data Fields
        private Node<Comparable> head = null;
        private int nodeCount = 0;

        // Constructor from array of integers
        public BST(Comparable[] comparableList)
        {
            foreach (Comparable val in comparableList)
            {
                this.Insert(val);
            }
        }

        // Internal inorder traversal in which the processing step is printing to the screen.
        private void PrintInOrderRecursive(ref Node<Comparable> node)
        {
            if (node != null)
            {
                PrintInOrderRecursive(ref node.Left);
                Console.Write(node); Console.Write(' ');     // add space between each node
                PrintInOrderRecursive(ref node.Right);
            }
        }

        // Internal postorder traversal in which the processing step is printing to the screen.
        private void PrintPostOrderRecursive(ref Node<Comparable> node)
        {
            if (node != null)
            {
                PrintPostOrderRecursive(ref node.Left);
                PrintPostOrderRecursive(ref node.Right);
                Console.Write(node); Console.Write(' ');     // add space between each node
            }
        }

        // Internal preorder traversal in which the processing step is printing to the screen.
        private void PrintPreOrderRecursive(ref Node<Comparable> node)
        {
            if (node != null)
            {
                Console.Write(node); Console.Write(' ');     // add space between each node
                PrintPreOrderRecursive(ref node.Left);
                PrintPreOrderRecursive(ref node.Right);
            }
        }

        // Internal method to print the number of nodes to the screen.
        private void PrintNumberOfNodes()
        {
            Console.WriteLine("\tNumber of nodes: '{0}'.", nodeCount);
        }

        // Internal method to recursively calculate the maximum depth (aka the height) of the tree
        private int MaxDepth(ref Node<Comparable> node)
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
            PrintInOrderRecursive(ref head);
            Console.Write('\n');   // add in new line
        }

        public override void PreOrder()
        {
            Console.Write("Tree contents: ");
            PrintPreOrderRecursive(ref head);
            Console.Write('\n');   // add in new line
        }

        public override void PostOrder()
        {
            Console.Write("Tree contents: ");
            PrintPostOrderRecursive(ref head);
            Console.Write('\n');   // add in new line
        }

        // Internal insert helper
        private void InsertHelper(ref Node<Comparable> newNode, ref Node<Comparable> tree)
        {
            if (tree == null)               // Then it's time to insert
            {
                tree = newNode;
                ++nodeCount;                // increment node count
            }
            else if (tree < newNode)       // newNode belongs in right subtree
            {
                InsertHelper(ref newNode, ref tree.Right);
            }
            else if (tree > newNode)        // newNode belonds in left subtree
            {
                InsertHelper(ref newNode, ref tree.Left);
            }
        }

        public override void Insert(Comparable val)
        {
            Node<Comparable> newNode = new Node<Comparable>(val);               // Create the new node
            this.InsertHelper(ref newNode, ref head);                           // Insert in appropriate location
        }

        private bool ContainsHelper(ref Node<Comparable> newNode, ref Node<Comparable> tree)
        {
            if (tree == null)        // If null, we've reached a dead end. Return false.
            {
                return false;
            }
            else if (tree == newNode)    // Utilize overridden '==' so as not to access internals of the data
            {                              
                return true;                       
            }
            else if (tree < newNode)       // newNode is in right subtree (if it exists)
            {
                return ContainsHelper(ref newNode, ref tree.Right);
            }
            else        // tree > newNode, so traverse left subtree
            {
                return ContainsHelper(ref newNode, ref tree.Left);
            }
        }

        public override bool Contains(Comparable val)
        {
            Node<Comparable> newNode = new Node<Comparable>(val);        // Contains by comparing nodes themselves; don't access internals of Node; solution is to make temp node
            bool result = ContainsHelper(ref newNode, ref head);
            newNode = null;         // set temp node to null, so that garbage collector gets it
            return result;
        }
    }
}
