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
using DVLD_Project.Properties;

namespace DVLD_Project.Tests
{
    public partial class frmListTestAppointments : Form
    {

        private int _localDrivingLicenseApplicationID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmListTestAppointments(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();

            _localDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestTypeID;
        }

        private void _LoadTestAppointments()
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_localDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplication.ApplicationStatus != clsApplication.enApplicationStatus.New)
            {
                MessageBox.Show("Error: Can't do the Test, Application is not in progress", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblTitle.Text = (_TestTypeID == clsTestType.enTestType.VisionTest) ? "Vision Test Appointments" : (_TestTypeID == clsTestType.enTestType.WrittenTest) ? "Written Test Appointments" : "Street Test Appointments";
            this.Text = lblTitle.Text;
            pbTestAppointments.Image = (_TestTypeID == clsTestType.enTestType.VisionTest) ? Resources.Vision_512 : (_TestTypeID == clsTestType.enTestType.WrittenTest) ? Resources.Written_Test_512 : Resources.driving_test_512;


            ctrlDrivingLicenseApplicationBasicInfo1.LoadLocalDrivingLicenseApplicationInfo(_localDrivingLicenseApplicationID);

            //Fill the DataGridView with Test Appointments for the selected Test Type
            dgvListAppointments.DataSource = clsTestAppointment.GetAllTestAppointmentsForEachTestType(
                _localDrivingLicenseApplicationID, _TestTypeID);

            lblRecordCount2.Text = dgvListAppointments.Rows.Count.ToString();
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestAppointments();

            if(dgvListAppointments.Rows.Count > 0)
            {
                dgvListAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvListAppointments.Columns[0].Width = 130;

                dgvListAppointments.Columns[1].HeaderText = "Apponintment Date";
                dgvListAppointments.Columns[1].Width = 170;

                dgvListAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvListAppointments.Columns[2].Width = 110;

                dgvListAppointments.Columns[3].HeaderText = "Is Locked";
                dgvListAppointments.Columns[3].Width = 120;

            }
        }

        private void btnAddTestAppointment_Click(object sender, EventArgs e)
        {
            if(_LocalDrivingLicenseApplication.IsThereAnActiveTestAppointment(_TestTypeID))
            {
                MessageBox.Show("Person already has an active appointment for this test, You cannot add new Appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = _LocalDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            if (LastTest == null)
            {
                frmScheduleTest frm = new frmScheduleTest(_localDrivingLicenseApplicationID, _TestTypeID);
                frm.ShowDialog();

                frmListTestAppointments_Load(null, null);
                return;
            }

            if(LastTest.TestResult == true)
            {
                MessageBox.Show("Person already passed this test, You cannot add new Test Appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if (_LocalDrivingLicenseApplication.DoesPassTestType(_TestTypeID))
            //{
            //    MessageBox.Show("Person already passed this test, You cannot add new Test Appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            frmScheduleTest frm2 = new frmScheduleTest
                (LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestTypeID);
            frm2.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvListAppointments.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(_localDrivingLicenseApplicationID, _TestTypeID, TestAppointmentID);
            frm.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvListAppointments.CurrentRow.Cells[0].Value;

           
            frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestTypeID);
            frm.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
