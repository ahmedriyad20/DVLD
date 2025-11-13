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

namespace DVLD_Project.Applications.Applications_Types
{
    public partial class frmUpdateApplicationType : Form
    {
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;

        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();

            _ApplicationTypeID = ApplicationTypeID;
            if (_ApplicationTypeID == -1)
                Mode = enMode.AddNew;
            else
                Mode = enMode.Update;
        }

        private void _LoadApplicationTypeInfo()
        {
            if (Mode == enMode.AddNew)
                return;

            //if we reached here means we are in update mode
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            //Check if any other user using the system now has deleted the record
            if (_ApplicationType == null)
            {
                MessageBox.Show("No Application Type found with this ID", "Error, No Application Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
            txtApplicationTypeTitle.Text = _ApplicationType.ApplicationTypeTitle;
            txtApplicationFees.Text = _ApplicationType.ApplicationFees.ToString();


        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _LoadApplicationTypeInfo();

            txtApplicationTypeTitle.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, Please check them and try again!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_ApplicationType == null)
                _ApplicationType = new clsApplicationType();

            _ApplicationType.ApplicationTypeTitle = txtApplicationTypeTitle.Text.Trim();
            _ApplicationType.ApplicationFees = Convert.ToInt32(txtApplicationFees.Text.Trim());

            if(_ApplicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Failed to Save the Data!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtApplicationTypeTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeTitle, "Application Type Title cannot be blank!");
            }
            else
                errorProvider1.SetError(txtApplicationTypeTitle, null);
        }

        private void txtApplicationFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtApplicationFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationFees, "Application Fees cannot be blank!");
            }
            else
                errorProvider1.SetError(txtApplicationFees, null);
        }

        private void txtApplicationFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
