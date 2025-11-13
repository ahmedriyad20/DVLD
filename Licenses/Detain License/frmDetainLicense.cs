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
using DVLD_Project.Licenses.Local_Licenses;

namespace DVLD_Project.Licenses.Detain_License
{
    public partial class frmDetainLicense : Form
    {
        private int _SelectedLicenseID = -1;
        private int _DetainID = -1;

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUserID.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Please fill all required fields correctly!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to detain this License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainLicense(Convert.ToInt32(txtFineFees.Text),
                clsGlobal.CurrentUser.UserID);

            if(_DetainID == -1)
            {
                MessageBox.Show("Failed to Detain the License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnDetainLicense.Enabled = false;
            llShowLicensesInfo.Enabled = true;
            MessageBox.Show("License Detained Successfully with ID = " + _DetainID.ToString(), "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);

           
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            lblDetainID.Text = _DetainID.ToString();
            txtFineFees.Enabled = false;
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
                return;

            // Check if the selected license is detained
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is already detained!, Choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetainLicense.Enabled = false;
                return;
            }

            // Check if the selected license is active
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("License is not active, Choose another one!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetainLicense.Enabled = false;
                return;
            }

            txtFineFees.Focus();
            btnDetainLicense.Enabled = true;
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "This field is required!");
            }
            else
                errorProvider1.SetError(txtFineFees, null);
        }
    }
}
