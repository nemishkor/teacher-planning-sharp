using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Planning
{
    public partial class Results : Form
    {
        public Results(string textBox)
        {
            InitializeComponent();
            richTextBoxResults.Text = textBox;
        }

        private void buttonSaveInTxt_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"c:\";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                StreamWriter myStream = new StreamWriter(saveFileDialog.FileName);
                for (int i = 0; i < richTextBoxResults.Lines.Length; i++)
                {
                    myStream.WriteLine(richTextBoxResults.Lines[i]);
                }

                myStream.Close();

                this.Close();
            }
        }

        private void Results_Resize(object sender, EventArgs e)
        {
            richTextBoxResults.Size = new Size(this.Width - 24, this.Height - 103);
            buttonClose.Location = new Point(this.Width - 116, this.Height - 85);
            buttonSaveInTxt.Location = new Point(12, this.Height - 85);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {

            DialogResult res =
                MessageBox.Show("Вийти без збереження результатів?", "Закрити програму",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                this.Close();
            }

        }
    }
}
