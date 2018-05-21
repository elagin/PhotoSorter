namespace PhotoSorter
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxProcessed = new System.Windows.Forms.TextBox();
            this.textBoxTotalSize = new System.Windows.Forms.TextBox();
            this.comboBoxDriveList = new System.Windows.Forms.ComboBox();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(13, 66);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Старт";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxProcessed
            // 
            this.textBoxProcessed.Location = new System.Drawing.Point(144, 215);
            this.textBoxProcessed.Name = "textBoxProcessed";
            this.textBoxProcessed.Size = new System.Drawing.Size(75, 20);
            this.textBoxProcessed.TabIndex = 1;
            // 
            // textBoxTotalSize
            // 
            this.textBoxTotalSize.Location = new System.Drawing.Point(225, 215);
            this.textBoxTotalSize.Name = "textBoxTotalSize";
            this.textBoxTotalSize.Size = new System.Drawing.Size(75, 20);
            this.textBoxTotalSize.TabIndex = 2;
            // 
            // comboBoxDriveList
            // 
            this.comboBoxDriveList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDriveList.FormattingEnabled = true;
            this.comboBoxDriveList.Location = new System.Drawing.Point(13, 13);
            this.comboBoxDriveList.Name = "comboBoxDriveList";
            this.comboBoxDriveList.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDriveList.TabIndex = 3;
            // 
            // textBoxFolder
            // 
            this.textBoxFolder.Location = new System.Drawing.Point(12, 40);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.Size = new System.Drawing.Size(695, 20);
            this.textBoxFolder.TabIndex = 4;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(713, 37);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 5;
            this.buttonBrowse.Text = "Обзор...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Location = new System.Drawing.Point(713, 8);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(75, 23);
            this.buttonAbout.TabIndex = 6;
            this.buttonAbout.Text = "Автора";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxFolder);
            this.Controls.Add(this.comboBoxDriveList);
            this.Controls.Add(this.textBoxTotalSize);
            this.Controls.Add(this.textBoxProcessed);
            this.Controls.Add(this.buttonStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBoxProcessed;
        private System.Windows.Forms.TextBox textBoxTotalSize;
        private System.Windows.Forms.ComboBox comboBoxDriveList;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonAbout;
    }
}

