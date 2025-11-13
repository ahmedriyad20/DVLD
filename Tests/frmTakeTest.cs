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

namespace DVLD_Project.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _TestAppointmentID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        clsTestAppointment _TestAppointment;
        clsTest _Test;

        public frmTakeTest(int TestAppointmentID,  clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();

            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestTypeID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            _Test = clsTest.FindByTestAppointmentID(_TestAppointmentID);


            if(_TestAppointment.isLocked)
            {
                rbPass.Enabled = false;
                rbFail.Enabled = false;

                lblUserMessage.Visible = true;

                if (_Test.TestResult == true)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
            }


            ctrlScheduledTest1.TestTypeID = _TestTypeID;
            ctrlScheduledTest1.LoadTakeTestInfo(_TestAppointmentID, _TestTypeID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            clsTest Test = clsTest.FindByTestAppointmentID(_TestAppointmentID);

            if (Test == null)
                Test = new clsTest();

            Test.TestAppointmentID = _TestAppointmentID;
            Test.TestResult = (rbPass.Checked);
            Test.Notes = txtNotes.Text.Trim();
            Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (Test.Save())
            {
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
                MessageBox.Show("Failed to Save the Data!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
