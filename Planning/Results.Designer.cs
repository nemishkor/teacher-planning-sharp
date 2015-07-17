namespace Planning
{
    partial class Results
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
            this.richTextBoxResults = new System.Windows.Forms.RichTextBox();
            this.buttonSaveInTxt = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // richTextBoxResults
            // 
            this.richTextBoxResults.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxResults.Name = "richTextBoxResults";
            this.richTextBoxResults.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxResults.Size = new System.Drawing.Size(268, 267);
            this.richTextBoxResults.TabIndex = 0;
            this.richTextBoxResults.Text = "";
            // 
            // buttonSaveInTxt
            // 
            this.buttonSaveInTxt.Image = global::Planning.Properties.Resources.import;
            this.buttonSaveInTxt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSaveInTxt.Location = new System.Drawing.Point(12, 285);
            this.buttonSaveInTxt.Name = "buttonSaveInTxt";
            this.buttonSaveInTxt.Size = new System.Drawing.Size(157, 48);
            this.buttonSaveInTxt.TabIndex = 1;
            this.buttonSaveInTxt.Text = "Зберегти та закрити";
            this.buttonSaveInTxt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSaveInTxt.UseVisualStyleBackColor = true;
            this.buttonSaveInTxt.Click += new System.EventHandler(this.buttonSaveInTxt_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Image = global::Planning.Properties.Resources.back;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClose.Location = new System.Drawing.Point(184, 285);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(96, 48);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // Results
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 343);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSaveInTxt);
            this.Controls.Add(this.richTextBoxResults);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Results";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Результати";
            this.Resize += new System.EventHandler(this.Results_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBoxResults;
        private System.Windows.Forms.Button buttonSaveInTxt;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}