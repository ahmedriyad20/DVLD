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

namespace DVLD_Project.Tests.Test_Types
{
    public partial class frmEditTestType : Form
    { 
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;

        private int _TestTypeID = -1;
        private clsTestType _TestType;

        public frmEditTestType(int TestTypeID)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID;
            if (_TestTypeID != -1)
                Mode = enMode.Update;
            else
                Mode = enMode.AddNew;
        }

        private void _LoadTestTypeInfo()
        {
            if (Mode == enMode.AddNew)
                return;

            //if we reached here means we are in update mode
            _TestType = clsTestType.Find((clsTestType.enTestType)_TestTypeID);

            //Check if any other user using the system now has deleted the record
            if (_TestType == null)
            {
                MessageBox.Show("No Test Type found with this ID", "Error, No Test Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTestTypeID.Text = _TestType.TestTypeID.ToString();
            txtTestTypeTitle.Text = _TestType.TestTypeTitle;
            txtTestTypeDescription.Text = _TestType.TestTypeDescription;
            txtTestFees.Text = _TestType.TestTypeFees.ToString();


        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _LoadTestTypeInfo();

            txtTestTypeTitle.Focus();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, Please check them and try again!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_TestType == null)
                _TestType = new clsTestType();

            _TestType.TestTypeTitle = txtTestTypeTitle.Text.Trim();
            _TestType.TestTypeDescription = txtTestTypeDescription.Text.Trim();
            _TestType.TestTypeFees = Convert.ToInt32(txtTestFees.Text);

            if (_TestType.Save())
            {
                MessageBox.Show("Data Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Failed to Save the Data!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtTestTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestTypeTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeTitle, "Test Type Title cannot be blank!");
            }
            else
                errorProvider1.SetError(txtTestTypeTitle, null);
        }

        private void txtTestDescription_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtTestTypeDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTypeDescription, "Test Type Description cannot be blank!");
            }
            else
                errorProvider1.SetError(txtTestTypeDescription, null);
        }

        private void txtTestFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTestFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestFees, "Test Fees cannot be blank!");
            }
            else
                errorProvider1.SetError(txtTestFees, null);
        }

        private void txtTestFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        
    }
}
