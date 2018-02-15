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
            textBox1.ReadOnly = true;
        }

        private void LoadText(TextReader sr)
        {
            textBox1.Clear();
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
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select a file to be read and displayed in the window.";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)        // User selected file and clicked "ok"
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName);      // add some exception handling right here because files suck
                LoadText(sr);
            }
        }

        private void loadFibonacciNumbersfirst50ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
