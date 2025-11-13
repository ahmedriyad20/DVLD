namespace DVLD_Project.Licenses
{
    partial class frmShowPersonLicensesHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowPersonLicensesHistory));
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlDriverLicenses1 = new DVLD_Project.Licenses.Controls.ctrlDriverLicenses();
            this.pbPeopleImage = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ctrlPersonCardWithFilter1 = new DVLD_Project.People.Controls.ctrlPersonCardWithFilter();
            ((System.ComponentModel.ISupportInitialize)(this.pbPeopleImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Image = global::DVLD_Project.Properties.Resources.Close_32;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrlDriverLicenses1
            // 
            this.ctrlDriverLicenses1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.ctrlDriverLicenses1, "ctrlDriverLicenses1");
            this.ctrlDriverLicenses1.Name = "ctrlDriverLicenses1";
            // 
            // pbPeopleImage
            // 
            this.pbPeopleImage.Image = global::DVLD_Project.Properties.Resources.PersonLicenseHistory_512;
            resources.ApplyResources(this.pbPeopleImage, "pbPeopleImage");
            this.pbPeopleImage.Name = "pbPeopleImage";
            this.pbPeopleImage.TabStop = false;
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Name = "lblTitle";
            // 
            // ctrlPersonCardWithFilter1
            // 
            this.ctrlPersonCardWithFilter1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ctrlPersonCardWithFilter1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.ctrlPersonCardWithFilter1, "ctrlPersonCardWithFilter1");
            this.ctrlPersonCardWithFilter1.Name = "ctrlPersonCardWithFilter1";
            this.ctrlPersonCardWithFilter1.OnPersonSelected += new System.Action<int>(this.ctrlPersonCardWithFilter1_OnPersonSelected);
            // 
            // frmShowPersonLicensesHistory
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlDriverLicenses1);
            this.Controls.Add(this.pbPeopleImage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.ctrlPersonCardWithFilter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmShowPersonLicensesHistory";
            this.Load += new System.EventHandler(this.frmShowPersonLicensesHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbPeopleImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private Controls.ctrlDriverLicenses ctrlDriverLicenses1;
        private System.Windows.Forms.PictureBox pbPeopleImage;
        private System.Windows.Forms.Label lblTitle;
        private People.Controls.ctrlPersonCardWithFilter ctrlPersonCardWithFilter1;
    }
}