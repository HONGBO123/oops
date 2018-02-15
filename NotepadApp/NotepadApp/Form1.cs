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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NotepadApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadText(TextReader sr)
        {
            textBox1.Clear();          // Before loading, clear whatever text is currently in the text box
            try
            {
                textBox1.AppendText(sr.ReadToEnd());
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: couldn't read file from disk. Original Error: " + e.Message);
            }
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Select a file to be read and displayed in the window.",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)        // User selected file and clicked "ok"
            {
                using (StreamReader sr = new StreamReader(openFileDialog1.FileName))      // Using statement to open & close file all in one.
                {
                    LoadText(sr);          // Now that file is open, we load the text into the text box.
                }
            }
        }

        private void loadFibonacciNumbersfirst50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FibonacciTextReader ftr = new FibonacciTextReader(50))
            {
                LoadText(ftr);
            }
        }

        private void loadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FibonacciTextReader ftr = new FibonacciTextReader(100))
            {
                LoadText(ftr);
            }
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Title = "Save the text in the window to a file.",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)        // User selected file and clicked "ok"
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))      // Using statement to open & close file all in one.
                {
                    try
                    {
                        sw.Write(textBox1.Text);         // using Write instead of WriteLine so that newline isn't automatically added
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: writing window's text to file failure. Original Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
