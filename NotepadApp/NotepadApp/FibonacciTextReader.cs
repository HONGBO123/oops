/*
 * Author: Kyler Little
 * WSU ID Number: 11472421
 * Last Modified: 2/15/2018
 * Project Title: NotepadApp
 * Description:
 *          This project is a windows form application with basic I/O operations. The user can load a text file and have
 *          its contents displayed to the window. The user may also save the current text in the window to a file of his/
 *          her choosing.
 *          Lastly, the user can choose to load the first 50 or 100 Fibonacci Numbers. He/she may also save these to a file
 *          name of his/her choosing.
 * 
 */




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NotepadApp
{
    class Fibonacci
    {
        /*
         *  NthFibonacciNumber:
         *      Code adapted from: https://www.geeksforgeeks.org/program-for-nth-fibonacci-number/
         *      Returns the Nth number in the fibonacci sequence
         */
        public static BigInteger NthFibonacciNumber(int N)
        {
            BigInteger a = 0, b = 1, c;
            if (N == 1)         // Define the first number in the sequence to be a == 0.
            {
                return a;
            }
            else
            {
                for (int i = 3; i <= N; i++)         // If N is 2, return the second number (b == 1). Otherwise, calculate the next number.
                {
                    c = a + b;                       // Next number is sum of previous two.
                    a = b;                           // Update a.
                    b = c;                           // Update b.
                }
                return b;
            }
        }
    }
    class FibonacciTextReader : System.IO.TextReader
    {
        private int nLines;                  // Internal Storage for Number of Fib Terms to Calculate
        private int currentLine = 1;         // Internal Counter
        public FibonacciTextReader(int n)
        {
            nLines = n;
        }

        public override string ReadLine()
        {
            return Fibonacci.NthFibonacciNumber(currentLine).ToString();
        }

        public override string ReadToEnd()
        {
            string fibNumbers = "";
            while (currentLine <= nLines)
            {
                fibNumbers += $"{currentLine}: " + ReadLine();              // Basically, append "TermNumberInFibSequence: FibSequence[TermNumber]"
                if (currentLine != nLines)              // If the current line is NOT the last one, then add a newline.
                {
                    fibNumbers += Environment.NewLine;
                }
                ++currentLine;
            }
            return fibNumbers;
        }
    }
}
