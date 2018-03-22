using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    internal abstract class TreeNode
    {
        public abstract double Eval();
    }

    internal class ValueNode : TreeNode
    {
        public override double Eval()
        {
            throw new NotImplementedException();
        }
    }

    internal abstract class OperatorNode : TreeNode
    {
        public override abstract double Eval();
    }

    internal abstract class BinaryOperatorNode : OperatorNode
    {
        /// <summary>
        /// Fields:
        ///     left
        ///     right
        ///     Binary Operator Nodes should have two children!
        /// </summary>
        public TreeNode left;
        public TreeNode right;
        public BinaryOperatorNode()
        {
            left = null;
            right = null;
        }
        public override abstract double Eval();
    }

    internal class AddNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() + this.right.Eval();
        }
    }

    internal class SubtractNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() - this.right.Eval();
        }
    }

    internal class MultiplyNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() * this.right.Eval();
        }
    }

    internal class DivideNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() / this.right.Eval();
        }
    }

    internal class CellReferenceNode : TreeNode
    {
        public override double Eval()
        {
            throw new NotImplementedException();
        }
    }

    internal abstract class OpNodeFactory
    {
        public abstract OperatorNode FactoryMethod(char op);
    }

    internal class ConcreteOpNodeFactory : OpNodeFactory
    {
        public override OperatorNode FactoryMethod(char op)
        {
            switch (op)
            {
                case '+': return new AddNode();
                case '-': return new SubtractNode();
                case '*': return new MultiplyNode();
                case '/': return new DivideNode();
                default: return null;
            }
        }
    }

    class ExpTree
    {
        TreeNode _root = null;
        Dictionary<string, double> _variable_dict;

        /// <summary>
        /// Construct the expression tree!
        /// </summary>
        /// <param name="expression"></param>
        ExpTree(string expression)
        {

        }

        void SetVar(string varName, double varValue)
        {

        }

        double Eval()
        {
            return _root.Eval();
        }
    }
}
