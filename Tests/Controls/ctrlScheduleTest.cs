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
using DVLD_Project.Properties;

namespace DVLD_Project.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode;

        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 }
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointment;
        

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        public clsTestType.enTestType TestTypeID
        {
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            lblTitle.Text = "Vision Test";
                            gbTestType.Text = "Vision Test";
                            pbTestImage.Image = Resources.Vision_512;
                            break;
                        }
                    case clsTestType.enTestType.WrittenTest:
                        {
                            lblTitle.Text = "Written Test";
                            gbTestType.Text = "Written Test";
                            pbTestImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            lblTitle.Text = "Street Test";
                            gbTestType.Text = "Street Test";
                            pbTestImage.Image = Resources.driving_test_512;
                            break;
                        }
                }
            }

            get { return _TestTypeID; }
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }


        public void LoadTestAppointmentInfo(int TestAppointmentID, int LocalDrivingLicenseApplicationID)
        {
            if (TestAppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _TestAppointmentID = TestAppointmentID;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Local Driving License Application Found with ID: " + LocalDrivingLicenseApplicationID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            //decide if the createion mode is retake test or not based if the person attended this test before
            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            if(_CreationMode == enCreationMode.FirstTimeSchedule)
            {
                lblTitle.Text = "Schedule Test";
                gbRetakeTestInfo.Enabled = false;
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                lblTitle.Text = "Schedule Retake Test  ";
                gbRetakeTestInfo.Enabled = true;
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees.ToString();
                lblRetakeTestAppID.Text = "N/A";
            }

            lblLocalDrivingLicenseAppID.Text = LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.GetNumberOfTrialsPerTest(_TestTypeID).ToString();

            if(_Mode == enMode.AddNew)
            {
                lblUserMessage.Enabled = false;
                lblFees.Text = clsTestType.Find(_TestTypeID).TestTypeFees.ToString();
                dtpTestDate.MinDate = DateTime.Now;

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if(!_LoadTestAppointmentData())
                {
                    return;
                }
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;
            
            if(!_HandleAppointmentLockedConstraint())
                return;

            if(!_HandlePrviousTestConstraint())
                return;
        }

        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if(_TestAppointment == null)
            {
                MessageBox.Show("No Test Appointment Found with ID: " + _TestAppointmentID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.PaidFees.ToString();

            if(_TestAppointment.AppointmentDate < DateTime.Today)
            {
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;
            }
            else
            {
                dtpTestDate.MinDate = DateTime.Now;
            }

            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if(_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeTestAppID.Text = "N/A";
                lblRetakeAppFees.Text = "0";
            }
            else
            {
                lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                lblRetakeAppFees.Text = _TestAppointment.RetakeTestApplicationInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test  ";
            }

            return true;
        }

        private bool _HandleActiveTestAppointmentConstraint()
        {
            if(_Mode == enMode.AddNew && _LocalDrivingLicenseApplication.IsThereAnActiveTestAppointment(_TestTypeID))
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person Already has an active appointment for this test";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;

            return true;
        }

        private bool _HandleAppointmentLockedConstraint()
        {
            if(_TestAppointment.isLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for this test, appointment is locked";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;

            return true;
        }

        private bool _HandlePrviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannot apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        //in this case no required prvious test to pass.
                        lblUserMessage.Visible = false;
                        return true;
                    }
                case clsTestType.enTestType.WrittenTest:
                    {
                        if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                        {
                            lblUserMessage.Visible = true;
                            lblUserMessage.Text = "Cannot schedule, Vision Test should be passed first";
                            dtpTestDate.Enabled = false;
                            btnSave.Enabled = false;
                            return false;
                        }
                        else
                        {
                            lblUserMessage.Visible = false;
                            dtpTestDate.Enabled = true;
                            btnSave.Enabled = true;
                            return true;
                        }
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                        {
                            lblUserMessage.Visible = true;
                            lblUserMessage.Text = "Cannot schedule, Written Test should be passed first";
                            dtpTestDate.Enabled = false;
                            btnSave.Enabled = false;
                            return false;
                        }
                        else
                        {
                            lblUserMessage.Visible = false;
                            dtpTestDate.Enabled = true;
                            btnSave.Enabled = true;
                            return true;
                        }
                    }


            }

            return true;
        }

        private bool _HandleRetakeTestApplication()
        {
            //this will decide to create a seperate application for retake test or not.
            // and will create it if needed , then it will linked to the appoinment.

            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                //incase the mode is add new and creation mode is retake test we should create a seperate application for it.
                //then we linke it with the appointment.

                //First Create Applicaiton 
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
                    return true;
                }
                else
                {
                    MessageBox.Show("Failed To create the Retake Test Application", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _TestAppointment.RetakeTestApplicationID = -1;
                    return false;
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeTestApplication())
                return;


            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = clsTestType.Find(_TestTypeID).TestTypeFees;
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _TestAppointment.isLocked = false;

            if(_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to Save the Data", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
