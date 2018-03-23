/*
 * Name: Kyler Little
 * ID: 11472421
 */



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
        private int _value;

        public ValueNode(int value)
        {
            _value = value;
        }

        public override double Eval()
        {
            return _value;
        }
    }

    internal abstract class OperatorNode : TreeNode
    {
        public override abstract double Eval();
    }

    internal abstract class BinaryOperatorNode : OperatorNode
    {
        /*
         * Fields (note that binary operator must have two children)
         */
        public TreeNode left;
        public TreeNode right;

        /// <summary>
        /// Initially, the "left" and "right" references are made null.
        /// </summary>
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
        /*
         * Fields
         */
        private string _var_name;
        private double? _val;

        /// <summary>
        /// Variables only need names when constructed. Their values can be determined later.
        /// </summary>
        /// <param name="var_name"></param>
        /// <param name="val"></param>
        public CellReferenceNode(string var_name)
        {
            _var_name = var_name;
        }

        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get
            {
                return _var_name;
            }
        }

        /// <summary>
        /// Value property
        /// </summary>
        public double Value
        {
            set
            {
                _val = value;
            }
        }

        /// <summary>
        /// If Value has been set, we can evaluate; otherwise, a NullReferenceException is thrown.
        /// </summary>
        /// <returns></returns>
        public override double Eval()
        {
            if (_val.HasValue)
            {
                return _val.Value;
            }
            else
            {
                throw new NullReferenceException(String.Format("Variable {0}'s value is unknown", _var_name));
            }
        }
    }

    internal abstract class OpNodeFactory
    {
        public abstract OperatorNode FactoryMethod(char op);
    }

    /// <summary>
    /// Concrete Factory for OperationNodes
    /// Given an input character (the operator), this factory's "FactoryMethod"
    /// method will return the appropriate node.
    /// </summary>
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

    internal abstract class TreeNodeFactory
    {
        public abstract TreeNode FactoryMethod(string expression);
    }

    /// <summary>
    /// Concrete Factory for TreeNodes.
    /// Given an input expression, if it's not an operator, then it is assumed
    /// to be a valuenode or cellreferencenode. If conversion to int fails, it automatically
    /// becomes a cellreferencenode.
    /// </summary>
    internal class ConcreteTreeNodeFactory : TreeNodeFactory
    {
        public override TreeNode FactoryMethod(string expression)
        {
            OpNodeFactory opNodeFactory = new ConcreteOpNodeFactory();
            OperatorNode @operator = opNodeFactory.FactoryMethod(expression[0]);
            if (@operator != null)
            {
                return @operator;
            }
            else
            {
                bool success = Int32.TryParse(expression, out int result);
                if (success)
                {
                    return new ValueNode(result);
                }
                else
                {
                    return new CellReferenceNode(expression);
                }
            }
        }
    }

    public class ExpTree
    {
        /*
         * Fields
         */
        private TreeNode _root = null;
        private Dictionary<string, HashSet<CellReferenceNode>> _variable_dict;   // HashSet allows for indentical cell references in the same expression

        /// <summary>
        /// Construct the expression tree and initialize dictionary
        /// </summary>
        /// <param name="expression"></param>
        public ExpTree(string expression)
        {
            expression = expression.Replace(" ", String.Empty);    // Remove spaces from expression
            this._variable_dict = new Dictionary<string, HashSet<CellReferenceNode>>();
            _root = ConstructTree(expression);
        }

        /// <summary>
        /// Recursively construct the expression tree. Determine node type by utilizing 
        /// the factory method. In this way, we don't have to directly determine the type of TreeNode
        /// in this method.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private TreeNode ConstructTree(string expression)
        {
            if (expression == string.Empty)   // base case
            {
                return null;
            }
            
            // Recursive Step: Add children only if token is an operator.
            // Currently, we'll ignore order of operations and assume the higher precedence operators are at the front of the string
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                OpNodeFactory opNodeFactory = new ConcreteOpNodeFactory();
                OperatorNode @operator = opNodeFactory.FactoryMethod(expression[i]);
                if (@operator != null)    // then @operator is an operator
                {
                    if (@operator is BinaryOperatorNode)
                    {
                        BinaryOperatorNode binaryOperator = @operator as BinaryOperatorNode;
                        TreeNode left = ConstructTree(expression.Substring(0, i)), right = ConstructTree(expression.Substring(i + 1));
                        if (left is null || right is null)        // reference check
                        {
                            throw new ArgumentNullException("Operator is binary and needs two arguments.");
                        }
                        binaryOperator.left = left;
                        binaryOperator.right = right;
                        return binaryOperator;    // return the reference
                    }
                }
            }

            // If we traversed through "expression" without node creation, then there are no operators remaining
            // in the expression. Thus, the expression is either a CellReferenceNode or ValueNode.
            TreeNodeFactory treeNodeFactory = new ConcreteTreeNodeFactory();
            TreeNode treeNode = treeNodeFactory.FactoryMethod(expression);
            if (treeNode is CellReferenceNode)
            {
                CellReferenceNode cellReferenceNode = treeNode as CellReferenceNode;
                if (!_variable_dict.ContainsKey(cellReferenceNode.Name))      // variable not currently in dictionary
                {
                    _variable_dict.Add(cellReferenceNode.Name, new HashSet<CellReferenceNode>());
                }
                _variable_dict[cellReferenceNode.Name].Add(cellReferenceNode);    // hashset allows for multiple nodes with identical keys (ex: A5 + A5 + 6)
            }
            return treeNode;
        }

        /// <summary>
        /// Add variable to internal dictionary to keep track of cell references and their values
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public void SetVar(string varName, double varValue)
        {
            if (_variable_dict.ContainsKey(varName))
            {
                foreach (CellReferenceNode node in _variable_dict[varName])    // for each identical cell reference, set its value
                {
                    node.Value = varValue;
                }
            }
            else
            {
                throw new KeyNotFoundException(String.Format("{0} is not a variable in the expression", varName));
            }
        }

        /// <summary>
        /// Recursively evaluate the expression tree from the root.
        /// Throws a null reference exception if value's (i.e. a variable's) value is unknown
        /// </summary>
        /// <returns></returns>
        public double Eval()
        {
            try
            {
                return _root.Eval();
            }
            catch (NullReferenceException)    // thrown if a variable (cell reference node) has no value
            {
                throw;
            }
        }
    }
}
