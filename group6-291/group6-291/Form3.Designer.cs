namespace group6_291
{
    partial class Form3
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
            this.WardChangeLabel = new System.Windows.Forms.Label();
            this.RegListBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SubmitNewWard = new System.Windows.Forms.Button();
            this.NewWardBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // WardChangeLabel
            // 
            this.WardChangeLabel.AutoSize = true;
            this.WardChangeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.WardChangeLabel.Location = new System.Drawing.Point(571, 9);
            this.WardChangeLabel.Name = "WardChangeLabel";
            this.WardChangeLabel.Size = new System.Drawing.Size(233, 39);
            this.WardChangeLabel.TabIndex = 0;
            this.WardChangeLabel.Text = "Ward Transfer";
            this.WardChangeLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // RegListBox
            // 
            this.RegListBox.FormattingEnabled = true;
            this.RegListBox.ItemHeight = 16;
            this.RegListBox.Location = new System.Drawing.Point(42, 147);
            this.RegListBox.Name = "RegListBox";
            this.RegListBox.Size = new System.Drawing.Size(157, 292);
            this.RegListBox.TabIndex = 1;
            this.RegListBox.SelectedIndexChanged += new System.EventHandler(this.RegListBox_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(288, 82);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(411, 357);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SubmitNewWard);
            this.tabPage1.Controls.Add(this.NewWardBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(403, 328);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ChangeWard";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SubmitNewWard
            // 
            this.SubmitNewWard.Location = new System.Drawing.Point(28, 291);
            this.SubmitNewWard.Name = "SubmitNewWard";
            this.SubmitNewWard.Size = new System.Drawing.Size(75, 23);
            this.SubmitNewWard.TabIndex = 2;
            this.SubmitNewWard.Text = "Submit";
            this.SubmitNewWard.UseVisualStyleBackColor = true;
            this.SubmitNewWard.Click += new System.EventHandler(this.SubmitNewWard_Click);
            // 
            // NewWardBox
            // 
            this.NewWardBox.FormattingEnabled = true;
            this.NewWardBox.Location = new System.Drawing.Point(10, 41);
            this.NewWardBox.Name = "NewWardBox";
            this.NewWardBox.Size = new System.Drawing.Size(121, 24);
            this.NewWardBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "NewWard";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(403, 328);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Patient";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 480);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.RegListBox);
            this.Controls.Add(this.WardChangeLabel);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WardChangeLabel;
        private System.Windows.Forms.ListBox RegListBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button SubmitNewWard;
        private System.Windows.Forms.ComboBox NewWardBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}