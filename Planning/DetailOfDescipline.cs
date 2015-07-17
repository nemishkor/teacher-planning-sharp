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
    public partial class DetailOfDescipline : Form
    {
        public DetailOfDescipline(List<DisciplineType> discipline, int i)
        {
            InitializeComponent();
            labelDisciplineGroup.Text = discipline[i].discipline + "-" + discipline[i].group;
            comboBox1Types.Items.AddRange(allTypesOfEmployment);
            addhours(comboBox1Types, numericUpDown1);
            this.discipline = discipline;
            this.i = i;
        }
        List<DisciplineType> discipline;
        int i;
        // ----------------------------- TypeOfEmployment
        int countTypesOfEmployments = 1;
        public string[] allTypesOfEmployment = new string[15]{
            "Лекції",
            "Практичні заняття",
            "Семінарські",
            "Лабораторні",
            "Інд. робота",
            "Самостійна",
            "Конс. з дисц.",
            "Екзамени",
            "Конс. до екзаменів",
            "Заліки",
            "Інд. завдання/зан.",
            "Контр. роботи",
            "Дипл. (Маг) роботи",
            "Керівн. практ.",
            "Участь в ДЕК",
        };
        class Employ
        {
            public Employ(ComboBox comboBox, NumericUpDown numericUpDown)
            {
                typeOfEmployments = comboBox;
                hours = numericUpDown;
            }
            public ComboBox typeOfEmployments;
            public NumericUpDown hours;
            public override string ToString()
            {
                return typeOfEmployments.Text + ": " + hours.Value + "hours";
            }

        }
        void employmentsDataToControls()
        {
            for (int j = 0; j < discipline[i].employments.Count; j++)
            {
                add
            }
        }
        List<Employ> hours = new List<Employ>(); //results saved there
        void addhours(ComboBox comboBox, NumericUpDown numericUpDown)
        {
            hours.Add(new Employ(comboBox, numericUpDown));
        }

        private void addTypeOfEmployment_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Visible = false;
            string num = b.Name.ToString().Substring(19);
            int numInt = Convert.ToInt32(num);
            //список
            ComboBox comboBox2Types = new ComboBox();
            comboBox2Types.FormattingEnabled = true;
            comboBox2Types.Location = new System.Drawing.Point(0, (numInt - 1) * 25 - panel1.VerticalScroll.Value);
            comboBox2Types.Name = "comboBox1Types";
            comboBox2Types.Size = new System.Drawing.Size(130, 21);
            comboBox2Types.TabIndex = 0;
            comboBox2Types.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            panel1.Controls.Add(comboBox2Types);
            comboBox2Types.Items.AddRange(allTypesOfEmployment);
            //кількість
            NumericUpDown numericUpDown2 = new NumericUpDown();
            numericUpDown2.Location = new System.Drawing.Point(134, (numInt - 1) * 25 - panel1.VerticalScroll.Value);
            numericUpDown2.Name = "numericUpDown" + num;
            numericUpDown2.Size = new System.Drawing.Size(70, 21);
            numericUpDown2.TabIndex = 1;
            numericUpDown2.Value = 1;
            panel1.Controls.Add(numericUpDown2);
                //кнопка додати
                Button addTypeOfEmployment = new Button();
                addTypeOfEmployment.BackgroundImage = global::Planning.Properties.Resources.add_32;
                addTypeOfEmployment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                addTypeOfEmployment.Location = new System.Drawing.Point(210, (numInt - 1) * 25 - panel1.VerticalScroll.Value);
                addTypeOfEmployment.Size = new System.Drawing.Size(20, 20);
                addTypeOfEmployment.TabIndex = 3;
                addTypeOfEmployment.UseVisualStyleBackColor = true;
                addTypeOfEmployment.Click += new System.EventHandler(this.addTypeOfEmployment_Click);
                addTypeOfEmployment.Name = "addTypeOfEmployment" + (numInt + 1);
                panel1.Controls.Add(addTypeOfEmployment);
            countTypesOfEmployments++;
            addhours(comboBox2Types, numericUpDown2);
        }
        void addTypeOfEmployment()
        { 
        }

        private void btnDetailsReady_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < hours.Count; index++)
            {
                Form1 main = this.Owner as Form1;
                discipline[i].employments.Add(new Employment()
                {
                    type = hours[index].typeOfEmployments.Text,
                    hoursNeedToTeach = hours[index].hours.Value
                });
            }
            this.Close();
        }

        private void btnDetailsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
