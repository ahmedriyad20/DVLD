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

namespace DVLD_Project.Applications.Release_Detained_License
{
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _SelectedLicenseID = -1;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();

        }

        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();

            _SelectedLicenseID = LicenseID;

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {

        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int ApplicationID = -1;

            bool IsReleased = false;

            IsReleased = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID,
                ref ApplicationID);

            if(!IsReleased)
            {
                MessageBox.Show("Failed to Release the Detained License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = ApplicationID.ToString();
            MessageBox.Show("Detained License Released Successfully with Application ID = " + ApplicationID.ToString(), "Released Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnReleaseLicense.Enabled = false;
            llShowLicensesInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;

            lblLicenseID.Text = _SelectedLicenseID.ToString();

            llShowLicensesHistory.Enabled = (_SelectedLicenseID != -1);

            if (_SelectedLicenseID == -1)
            {
                return;
            }

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is not detained! Choose another one", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblDetainID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainID.ToString();
                return;
            }

            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees.ToString();
            lblDetainDate.Text = clsFormat.DateToShort(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainDate);
            lblLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID.ToString();

            lblCreatedByUserID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.CreatedByUserID.ToString();
            lblDetainID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainID.ToString();
            lblFineFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text) + Convert.ToInt32(lblFineFees.Text)).ToString();

            btnReleaseLicense.Enabled = true;
        }
    }
}
