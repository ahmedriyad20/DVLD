using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;
using DVLD_Project.Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Local_Licenses;

namespace DVLD_Project.Applications.Renew_Local_License
{
    public partial class frmRenewLocalLicense : Form
    {
        private int _NewLicenseID = -1;

        public frmRenewLocalLicense()
        {
            InitializeComponent();
        }

        private void frmRenewLocalLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees.ToString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to renew the License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text,
                clsGlobal.CurrentUser.UserID);

            if(NewLicense == null)
            {
                MessageBox.Show("Failed to Renew the License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            //{
            //    MessageBox.Show("The old license is still active, please deactivate it manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

            _NewLicenseID = NewLicense.LicenseID;

            lblRenewLocalApplicationID.Text = NewLicense.ApplicationID.ToString();
            lblRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
            MessageBox.Show("License Renewed Successfully with ID = " + NewLicense.LicenseID, "License Renewed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowNewLicensesInfo.Enabled = true;
            btnRenewLicense.Enabled = false;


            
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;

            llShowLicensesHistory.Enabled = (LicenseID != -1);

            if (LicenseID == -1)
                return;

            int DefaultValidityLength = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength;

            lblOldLicenseID.Text = LicenseID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text) + Convert.ToInt32(lblLicenseFees.Text)).ToString();
            txtNotes.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;

            // Check if the selected license is expired
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Select License is not yet expired, It will expire on: " + ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate.ToShortDateString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            // Check if the selected license is active
            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("License is not active, Choose another one!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            // Check if the selected license is detained
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("License is detained!, Release it first", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            btnRenewLicense.Enabled = true;
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
            frm.ShowDialog();
            
        }

        private void llShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
