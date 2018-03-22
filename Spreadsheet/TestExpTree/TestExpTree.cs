using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetEngine;

namespace TestExpTree
{
    class TestExpTree
    {
        static void Main(string[] args)
        {
            RunExpTreeTester();
            ExpTree tree = new ExpTree("5+6 * 7");
            Console.WriteLine(tree.Eval());
        }

        static void PrintMenu(string current_expression)
        {
            Console.WriteLine("---------- MENU ----------");
            Console.WriteLine("Current Expression: {0}", current_expression);
            Console.WriteLine("\t1. Enter a new expression");
        }

        public enum MenuOptions { NEW_EXPR = 1, SET_VAR_VAL, EVAL, QUIT }

        /// <summary>
        /// UI program to test expression tree
        /// </summary>
        static void RunExpTreeTester()
        {
            string new_expr = String.Empty;
            int menu_option = 0;
            do
            {
                PrintMenu(new_expr);
                menu_option = Console.Read();
                switch ((MenuOptions)menu_option)
                {
                    case MenuOptions.NEW_EXPR:
                        new_expr = Console.ReadLine();
                        break;
                    case MenuOptions.SET_VAR_VAL:
                        break;
                    case MenuOptions.EVAL:
                        break;
                    case MenuOptions.QUIT:
                        Console.WriteLine("Done.");
                        break;
                    default:
                        break;
                }

            } while ((MenuOptions)menu_option != MenuOptions.QUIT);
        }
    }
}
