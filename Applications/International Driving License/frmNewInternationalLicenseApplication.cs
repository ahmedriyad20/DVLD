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
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Licenses.Local_Licenses.Control;

namespace DVLD_Project.Applications.International_Driving_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _SelectedLocalLicenseID = -1;
        private int _InternationalLicenseID = -1;

        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            //Creating a new Interonational License Application first
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees;
            Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(Application.Save())
            {
                lblApplicationID.Text = Application.ApplicationID.ToString();
            }
            else
            {
                MessageBox.Show("Couldn't Save the Application!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }

            //Now we create a new International License
            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            InternationalLicense.ApplicationID = Application.ApplicationID;
            InternationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            InternationalLicense.IsActive = true;
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(InternationalLicense.Save() )
            {
                MessageBox.Show("International License Issued Successfully with ID = " + InternationalLicense.InternationalLicenseID, "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
                llShowLicensesInfo.Enabled = true;
                btnIssueLicense.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
                lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to Issue the License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLocalLicenseID = obj;

            lblLocalLicenseID.Text = _SelectedLocalLicenseID.ToString();
            llShowLicensesHistory.Enabled = (_SelectedLocalLicenseID != -1);
            btnIssueLicense.Enabled = false;

            if (_SelectedLocalLicenseID == -1)
            {
                return;
            }

            //check if the selected local license is active or not
            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("License is not Active!, Choose another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueLicense.Enabled = false;
                return;
            }

            //check if the selected local license is expired or not
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("License is Expired!, Choose another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueLicense.Enabled = false;
                return;
            }

            //check if the selected local license is class 3 or not
            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassID != 3)
            {
                MessageBox.Show("Selected License Should be Class 3, Choose another license", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueLicense.Enabled = false;
                return;
            }

            //check if the person already has an active international license
            int ActiveInternationalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);

            if(ActiveInternationalLicenseID != -1)
            {
                MessageBox.Show("Person already has an active international licnese with ID = " + ActiveInternationalLicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueLicense.Enabled = false;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                return;
            }

            btnIssueLicense.Enabled = true;
        }
    }
}
