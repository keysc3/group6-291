namespace group6_291
{
    partial class AdminHome
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
            this.PatientButton = new System.Windows.Forms.Button();
            this.AccountButton = new System.Windows.Forms.Button();
            this.PatientRecordButton = new System.Windows.Forms.Button();
            this.WardButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PatientButton
            // 
            this.PatientButton.Location = new System.Drawing.Point(214, 86);
            this.PatientButton.Name = "PatientButton";
            this.PatientButton.Size = new System.Drawing.Size(149, 48);
            this.PatientButton.TabIndex = 0;
            this.PatientButton.Text = "Patient Registration";
            this.PatientButton.UseVisualStyleBackColor = true;
            PatientButton.Click += new System.EventHandler(this.PatientButton_Click);
            // 
            // AccountButton
            // 
            this.AccountButton.Location = new System.Drawing.Point(30, 86);
            this.AccountButton.Name = "AccountButton";
            this.AccountButton.Size = new System.Drawing.Size(149, 48);
            this.AccountButton.TabIndex = 1;
            this.AccountButton.Text = "Account Details";
            this.AccountButton.UseVisualStyleBackColor = true;
            this.AccountButton.Click += new System.EventHandler(this.AccountButton_Click);
            // 
            // PatientRecordButton
            // 
            this.PatientRecordButton.Location = new System.Drawing.Point(399, 85);
            this.PatientRecordButton.Name = "PatientRecordButton";
            this.PatientRecordButton.Size = new System.Drawing.Size(149, 49);
            this.PatientRecordButton.TabIndex = 2;
            this.PatientRecordButton.Text = "Patient Records";
            this.PatientRecordButton.UseVisualStyleBackColor = true;
            this.PatientRecordButton.Click += new System.EventHandler(this.PatientRecordButton_Click);
            // 
            // WardButton
            // 
            this.WardButton.Location = new System.Drawing.Point(214, 163);
            this.WardButton.Name = "WardButton";
            this.WardButton.Size = new System.Drawing.Size(149, 48);
            this.WardButton.TabIndex = 3;
            this.WardButton.Text = "Ward Records";
            this.WardButton.UseVisualStyleBackColor = true;
            this.WardButton.Click += new System.EventHandler(this.WardButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label1.Location = new System.Drawing.Point(208, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 31);
            this.label1.TabIndex = 4;
            this.label1.Text = "Admin Tools";
            // 
            // AdminHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 240);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.WardButton);
            this.Controls.Add(this.PatientRecordButton);
            this.Controls.Add(this.AccountButton);
            this.Controls.Add(this.PatientButton);
            this.Name = "AdminHome";
            this.Text = "AdminHome";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PatientButton;
        private System.Windows.Forms.Button AccountButton;
        private System.Windows.Forms.Button PatientRecordButton;
        private System.Windows.Forms.Button WardButton;
        private System.Windows.Forms.Label label1;
    }
}