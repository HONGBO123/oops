/*
 * Name: Kyler Little
 * ID: 11472421
 */




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
        }

        static void PrintMenu(string current_expression)
        {
            Console.WriteLine("---------- MENU ----------");
            Console.WriteLine("Current Expression: {0}", current_expression);
            Console.WriteLine("\t1. Enter a new expression");
            Console.WriteLine("\t2. Set a variable value");
            Console.WriteLine("\t3. Evaluate tree");
            Console.WriteLine("\t4. Quit");
        }

        public enum MenuOptions { NEW_EXPR = 1, SET_VAR_VAL, EVAL, QUIT }

        /// <summary>
        /// UI program to test expression tree
        /// </summary>
        static void RunExpTreeTester()
        {
            string new_expr = "5+5", menu_option = String.Empty;
            ExpTree tree = new ExpTree(new_expr);
            int option = 0;
            do
            {
                PrintMenu(new_expr);        // print menu each time through the loop
                menu_option = Console.ReadLine();
                Int32.TryParse(menu_option, out option);        // if this fails, we'll go to the default case in the switch statement
                switch ((MenuOptions)option)
                {
                    case MenuOptions.NEW_EXPR:
                        Console.Write("Enter new expression: ");             // Prompt for input.
                        new_expr = Console.ReadLine();                       // Grab user input (new expression).
                        try
                        {
                            tree = new ExpTree(new_expr);                        // Instantiate new expression tree
                        }
                        catch (Exception ex)        // error can be thrown if construction fails
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    case MenuOptions.SET_VAR_VAL:
                        string variable_name = String.Empty, variable_value = String.Empty;
                        Console.Write("Enter variable name: ");             // Prompt for input.
                        variable_name = Console.ReadLine();                 // Grab variable name.
                        Console.Write("Enter variable value: ");            // Prompt for input.
                        variable_value = Console.ReadLine();                // Grab variable value as string.
                        bool success = Int32.TryParse(variable_value, out int val);        // Parse value into integer.
                        if (success)
                        {
                            try
                            {
                                tree.SetVar(variable_name, val);                    // Set variable within expression tree if successful parse.
                            }
                            catch (KeyNotFoundException ex)       // if variable not found, we need to handle the error gracefully
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid variable value.");
                        }
                        break;
                    case MenuOptions.EVAL:
                        try
                        {
                            Console.WriteLine("Result: {0}", tree.Eval());
                        }
                        catch (NullReferenceException ex)      // if not all variables are set, then null reference exception thrown
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case MenuOptions.QUIT:
                        Console.WriteLine("Done.");
                        break;
                    default:
                        Console.WriteLine("Not a valid menu option.");
                        break;
                }

            } while ((MenuOptions)option != MenuOptions.QUIT);
        }
    }
}
