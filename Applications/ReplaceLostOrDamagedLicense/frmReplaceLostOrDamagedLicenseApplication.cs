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
using static DVLD_BusinessLayer.clsLicense;

namespace DVLD_Project.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private int _NewReplacedLicense = -1;

        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private int _GetApplicationTypeID()
        { 

            if (rbDamagedLicnese.Checked)

                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
        }

        private clsLicense.enIssueReason _GetIssueReason()
        {
            if (rbDamagedLicnese.Checked)
                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            rbDamagedLicnese.Checked = true;

            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString();
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Issue a Replacement for the License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReplaceLicense(_GetIssueReason(),
                 _GetApplicationTypeID(), clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Failed to Renew the License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            //{
            //    MessageBox.Show("The old license is still active, please deactivate it manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}

            _NewReplacedLicense = NewLicense.LicenseID;

            lblLocalReplacemetApplicationID.Text = NewLicense.ApplicationID.ToString();
            lblReplacedLicenseID.Text = NewLicense.LicenseID.ToString();
            MessageBox.Show("License Replaced Successfully with ID = " + NewLicense.LicenseID, "License Replaced", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowNewLicensesInfo.Enabled = true;
            btnIssueReplacementLicense.Enabled = false;
            gbReplacementFor.Enabled = false;
        }

        private void rbDamagedLicnese_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = (rbDamagedLicnese.Checked) ? clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString()
                : clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees.ToString();

            lblTitle.Text = (rbDamagedLicnese.Checked) ? "Replacement For Damaged License" : "Replacement For Lost License";
            this.Text = lblTitle.Text;
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = (rbDamagedLicnese.Checked) ? clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString()
                : clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees.ToString();

            lblTitle.Text = (rbDamagedLicnese.Checked) ? "Replacement For Damaged License" : "Replacement For Lost License";
            this.Text = lblTitle.Text;

        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
            frm.ShowDialog();
        }

        private void llShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewReplacedLicense);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;

            lblOldLicenseID.Text = LicenseID.ToString();
            llShowLicensesHistory.Enabled = (LicenseID != -1);

            if (LicenseID == -1)
                return;

            // Check if the selected license is expired
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is expired, You have to renew it first: ", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacementLicense.Enabled = false;
                return;
            }

            // Check if the selected license is active
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("License is not active, Choose another one!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacementLicense.Enabled = false;
                return;
            }

            // Check if the selected license is detained
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("License is detained!, Release it first", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacementLicense.Enabled = false;
                return;
            }

            btnIssueReplacementLicense.Enabled = true;
        }
    }
}
