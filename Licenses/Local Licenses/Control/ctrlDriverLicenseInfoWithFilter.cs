using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;

namespace DVLD_Project.Licenses.Local_Licenses.Control
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        private int _LicenseID = -1;

        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(LicenseID); // Raise the event with the parameter
            }
        }

        public void LoadLicenseInfo(int LicenseID)
        {
            txtLicenseID.Focus();
            txtLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadDriverLicenseInfo(LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;
            if (OnLicenseSelected != null && FilterEnabled)
                //Raise the event with the LicenseID
                LicenseSelected(_LicenseID);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("License ID field is not valid!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLicenseID.Focus();
                return;
            }

            _LicenseID = Convert.ToInt32(txtLicenseID.Text.Trim());
            LoadLicenseInfo(_LicenseID);
        }

        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if(e.KeyChar == (char)13)
                btnFind.PerformClick();
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtLicenseID.Text))
            {
                e.Cancel = true; // Cancel the event if the text is empty
                errorProvider1.SetError(txtLicenseID, "This filed cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtLicenseID, null);
            }
        }
    }
}
