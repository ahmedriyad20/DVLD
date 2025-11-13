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
using DVLD_Project.Licenses.Local_Licenses;

namespace DVLD_Project.Applications.Local_Driving_License
{
    public partial class ctrlDrivingLicenseApplicationBasicInfo : UserControl
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        
        private int _LicenseID = -1;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }

        public ctrlDrivingLicenseApplicationBasicInfo()
        {
            InitializeComponent();
        }

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            lblLocalDrivingLicenseApplicationID.Text = "[???]";
            lblAppliedForLicense.Text = "[???]";
            lblPassedTests.Text = "0/3";
            llShowLicenseInfo.Enabled = false;
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
        }

        public void LoadLocalDrivingLicenseApplicationInfo(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Local Driving License Application not found.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                _ResetLocalDrivingLicenseApplicationInfo();
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            
            llShowLicenseInfo.Enabled = (_LicenseID != -1);

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblAppliedForLicense.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTestsCount().ToString() + "/3";
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);
        }

        public void EnableShowLicenseInfo(bool Enabled)
        {
            llShowLicenseInfo.Enabled = Enabled;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }
    }
}
