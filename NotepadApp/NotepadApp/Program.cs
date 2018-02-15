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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotepadApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
