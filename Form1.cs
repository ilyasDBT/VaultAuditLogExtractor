using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VaultAuditLogExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog oDialog = new OpenFileDialog() {
                Title = "Select vault audit log files",
                Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt",
                FilterIndex = 2,
                Multiselect = true
            };

            oDialog.ShowDialog();
            if (oDialog.FileNames.Count() == 0) { return; }

            listBox1.Items.Clear();
            //MessageBox.Show("more than 0");
            foreach (string oFileName in oDialog.FileNames)
            {
                listBox1.Items.Add(oFileName);
            }
            label2.Text = listBox1.Items.Count.ToString() + " Files Selected";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { MessageBox.Show("Search string is empty. Abort"); return; }
            if (listBox1.Items.Count == 0) { MessageBox.Show("No log file specified. Abort"); return; }
            string searchString = textBox1.Text;

            SaveFileDialog oDialog = new SaveFileDialog()
            {
                Title = "Choose result save location",
                Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt",
                FilterIndex = 2
            };
            oDialog.ShowDialog();

            if (oDialog.FileNames.Count() == 0) {return; }

            int counter = 0;
            using (StreamWriter oResultFile = new StreamWriter(oDialog.FileName))
            {
                foreach (string oFileName in listBox1.Items)
                {
                    using (StreamReader oFile = new StreamReader(oFileName))
                    {
                        string oLine = "";
                        string previousLine = "";
                        while ((oLine = oFile.ReadLine()) != null)
                        {
                            if (oLine.Contains(searchString))
                            {
                                counter++;
                                oResultFile.WriteLine(previousLine);
                                oResultFile.WriteLine(oLine);
                                oResultFile.WriteLine("");
                            }
                            previousLine = oLine;
                        }
                        oFile.Close();

                    }

                }
            }
            MessageBox.Show("Extraction Complete. " + counter.ToString() + " records found.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
