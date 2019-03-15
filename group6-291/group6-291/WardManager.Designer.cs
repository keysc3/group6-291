namespace group6_291
{
    partial class WardManager
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
            this.wardManagementTitle = new System.Windows.Forms.Label();
            this.wardListBox = new System.Windows.Forms.ListBox();
            this.wardListLabel = new System.Windows.Forms.Label();
            this.wardListScrollBar = new System.Windows.Forms.VScrollBar();
            this.wardActionTabs = new System.Windows.Forms.TabControl();
            this.addWardTab = new System.Windows.Forms.TabPage();
            this.updateWardTab = new System.Windows.Forms.TabPage();
            this.wardActionLabel = new System.Windows.Forms.Label();
            this.addWardStatus = new System.Windows.Forms.Label();
            this.addWardStatusBox = new System.Windows.Forms.TextBox();
            this.addWardType = new System.Windows.Forms.Label();
            this.addWardName = new System.Windows.Forms.Label();
            this.AddWardCapacityBox = new System.Windows.Forms.TextBox();
            this.addWardNameBox = new System.Windows.Forms.TextBox();
            this.addWardButton = new System.Windows.Forms.Button();
            this.updateWardStatus = new System.Windows.Forms.Label();
            this.updateWardStatusBox = new System.Windows.Forms.TextBox();
            this.updateWardCapacity = new System.Windows.Forms.Label();
            this.updateWardName = new System.Windows.Forms.Label();
            this.updateWardCapacityBox = new System.Windows.Forms.TextBox();
            this.updateWardNameBox = new System.Windows.Forms.TextBox();
            this.updateWardButton = new System.Windows.Forms.Button();
            this.updateSelectedWard = new System.Windows.Forms.Label();
            this.updateCurrentName = new System.Windows.Forms.Label();
            this.updateCurrentCapacity = new System.Windows.Forms.Label();
            this.updateSelectedCapacity = new System.Windows.Forms.Label();
            this.updateCurrentStatus = new System.Windows.Forms.Label();
            this.updateSelectedStatus = new System.Windows.Forms.Label();
            this.deleteWardTab = new System.Windows.Forms.TabPage();
            this.deleteCurrentStatus = new System.Windows.Forms.Label();
            this.deleteSelectedStatus = new System.Windows.Forms.Label();
            this.deleteCurrentCapacity = new System.Windows.Forms.Label();
            this.deleteSelectedCapacity = new System.Windows.Forms.Label();
            this.deleteCurrentName = new System.Windows.Forms.Label();
            this.deleteSelectedWard = new System.Windows.Forms.Label();
            this.deleteWardButton = new System.Windows.Forms.Button();
            this.wardActionTabs.SuspendLayout();
            this.addWardTab.SuspendLayout();
            this.updateWardTab.SuspendLayout();
            this.deleteWardTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // wardManagementTitle
            // 
            this.wardManagementTitle.AutoSize = true;
            this.wardManagementTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.wardManagementTitle.Location = new System.Drawing.Point(585, 9);
            this.wardManagementTitle.Name = "wardManagementTitle";
            this.wardManagementTitle.Size = new System.Drawing.Size(242, 31);
            this.wardManagementTitle.TabIndex = 1;
            this.wardManagementTitle.Text = "Ward Management";
            // 
            // wardListBox
            // 
            this.wardListBox.FormattingEnabled = true;
            this.wardListBox.Location = new System.Drawing.Point(12, 76);
            this.wardListBox.Name = "wardListBox";
            this.wardListBox.Size = new System.Drawing.Size(182, 433);
            this.wardListBox.TabIndex = 37;
            // 
            // wardListLabel
            // 
            this.wardListLabel.AutoSize = true;
            this.wardListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.wardListLabel.Location = new System.Drawing.Point(40, 35);
            this.wardListLabel.Name = "wardListLabel";
            this.wardListLabel.Size = new System.Drawing.Size(92, 24);
            this.wardListLabel.TabIndex = 36;
            this.wardListLabel.Text = "Ward List:";
            // 
            // wardListScrollBar
            // 
            this.wardListScrollBar.Location = new System.Drawing.Point(211, 76);
            this.wardListScrollBar.Name = "wardListScrollBar";
            this.wardListScrollBar.Size = new System.Drawing.Size(17, 433);
            this.wardListScrollBar.TabIndex = 35;
            // 
            // wardActionTabs
            // 
            this.wardActionTabs.Controls.Add(this.addWardTab);
            this.wardActionTabs.Controls.Add(this.updateWardTab);
            this.wardActionTabs.Controls.Add(this.deleteWardTab);
            this.wardActionTabs.Location = new System.Drawing.Point(248, 76);
            this.wardActionTabs.Name = "wardActionTabs";
            this.wardActionTabs.SelectedIndex = 0;
            this.wardActionTabs.Size = new System.Drawing.Size(584, 433);
            this.wardActionTabs.TabIndex = 39;
            // 
            // addWardTab
            // 
            this.addWardTab.Controls.Add(this.addWardButton);
            this.addWardTab.Controls.Add(this.addWardStatus);
            this.addWardTab.Controls.Add(this.addWardStatusBox);
            this.addWardTab.Controls.Add(this.addWardType);
            this.addWardTab.Controls.Add(this.addWardName);
            this.addWardTab.Controls.Add(this.AddWardCapacityBox);
            this.addWardTab.Controls.Add(this.addWardNameBox);
            this.addWardTab.Location = new System.Drawing.Point(4, 22);
            this.addWardTab.Name = "addWardTab";
            this.addWardTab.Padding = new System.Windows.Forms.Padding(3);
            this.addWardTab.Size = new System.Drawing.Size(576, 407);
            this.addWardTab.TabIndex = 0;
            this.addWardTab.Text = "Add";
            this.addWardTab.UseVisualStyleBackColor = true;
            // 
            // updateWardTab
            // 
            this.updateWardTab.Controls.Add(this.updateCurrentStatus);
            this.updateWardTab.Controls.Add(this.updateSelectedStatus);
            this.updateWardTab.Controls.Add(this.updateCurrentCapacity);
            this.updateWardTab.Controls.Add(this.updateSelectedCapacity);
            this.updateWardTab.Controls.Add(this.updateCurrentName);
            this.updateWardTab.Controls.Add(this.updateSelectedWard);
            this.updateWardTab.Controls.Add(this.updateWardButton);
            this.updateWardTab.Controls.Add(this.updateWardStatus);
            this.updateWardTab.Controls.Add(this.updateWardStatusBox);
            this.updateWardTab.Controls.Add(this.updateWardCapacity);
            this.updateWardTab.Controls.Add(this.updateWardName);
            this.updateWardTab.Controls.Add(this.updateWardCapacityBox);
            this.updateWardTab.Controls.Add(this.updateWardNameBox);
            this.updateWardTab.Location = new System.Drawing.Point(4, 22);
            this.updateWardTab.Name = "updateWardTab";
            this.updateWardTab.Padding = new System.Windows.Forms.Padding(3);
            this.updateWardTab.Size = new System.Drawing.Size(576, 407);
            this.updateWardTab.TabIndex = 1;
            this.updateWardTab.Text = "Update";
            this.updateWardTab.UseVisualStyleBackColor = true;
            // 
            // wardActionLabel
            // 
            this.wardActionLabel.AutoSize = true;
            this.wardActionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.wardActionLabel.Location = new System.Drawing.Point(244, 35);
            this.wardActionLabel.Name = "wardActionLabel";
            this.wardActionLabel.Size = new System.Drawing.Size(122, 24);
            this.wardActionLabel.TabIndex = 23;
            this.wardActionLabel.Text = "Ward Actions";
            this.wardActionLabel.Click += new System.EventHandler(this.wardActionLabel_Click);
            // 
            // addWardStatus
            // 
            this.addWardStatus.AutoSize = true;
            this.addWardStatus.Location = new System.Drawing.Point(3, 93);
            this.addWardStatus.Name = "addWardStatus";
            this.addWardStatus.Size = new System.Drawing.Size(40, 13);
            this.addWardStatus.TabIndex = 46;
            this.addWardStatus.Text = "Status:";
            // 
            // addWardStatusBox
            // 
            this.addWardStatusBox.Location = new System.Drawing.Point(6, 109);
            this.addWardStatusBox.Name = "addWardStatusBox";
            this.addWardStatusBox.Size = new System.Drawing.Size(100, 20);
            this.addWardStatusBox.TabIndex = 45;
            // 
            // addWardType
            // 
            this.addWardType.AutoSize = true;
            this.addWardType.Location = new System.Drawing.Point(3, 54);
            this.addWardType.Name = "addWardType";
            this.addWardType.Size = new System.Drawing.Size(80, 13);
            this.addWardType.TabIndex = 44;
            this.addWardType.Text = "Ward Capacity:";
            this.addWardType.Click += new System.EventHandler(this.addWardType_Click);
            // 
            // addWardName
            // 
            this.addWardName.AutoSize = true;
            this.addWardName.Location = new System.Drawing.Point(3, 15);
            this.addWardName.Name = "addWardName";
            this.addWardName.Size = new System.Drawing.Size(67, 13);
            this.addWardName.TabIndex = 43;
            this.addWardName.Text = "Ward Name:";
            // 
            // AddWardCapacityBox
            // 
            this.AddWardCapacityBox.Location = new System.Drawing.Point(6, 70);
            this.AddWardCapacityBox.Name = "AddWardCapacityBox";
            this.AddWardCapacityBox.Size = new System.Drawing.Size(100, 20);
            this.AddWardCapacityBox.TabIndex = 42;
            // 
            // addWardNameBox
            // 
            this.addWardNameBox.Location = new System.Drawing.Point(6, 31);
            this.addWardNameBox.Name = "addWardNameBox";
            this.addWardNameBox.Size = new System.Drawing.Size(100, 20);
            this.addWardNameBox.TabIndex = 41;
            // 
            // addWardButton
            // 
            this.addWardButton.Location = new System.Drawing.Point(495, 378);
            this.addWardButton.Name = "addWardButton";
            this.addWardButton.Size = new System.Drawing.Size(75, 23);
            this.addWardButton.TabIndex = 47;
            this.addWardButton.Text = "Add Ward";
            this.addWardButton.UseVisualStyleBackColor = true;
            // 
            // updateWardStatus
            // 
            this.updateWardStatus.AutoSize = true;
            this.updateWardStatus.Location = new System.Drawing.Point(7, 199);
            this.updateWardStatus.Name = "updateWardStatus";
            this.updateWardStatus.Size = new System.Drawing.Size(65, 13);
            this.updateWardStatus.TabIndex = 52;
            this.updateWardStatus.Text = "New Status:";
            // 
            // updateWardStatusBox
            // 
            this.updateWardStatusBox.Location = new System.Drawing.Point(10, 215);
            this.updateWardStatusBox.Name = "updateWardStatusBox";
            this.updateWardStatusBox.Size = new System.Drawing.Size(100, 20);
            this.updateWardStatusBox.TabIndex = 51;
            // 
            // updateWardCapacity
            // 
            this.updateWardCapacity.AutoSize = true;
            this.updateWardCapacity.Location = new System.Drawing.Point(7, 160);
            this.updateWardCapacity.Name = "updateWardCapacity";
            this.updateWardCapacity.Size = new System.Drawing.Size(76, 13);
            this.updateWardCapacity.TabIndex = 50;
            this.updateWardCapacity.Text = "New Capacity:";
            // 
            // updateWardName
            // 
            this.updateWardName.AutoSize = true;
            this.updateWardName.Location = new System.Drawing.Point(7, 121);
            this.updateWardName.Name = "updateWardName";
            this.updateWardName.Size = new System.Drawing.Size(63, 13);
            this.updateWardName.TabIndex = 49;
            this.updateWardName.Text = "New Name:";
            this.updateWardName.Click += new System.EventHandler(this.updateWardName_Click);
            // 
            // updateWardCapacityBox
            // 
            this.updateWardCapacityBox.Location = new System.Drawing.Point(10, 176);
            this.updateWardCapacityBox.Name = "updateWardCapacityBox";
            this.updateWardCapacityBox.Size = new System.Drawing.Size(100, 20);
            this.updateWardCapacityBox.TabIndex = 48;
            // 
            // updateWardNameBox
            // 
            this.updateWardNameBox.Location = new System.Drawing.Point(10, 137);
            this.updateWardNameBox.Name = "updateWardNameBox";
            this.updateWardNameBox.Size = new System.Drawing.Size(100, 20);
            this.updateWardNameBox.TabIndex = 47;
            // 
            // updateWardButton
            // 
            this.updateWardButton.Location = new System.Drawing.Point(495, 378);
            this.updateWardButton.Name = "updateWardButton";
            this.updateWardButton.Size = new System.Drawing.Size(75, 23);
            this.updateWardButton.TabIndex = 53;
            this.updateWardButton.Text = "Update Ward";
            this.updateWardButton.UseVisualStyleBackColor = true;
            this.updateWardButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // updateSelectedWard
            // 
            this.updateSelectedWard.AutoSize = true;
            this.updateSelectedWard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateSelectedWard.Location = new System.Drawing.Point(6, 15);
            this.updateSelectedWard.Name = "updateSelectedWard";
            this.updateSelectedWard.Size = new System.Drawing.Size(122, 20);
            this.updateSelectedWard.TabIndex = 54;
            this.updateSelectedWard.Text = "Selected Ward: ";
            // 
            // updateCurrentName
            // 
            this.updateCurrentName.AutoSize = true;
            this.updateCurrentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateCurrentName.Location = new System.Drawing.Point(133, 15);
            this.updateCurrentName.Name = "updateCurrentName";
            this.updateCurrentName.Size = new System.Drawing.Size(51, 20);
            this.updateCurrentName.TabIndex = 55;
            this.updateCurrentName.Text = "Name";
            // 
            // updateCurrentCapacity
            // 
            this.updateCurrentCapacity.AutoSize = true;
            this.updateCurrentCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateCurrentCapacity.Location = new System.Drawing.Point(134, 44);
            this.updateCurrentCapacity.Name = "updateCurrentCapacity";
            this.updateCurrentCapacity.Size = new System.Drawing.Size(29, 13);
            this.updateCurrentCapacity.TabIndex = 57;
            this.updateCurrentCapacity.Text = "Num";
            this.updateCurrentCapacity.Click += new System.EventHandler(this.label2_Click);
            // 
            // updateSelectedCapacity
            // 
            this.updateSelectedCapacity.AutoSize = true;
            this.updateSelectedCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateSelectedCapacity.Location = new System.Drawing.Point(6, 44);
            this.updateSelectedCapacity.Name = "updateSelectedCapacity";
            this.updateSelectedCapacity.Size = new System.Drawing.Size(51, 13);
            this.updateSelectedCapacity.TabIndex = 56;
            this.updateSelectedCapacity.Text = "Capacity:";
            this.updateSelectedCapacity.Click += new System.EventHandler(this.label3_Click);
            // 
            // updateCurrentStatus
            // 
            this.updateCurrentStatus.AutoSize = true;
            this.updateCurrentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateCurrentStatus.Location = new System.Drawing.Point(134, 70);
            this.updateCurrentStatus.Name = "updateCurrentStatus";
            this.updateCurrentStatus.Size = new System.Drawing.Size(37, 13);
            this.updateCurrentStatus.TabIndex = 59;
            this.updateCurrentStatus.Text = "Status";
            // 
            // updateSelectedStatus
            // 
            this.updateSelectedStatus.AutoSize = true;
            this.updateSelectedStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateSelectedStatus.Location = new System.Drawing.Point(7, 70);
            this.updateSelectedStatus.Name = "updateSelectedStatus";
            this.updateSelectedStatus.Size = new System.Drawing.Size(40, 13);
            this.updateSelectedStatus.TabIndex = 58;
            this.updateSelectedStatus.Text = "Status:";
            // 
            // deleteWardTab
            // 
            this.deleteWardTab.Controls.Add(this.deleteWardButton);
            this.deleteWardTab.Controls.Add(this.deleteCurrentStatus);
            this.deleteWardTab.Controls.Add(this.deleteSelectedStatus);
            this.deleteWardTab.Controls.Add(this.deleteCurrentCapacity);
            this.deleteWardTab.Controls.Add(this.deleteSelectedCapacity);
            this.deleteWardTab.Controls.Add(this.deleteCurrentName);
            this.deleteWardTab.Controls.Add(this.deleteSelectedWard);
            this.deleteWardTab.Location = new System.Drawing.Point(4, 22);
            this.deleteWardTab.Name = "deleteWardTab";
            this.deleteWardTab.Size = new System.Drawing.Size(576, 407);
            this.deleteWardTab.TabIndex = 2;
            this.deleteWardTab.Text = "Delete";
            this.deleteWardTab.UseVisualStyleBackColor = true;
            // 
            // deleteCurrentStatus
            // 
            this.deleteCurrentStatus.AutoSize = true;
            this.deleteCurrentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteCurrentStatus.Location = new System.Drawing.Point(134, 70);
            this.deleteCurrentStatus.Name = "deleteCurrentStatus";
            this.deleteCurrentStatus.Size = new System.Drawing.Size(37, 13);
            this.deleteCurrentStatus.TabIndex = 65;
            this.deleteCurrentStatus.Text = "Status";
            // 
            // deleteSelectedStatus
            // 
            this.deleteSelectedStatus.AutoSize = true;
            this.deleteSelectedStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteSelectedStatus.Location = new System.Drawing.Point(7, 70);
            this.deleteSelectedStatus.Name = "deleteSelectedStatus";
            this.deleteSelectedStatus.Size = new System.Drawing.Size(40, 13);
            this.deleteSelectedStatus.TabIndex = 64;
            this.deleteSelectedStatus.Text = "Status:";
            // 
            // deleteCurrentCapacity
            // 
            this.deleteCurrentCapacity.AutoSize = true;
            this.deleteCurrentCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteCurrentCapacity.Location = new System.Drawing.Point(134, 44);
            this.deleteCurrentCapacity.Name = "deleteCurrentCapacity";
            this.deleteCurrentCapacity.Size = new System.Drawing.Size(29, 13);
            this.deleteCurrentCapacity.TabIndex = 63;
            this.deleteCurrentCapacity.Text = "Num";
            this.deleteCurrentCapacity.Click += new System.EventHandler(this.label3_Click_1);
            // 
            // deleteSelectedCapacity
            // 
            this.deleteSelectedCapacity.AutoSize = true;
            this.deleteSelectedCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteSelectedCapacity.Location = new System.Drawing.Point(6, 44);
            this.deleteSelectedCapacity.Name = "deleteSelectedCapacity";
            this.deleteSelectedCapacity.Size = new System.Drawing.Size(51, 13);
            this.deleteSelectedCapacity.TabIndex = 62;
            this.deleteSelectedCapacity.Text = "Capacity:";
            // 
            // deleteCurrentName
            // 
            this.deleteCurrentName.AutoSize = true;
            this.deleteCurrentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteCurrentName.Location = new System.Drawing.Point(133, 15);
            this.deleteCurrentName.Name = "deleteCurrentName";
            this.deleteCurrentName.Size = new System.Drawing.Size(51, 20);
            this.deleteCurrentName.TabIndex = 61;
            this.deleteCurrentName.Text = "Name";
            // 
            // deleteSelectedWard
            // 
            this.deleteSelectedWard.AutoSize = true;
            this.deleteSelectedWard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteSelectedWard.Location = new System.Drawing.Point(6, 15);
            this.deleteSelectedWard.Name = "deleteSelectedWard";
            this.deleteSelectedWard.Size = new System.Drawing.Size(122, 20);
            this.deleteSelectedWard.TabIndex = 60;
            this.deleteSelectedWard.Text = "Selected Ward: ";
            // 
            // deleteWardButton
            // 
            this.deleteWardButton.Location = new System.Drawing.Point(495, 378);
            this.deleteWardButton.Name = "deleteWardButton";
            this.deleteWardButton.Size = new System.Drawing.Size(75, 23);
            this.deleteWardButton.TabIndex = 66;
            this.deleteWardButton.Text = "Delete Ward";
            this.deleteWardButton.UseVisualStyleBackColor = true;
            // 
            // WardManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 521);
            this.Controls.Add(this.wardActionTabs);
            this.Controls.Add(this.wardListBox);
            this.Controls.Add(this.wardListLabel);
            this.Controls.Add(this.wardListScrollBar);
            this.Controls.Add(this.wardActionLabel);
            this.Controls.Add(this.wardManagementTitle);
            this.Name = "WardManager";
            this.Text = "WardManager";
            this.wardActionTabs.ResumeLayout(false);
            this.addWardTab.ResumeLayout(false);
            this.addWardTab.PerformLayout();
            this.updateWardTab.ResumeLayout(false);
            this.updateWardTab.PerformLayout();
            this.deleteWardTab.ResumeLayout(false);
            this.deleteWardTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label wardManagementTitle;
        private System.Windows.Forms.ListBox wardListBox;
        private System.Windows.Forms.Label wardListLabel;
        private System.Windows.Forms.VScrollBar wardListScrollBar;
        private System.Windows.Forms.TabControl wardActionTabs;
        private System.Windows.Forms.TabPage addWardTab;
        private System.Windows.Forms.TabPage updateWardTab;
        private System.Windows.Forms.Label wardActionLabel;
        private System.Windows.Forms.Label addWardStatus;
        private System.Windows.Forms.TextBox addWardStatusBox;
        private System.Windows.Forms.Label addWardType;
        private System.Windows.Forms.Label addWardName;
        private System.Windows.Forms.TextBox AddWardCapacityBox;
        private System.Windows.Forms.TextBox addWardNameBox;
        private System.Windows.Forms.Button addWardButton;
        private System.Windows.Forms.Button updateWardButton;
        private System.Windows.Forms.Label updateWardStatus;
        private System.Windows.Forms.TextBox updateWardStatusBox;
        private System.Windows.Forms.Label updateWardCapacity;
        private System.Windows.Forms.Label updateWardName;
        private System.Windows.Forms.TextBox updateWardCapacityBox;
        private System.Windows.Forms.TextBox updateWardNameBox;
        private System.Windows.Forms.Label updateCurrentCapacity;
        private System.Windows.Forms.Label updateSelectedCapacity;
        private System.Windows.Forms.Label updateCurrentName;
        private System.Windows.Forms.Label updateSelectedWard;
        private System.Windows.Forms.Label updateCurrentStatus;
        private System.Windows.Forms.Label updateSelectedStatus;
        private System.Windows.Forms.TabPage deleteWardTab;
        private System.Windows.Forms.Label deleteCurrentStatus;
        private System.Windows.Forms.Label deleteSelectedStatus;
        private System.Windows.Forms.Label deleteCurrentCapacity;
        private System.Windows.Forms.Label deleteSelectedCapacity;
        private System.Windows.Forms.Label deleteCurrentName;
        private System.Windows.Forms.Label deleteSelectedWard;
        private System.Windows.Forms.Button deleteWardButton;
    }
}