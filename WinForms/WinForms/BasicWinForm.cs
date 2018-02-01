using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class BasicWinForm : Form
    {
        public BasicWinForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;   // Set the 'ReadOnly' property to true.
            textBox1.Text = "Click on Crandall's Face to run the program.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Step 0. Disable button (only allowed to click once) and make it transparent.
            button1.Enabled = false;
            button1.BackColor = Color.Transparent;

            // Step 1. Generate random int list.
            List<int> randomIntList = new List<int>(Constants.listNumberElements);
            RandomIntListGenerator.Fill(ref randomIntList, Constants.listLowerBound, Constants.listUpperBound);

            // Step 2. Print off some cool information.
            textBox1.Clear();      // clear whatever is currently in textBox1 (in this case, the load message)
            textBox1.AppendText("1. HashSet Method: " + UniqueCounter.UsingDict(ref randomIntList).ToString() + " unique numbers" + Environment.NewLine);
            textBox1.AppendText(UniqueCounter.UsingDictExplanation() + Environment.NewLine);
            textBox1.AppendText("2. O(1) storage method: " + UniqueCounter.UsingBruteForce(ref randomIntList).ToString() + " unique numbers" + Environment.NewLine);
            textBox1.AppendText("3. Sorted method: " + UniqueCounter.UsingSortAndCount(ref randomIntList).ToString() + " unique numbers" + Environment.NewLine);
        }
    }

    static class Constants
    {
        public const int listLowerBound = 0;
        public const int listUpperBound = 20000;
        public const int listNumberElements = 10000;
    }
}
