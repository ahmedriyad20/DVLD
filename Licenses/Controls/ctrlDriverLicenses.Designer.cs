namespace DVLD_Project.Licenses.Controls
{
    partial class ctrlDriverLicenses
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tcDriverLicenses = new System.Windows.Forms.TabControl();
            this.tbLocalDriverLicenses = new System.Windows.Forms.TabPage();
            this.lblLocalLicensesRecord = new System.Windows.Forms.Label();
            this.dgvLocalDriverLicensesHistory = new System.Windows.Forms.DataGridView();
            this.cmsLocalLicensesHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showLicenseInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbInternationalDriverLicenses = new System.Windows.Forms.TabPage();
            this.lblInternationalLicensesRecord = new System.Windows.Forms.Label();
            this.dgvInternationalDriverLicensesHistory = new System.Windows.Forms.DataGridView();
            this.cmsInternationalLicensesHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showLicenseInfoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tcDriverLicenses.SuspendLayout();
            this.tbLocalDriverLicenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalDriverLicensesHistory)).BeginInit();
            this.cmsLocalLicensesHistory.SuspendLayout();
            this.tbInternationalDriverLicenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInternationalDriverLicensesHistory)).BeginInit();
            this.cmsInternationalLicensesHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tcDriverLicenses);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1175, 328);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Driver Licenses";
            // 
            // tcDriverLicenses
            // 
            this.tcDriverLicenses.Controls.Add(this.tbLocalDriverLicenses);
            this.tcDriverLicenses.Controls.Add(this.tbInternationalDriverLicenses);
            this.tcDriverLicenses.Location = new System.Drawing.Point(-3, 33);
            this.tcDriverLicenses.Name = "tcDriverLicenses";
            this.tcDriverLicenses.SelectedIndex = 0;
            this.tcDriverLicenses.Size = new System.Drawing.Size(1178, 298);
            this.tcDriverLicenses.TabIndex = 0;
            // 
            // tbLocalDriverLicenses
            // 
            this.tbLocalDriverLicenses.BackColor = System.Drawing.Color.White;
            this.tbLocalDriverLicenses.Controls.Add(this.lblLocalLicensesRecord);
            this.tbLocalDriverLicenses.Controls.Add(this.dgvLocalDriverLicensesHistory);
            this.tbLocalDriverLicenses.Controls.Add(this.label2);
            this.tbLocalDriverLicenses.Controls.Add(this.label1);
            this.tbLocalDriverLicenses.Location = new System.Drawing.Point(4, 34);
            this.tbLocalDriverLicenses.Name = "tbLocalDriverLicenses";
            this.tbLocalDriverLicenses.Padding = new System.Windows.Forms.Padding(3);
            this.tbLocalDriverLicenses.Size = new System.Drawing.Size(1170, 260);
            this.tbLocalDriverLicenses.TabIndex = 0;
            this.tbLocalDriverLicenses.Text = "Local";
            // 
            // lblLocalLicensesRecord
            // 
            this.lblLocalLicensesRecord.AutoSize = true;
            this.lblLocalLicensesRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalLicensesRecord.Location = new System.Drawing.Point(125, 230);
            this.lblLocalLicensesRecord.Name = "lblLocalLicensesRecord";
            this.lblLocalLicensesRecord.Size = new System.Drawing.Size(48, 25);
            this.lblLocalLicensesRecord.TabIndex = 4;
            this.lblLocalLicensesRecord.Text = "???";
            // 
            // dgvLocalDriverLicensesHistory
            // 
            this.dgvLocalDriverLicensesHistory.AllowUserToAddRows = false;
            this.dgvLocalDriverLicensesHistory.AllowUserToDeleteRows = false;
            this.dgvLocalDriverLicensesHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvLocalDriverLicensesHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocalDriverLicensesHistory.ContextMenuStrip = this.cmsLocalLicensesHistory;
            this.dgvLocalDriverLicensesHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLocalDriverLicensesHistory.Location = new System.Drawing.Point(3, 44);
            this.dgvLocalDriverLicensesHistory.MultiSelect = false;
            this.dgvLocalDriverLicensesHistory.Name = "dgvLocalDriverLicensesHistory";
            this.dgvLocalDriverLicensesHistory.ReadOnly = true;
            this.dgvLocalDriverLicensesHistory.RowHeadersWidth = 51;
            this.dgvLocalDriverLicensesHistory.RowTemplate.Height = 24;
            this.dgvLocalDriverLicensesHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLocalDriverLicensesHistory.Size = new System.Drawing.Size(1165, 183);
            this.dgvLocalDriverLicensesHistory.TabIndex = 1;
            // 
            // cmsLocalLicensesHistory
            // 
            this.cmsLocalLicensesHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsLocalLicensesHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLicenseInfoToolStripMenuItem});
            this.cmsLocalLicensesHistory.Name = "cmsApplications";
            this.cmsLocalLicensesHistory.Size = new System.Drawing.Size(213, 42);
            // 
            // showLicenseInfoToolStripMenuItem
            // 
            this.showLicenseInfoToolStripMenuItem.Image = global::DVLD_Project.Properties.Resources.License_View_32;
            this.showLicenseInfoToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showLicenseInfoToolStripMenuItem.Name = "showLicenseInfoToolStripMenuItem";
            this.showLicenseInfoToolStripMenuItem.Size = new System.Drawing.Size(212, 38);
            this.showLicenseInfoToolStripMenuItem.Text = "Show License Info";
            this.showLicenseInfoToolStripMenuItem.Click += new System.EventHandler(this.showLicenseInfoToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "# Records:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Local Licenses History:";
            // 
            // tbInternationalDriverLicenses
            // 
            this.tbInternationalDriverLicenses.BackColor = System.Drawing.Color.White;
            this.tbInternationalDriverLicenses.Controls.Add(this.lblInternationalLicensesRecord);
            this.tbInternationalDriverLicenses.Controls.Add(this.dgvInternationalDriverLicensesHistory);
            this.tbInternationalDriverLicenses.Controls.Add(this.label4);
            this.tbInternationalDriverLicenses.Controls.Add(this.label5);
            this.tbInternationalDriverLicenses.Location = new System.Drawing.Point(4, 34);
            this.tbInternationalDriverLicenses.Name = "tbInternationalDriverLicenses";
            this.tbInternationalDriverLicenses.Padding = new System.Windows.Forms.Padding(3);
            this.tbInternationalDriverLicenses.Size = new System.Drawing.Size(1170, 251);
            this.tbInternationalDriverLicenses.TabIndex = 1;
            this.tbInternationalDriverLicenses.Text = "International";
            // 
            // lblInternationalLicensesRecord
            // 
            this.lblInternationalLicensesRecord.AutoSize = true;
            this.lblInternationalLicensesRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInternationalLicensesRecord.Location = new System.Drawing.Point(129, 230);
            this.lblInternationalLicensesRecord.Name = "lblInternationalLicensesRecord";
            this.lblInternationalLicensesRecord.Size = new System.Drawing.Size(48, 25);
            this.lblInternationalLicensesRecord.TabIndex = 8;
            this.lblInternationalLicensesRecord.Text = "???";
            // 
            // dgvInternationalDriverLicensesHistory
            // 
            this.dgvInternationalDriverLicensesHistory.AllowUserToAddRows = false;
            this.dgvInternationalDriverLicensesHistory.AllowUserToDeleteRows = false;
            this.dgvInternationalDriverLicensesHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvInternationalDriverLicensesHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInternationalDriverLicensesHistory.ContextMenuStrip = this.cmsInternationalLicensesHistory;
            this.dgvInternationalDriverLicensesHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvInternationalDriverLicensesHistory.Location = new System.Drawing.Point(12, 43);
            this.dgvInternationalDriverLicensesHistory.MultiSelect = false;
            this.dgvInternationalDriverLicensesHistory.Name = "dgvInternationalDriverLicensesHistory";
            this.dgvInternationalDriverLicensesHistory.ReadOnly = true;
            this.dgvInternationalDriverLicensesHistory.RowHeadersWidth = 51;
            this.dgvInternationalDriverLicensesHistory.RowTemplate.Height = 24;
            this.dgvInternationalDriverLicensesHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInternationalDriverLicensesHistory.Size = new System.Drawing.Size(1147, 184);
            this.dgvInternationalDriverLicensesHistory.TabIndex = 6;
            // 
            // cmsInternationalLicensesHistory
            // 
            this.cmsInternationalLicensesHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsInternationalLicensesHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLicenseInfoToolStripMenuItem1});
            this.cmsInternationalLicensesHistory.Name = "cmsInternationalLicensesHistory";
            this.cmsInternationalLicensesHistory.Size = new System.Drawing.Size(213, 42);
            // 
            // showLicenseInfoToolStripMenuItem1
            // 
            this.showLicenseInfoToolStripMenuItem1.Image = global::DVLD_Project.Properties.Resources.License_View_321;
            this.showLicenseInfoToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showLicenseInfoToolStripMenuItem1.Name = "showLicenseInfoToolStripMenuItem1";
            this.showLicenseInfoToolStripMenuItem1.Size = new System.Drawing.Size(212, 38);
            this.showLicenseInfoToolStripMenuItem1.Text = "Show License Info";
            this.showLicenseInfoToolStripMenuItem1.Click += new System.EventHandler(this.showLicenseInfoToolStripMenuItem1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "# Records:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(303, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "International Licenses History:";
            // 
            // ctrlDriverLicenses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox1);
            this.Name = "ctrlDriverLicenses";
            this.Size = new System.Drawing.Size(1179, 337);
            this.groupBox1.ResumeLayout(false);
            this.tcDriverLicenses.ResumeLayout(false);
            this.tbLocalDriverLicenses.ResumeLayout(false);
            this.tbLocalDriverLicenses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalDriverLicensesHistory)).EndInit();
            this.cmsLocalLicensesHistory.ResumeLayout(false);
            this.tbInternationalDriverLicenses.ResumeLayout(false);
            this.tbInternationalDriverLicenses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInternationalDriverLicensesHistory)).EndInit();
            this.cmsInternationalLicensesHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tcDriverLicenses;
        private System.Windows.Forms.TabPage tbLocalDriverLicenses;
        private System.Windows.Forms.TabPage tbInternationalDriverLicenses;
        private System.Windows.Forms.DataGridView dgvLocalDriverLicensesHistory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLocalLicensesRecord;
        private System.Windows.Forms.Label lblInternationalLicensesRecord;
        private System.Windows.Forms.DataGridView dgvInternationalDriverLicensesHistory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip cmsLocalLicensesHistory;
        private System.Windows.Forms.ToolStripMenuItem showLicenseInfoToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsInternationalLicensesHistory;
        private System.Windows.Forms.ToolStripMenuItem showLicenseInfoToolStripMenuItem1;
    }
}
