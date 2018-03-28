/*
 * Name: Kyler Little
 * ID: 11472421
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SpreadsheetEngine
{
    internal abstract class TreeNode
    {
        public abstract double Eval();
    }

    internal class ValueNode : TreeNode
    {
        private int? _int;
        private double? _double;

        public ValueNode(int value)
        {
            _int = value;
        }

        public ValueNode(double value)
        {
            _double = value;
        }

        public override double Eval()
        {
            // Return the value that was set in the constructor
            if (_int.HasValue) return _int.Value;
            else return _double.Value;
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
        public abstract OperatorNode FactoryMethod(string op);
    }

    /// <summary>
    /// Concrete Factory for OperationNodes
    /// Given an input character (the operator), this factory's "FactoryMethod"
    /// method will return the appropriate node.
    /// </summary>
    internal class ConcreteOpNodeFactory : OpNodeFactory
    {
        public override OperatorNode FactoryMethod(string op)
        {
            switch (op)
            {
                case "+": return new AddNode();
                case "-": return new SubtractNode();
                case "*": return new MultiplyNode();
                case "/": return new DivideNode();
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
            OperatorNode @operator = opNodeFactory.FactoryMethod(expression);
            if (@operator != null)
            {
                return @operator;
            }
            else
            {
                bool int_success = Int32.TryParse(expression, out int int_result), 
                    double_success = Double.TryParse(expression, out double double_result);
                if (int_success)
                {
                    return new ValueNode(int_result);
                }
                else if (double_success)
                {
                    return new ValueNode(double_result);
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
            try
            {
                _root = ConstructTree(InfixToPostfix(expression));
            }
            catch (Exception ex)       // REMOVE THIS ONCE I ADD TO SPREADSHEET! Spreadsheet needs to handle the error / propagate it up to UI layer
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Construct the expression tree from a postfix input string. 
        /// Pseudocode from: https://www.geeksforgeeks.org/expression-tree/
        /// Determine node type by utilizing the factory method. In this way, 
        /// we don't have to directly determine the type of TreeNode
        /// in this method.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private TreeNode ConstructTree(string expression)
        {
            // Step 1: Tokenize once again.
            var list = Tokenize(expression, false);    // false ==> wildcards are NOT present

            // Step 2: Walk through tokens and build tree using a stack
            Stack<TreeNode> stack = new Stack<TreeNode>();
            TreeNodeFactory treeNodeFactory = new ConcreteTreeNodeFactory();
            foreach (string tok in list)
            {
                TreeNode tree = treeNodeFactory.FactoryMethod(tok);
                if (tree is OperatorNode)
                {
                    if (tree is BinaryOperatorNode)       // do subcheck because I might extend this for unary operators too.
                    {
                        BinaryOperatorNode binaryOperator = tree as BinaryOperatorNode;     // cast as binary op
                        TreeNode right = stack.Pop(), left = stack.Pop();                   // since stack is LIFO, right child is top
                        binaryOperator.left = left; binaryOperator.right = right;           // set children
                        stack.Push(binaryOperator);                                         // push back onto stack
                    }
                }
                else if (tree is CellReferenceNode)
                {
                    CellReferenceNode cellReferenceNode = tree as CellReferenceNode;
                    if (!_variable_dict.ContainsKey(cellReferenceNode.Name))      // variable not currently in dictionary
                    {
                        _variable_dict.Add(cellReferenceNode.Name, new HashSet<CellReferenceNode>());
                    }
                    _variable_dict[cellReferenceNode.Name].Add(cellReferenceNode);    // hashset allows for multiple nodes with identical keys (ex: A5 + A5 + 6)
                    stack.Push(tree);     // simply push
                }
                else      // ValueNode
                {
                    stack.Push(tree);     // simply push
                }
            }
            return stack.Pop();
        }

        /// <summary>
        /// Tokenize the expression into a list of strings. Matches decimal/integers, cell labels, operators, and wildcard.
        /// We match wildcard when wild_cards_present == true so that invalid input can be detected during the 
        /// infixToPostfix stage.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private List<string> Tokenize(string expression, bool wild_cards_present)
        {
            // REGEX Notes:
            // [] matches just single char in that group
            // () matches full expression
            // * means matches previous thing 0 or more times; + means one or more
            // ? means 0 or more
            // use '\' for special characters to escape it

            // This pattern will match decimal numbers (first part), cell label (second part), the operators: +-*/() (third part), or wildcard: . (fourth part)
            string @pattern = @"[\d]+\.?[\d]*|[A-Za-z]+[0-9]+|[-/\+\*\(\)]";
            if (wild_cards_present) pattern += "|.";         // add in this optional wildcard if bool passed in is true
            Regex r = new Regex(@pattern);
            MatchCollection matchList = Regex.Matches(expression, @pattern);
            return matchList.Cast<Match>().Select(match => match.Value).ToList();
        }

        /// <summary>
        /// Shunting Yard Algorithm
        /// Developed from pseudocode here: https://brilliant.org/wiki/shunting-yard-algorithm/
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string InfixToPostfix(string expression)
        {
            HashSet<char> operators = new HashSet<char>(new char[] { '+', '-', '*', '/' });
            Dictionary<char, int> precedence = new Dictionary<char, int>
            {
                ['('] = 0,
                ['+'] = 1,
                ['-'] = 1,
                ['*'] = 2,
                ['/'] = 2,
                [')'] = 10            // high precedence since it's closing off a priority subsection 
            };

            var list = Tokenize(expression, true);       // true ==> wildcards ARE present
            Queue<string> output_list = new Queue<string>(list.Capacity);
            Stack<char> opStack = new Stack<char>();
            foreach (string tok in list)
            {
                if (int.TryParse(tok, out int int_result) || double.TryParse(tok, out double dec_result)
                    || Regex.Match(tok, @"[A-Za-z]+[0-9]+").Success)        // if token is an integer or double or a cell label (i.e. some letters followed by some numbers)
                {
                    output_list.Enqueue(tok);    // push to output queue
                }
                else    // assume it's an operator type
                {
                    if (operators.Contains(tok[0]))          // i.e. tok is an operator
                    {
                        while (opStack.Count != 0 && precedence[opStack.Peek()] > precedence[tok[0]])    // while operator on opstack has higher precedence than current op
                        {
                            output_list.Enqueue(opStack.Pop().ToString());          // pop operator and enqueue to output
                        }
                        opStack.Push(tok[0]);       // push current op onto stack
                    }
                    else if (tok.StartsWith("("))      // mark left parenthesis by pushing onto opstack
                    {
                        opStack.Push(tok[0]);
                    }
                    else if (tok.StartsWith(")"))
                    {
                        try
                        {
                            while (opStack.Peek() != '(')          // If we run into a right parenthesis, keep popping opstack til we get to left parenthesis
                            {
                                output_list.Enqueue(opStack.Pop().ToString());       // this will throw an exception if opStack is empty
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            throw new Exception("Mismatched Parenthesis in expression");
                        }
                        opStack.Pop();    // pop left parenthesis from stack
                    }
                    else           // unrecognized token
                    {
                        throw new ArgumentException(string.Format("{0} is not a valid formula value.", tok));
                    }
                }
            }

            // If there are still operators on the opstack, pop them to the result queue.
            while (opStack.Count > 0)
            {
                if (opStack.Peek() != '(' || opStack.Peek() != ')')
                    output_list.Enqueue(opStack.Pop().ToString());
                else
                    throw new Exception("Mismatched Parenthesis in expression");
            }
            return string.Join(" ", output_list.ToArray());
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
