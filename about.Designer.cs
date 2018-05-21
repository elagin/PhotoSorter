namespace PhotoSorter
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.linkLabelVK = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // linkLabelVK
            // 
            this.linkLabelVK.AutoSize = true;
            this.linkLabelVK.Location = new System.Drawing.Point(151, 163);
            this.linkLabelVK.Name = "linkLabelVK";
            this.linkLabelVK.Size = new System.Drawing.Size(42, 13);
            this.linkLabelVK.TabIndex = 0;
            this.linkLabelVK.TabStop = true;
            this.linkLabelVK.Text = "vk.com";
            this.linkLabelVK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelVK_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.linkLabelVK);
            this.Name = "AboutForm";
            this.Text = "about";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabelVK;
    }
}