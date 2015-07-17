using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Planning
{
    public partial class NewProfileForm : Form
    {
        public NewProfileForm(List<string> profiles)
        {
            InitializeComponent();
            this.profiles = profiles;
        }

        List<string> profiles = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            addProfile();
        }
        void addProfile()
        {
            if (textBox1.Text != "")
            {
                Form1 main = this.Owner as Form1;
                profiles.Add(textBox1.Text);
                this.Close();
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addProfile();
            }
        }
    }
}
