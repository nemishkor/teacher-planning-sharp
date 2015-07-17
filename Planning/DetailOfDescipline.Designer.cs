namespace Planning
{
    partial class DetailOfDescipline
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.typeOfEmplyment = new System.Windows.Forms.GroupBox();
            this.addTypeOfEmployment2 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.comboBox1Types = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.labelDisciplineGroup = new System.Windows.Forms.Label();
            this.btnDetailsReady = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.typeOfEmplyment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // typeOfEmplyment
            // 
            this.typeOfEmplyment.Controls.Add(this.panel1);
            this.typeOfEmplyment.Location = new System.Drawing.Point(12, 103);
            this.typeOfEmplyment.Name = "typeOfEmplyment";
            this.typeOfEmplyment.Size = new System.Drawing.Size(270, 149);
            this.typeOfEmplyment.TabIndex = 6;
            this.typeOfEmplyment.TabStop = false;
            this.typeOfEmplyment.Text = "Вид роботи/кількість годин ";
            // 
            // addTypeOfEmployment2
            // 
            this.addTypeOfEmployment2.BackgroundImage = global::Planning.Properties.Resources.add;
            this.addTypeOfEmployment2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.addTypeOfEmployment2.Location = new System.Drawing.Point(210, 0);
            this.addTypeOfEmployment2.Name = "addTypeOfEmployment2";
            this.addTypeOfEmployment2.Size = new System.Drawing.Size(20, 20);
            this.addTypeOfEmployment2.TabIndex = 3;
            this.addTypeOfEmployment2.UseVisualStyleBackColor = true;
            this.addTypeOfEmployment2.Click += new System.EventHandler(this.addTypeOfEmployment_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(134, 0);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(70, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBox1Types
            // 
            this.comboBox1Types.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1Types.FormattingEnabled = true;
            this.comboBox1Types.Location = new System.Drawing.Point(0, 0);
            this.comboBox1Types.Name = "comboBox1Types";
            this.comboBox1Types.Size = new System.Drawing.Size(130, 21);
            this.comboBox1Types.TabIndex = 0;
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label41.Location = new System.Drawing.Point(12, 9);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(270, 40);
            this.label41.TabIndex = 9;
            this.label41.Text = "Заповніть навантаження для дисципліни-групи:";
            // 
            // labelDisciplineGroup
            // 
            this.labelDisciplineGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDisciplineGroup.Location = new System.Drawing.Point(12, 49);
            this.labelDisciplineGroup.Name = "labelDisciplineGroup";
            this.labelDisciplineGroup.Size = new System.Drawing.Size(270, 51);
            this.labelDisciplineGroup.TabIndex = 10;
            this.labelDisciplineGroup.Text = "{дисципліна-група}";
            this.labelDisciplineGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDetailsReady
            // 
            this.btnDetailsReady.Image = global::Planning.Properties.Resources.forward;
            this.btnDetailsReady.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDetailsReady.Location = new System.Drawing.Point(65, 258);
            this.btnDetailsReady.Name = "btnDetailsReady";
            this.btnDetailsReady.Size = new System.Drawing.Size(169, 27);
            this.btnDetailsReady.TabIndex = 11;
            this.btnDetailsReady.Text = "OK";
            this.btnDetailsReady.UseVisualStyleBackColor = true;
            this.btnDetailsReady.Click += new System.EventHandler(this.btnDetailsReady_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.addTypeOfEmployment2);
            this.panel1.Controls.Add(this.comboBox1Types);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 124);
            this.panel1.TabIndex = 4;
            // 
            // DetailOfDescipline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 301);
            this.ControlBox = false;
            this.Controls.Add(this.btnDetailsReady);
            this.Controls.Add(this.labelDisciplineGroup);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.typeOfEmplyment);
            this.Name = "DetailOfDescipline";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Додати навантаження";
            this.typeOfEmplyment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox typeOfEmplyment;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox comboBox1Types;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label labelDisciplineGroup;
        private System.Windows.Forms.Button btnDetailsReady;
        private System.Windows.Forms.Button addTypeOfEmployment2;
        private System.Windows.Forms.Panel panel1;
    }
}