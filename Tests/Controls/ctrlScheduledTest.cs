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
    public partial class ctrlScheduledTest : UserControl
    {
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        private int _LocalDrivingLicenseApplicationID = -1;
        private int _TestAppointmentID = -1;
        private int _TestID = -1;

        private clsTestAppointment _TestAppointment;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public clsTestType.enTestType TestTypeID
        {
            set
            {
                _TestTypeID = value;

                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    case clsTestType.enTestType.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    default:
                        gbTestType.Text = "Unknown Test Type";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                }
            }

            get { return _TestTypeID; }
        }

        public int TestID
        {
            get { return _TestID; }
        }

        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public void LoadTakeTestInfo(int TestAppointmentID, clsTestType.enTestType TestTypeID)
        {
            _TestAppointmentID = TestAppointmentID;

            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("No Test Appointment Found with ID = " + TestAppointmentID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestID = _TestAppointment.TestID;
            _TestTypeID = TestTypeID;

            _LocalDrivingLicenseApplicationID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.Find(_TestAppointment.LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Load Data
            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.GetNumberOfTrialsPerTest(_TestTypeID).ToString();
            lblDate.Text = clsFormat.DateToShort(_TestAppointment.AppointmentDate);
            lblFees.Text = _TestAppointment.PaidFees.ToString();

            if (_TestID == -1)
                lblTestID.Text = "Not Taken Yet";
            else
                lblTestID.Text = _TestID.ToString();
        }
    }
}
