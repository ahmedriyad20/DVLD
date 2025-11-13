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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Project.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _Mode = enMode.Update;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dataTable = clsLicenseClass.GetAllLicenseClasses();

            foreach(DataRow row in dataTable.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }

        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                btnSave.Enabled = false;
                tbApplicationInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();

                cbLicenseClass.SelectedIndex = 2; //Class 3 Ordinary driving License
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).ApplicationFees.ToString();
                lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                btnSave.Enabled = true;
                tbApplicationInfo.Enabled = true;
                ctrlPersonCardWithFilter1.EnableFilter(false);
            }
        }

        private void _LoadLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationID);

            //Check if any other user using the system now has deleted the record
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            //Fill Person Info in Person Card Control
            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);

            //Fill LDL Application Info
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = _LocalDrivingLicenseApplication.LicenseClassID - 1; //License Class IDs start from 1, but ComboBox index starts from 0
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.Find(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if(_Mode == enMode.Update)
                _LoadLocalDrivingLicenseApplicationInfo();
        }

        private bool _DoesPersonAgeMatchTheLicenseClass()
        {
            clsLicenseClass LicenseClass = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID);

            DateTime MinAllowdAge = DateTime.Now.AddYears(- LicenseClass.MinimumAllowedAge);

            //Update person Info if changed
            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);

            if (ctrlPersonCardWithFilter1.SelectedPersonInfo.DateOfBirth <= MinAllowdAge)
            {
                return true;
            }
            else
                return false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.AddNew)
            {
                if(ctrlPersonCardWithFilter1.SelectedPersonInfo == null)
                {
                    MessageBox.Show("Please select a person first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = false;
                    ctrlPersonCardWithFilter1.FilterFocus();
                    return;
                }
            }

            tbApplicationInfo.Enabled = true;
            btnSave.Enabled = true;
            tcApplicationInfo.SelectedIndex = 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.SelectedPersonInfo == null)
            {
                MessageBox.Show("Please select a person first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsLocalDrivingLicenseApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID,
                clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if(ActiveApplicationID != -1)
            {
                MessageBox.Show($@"Choose another License Class, The Selected Person Already has an Active application with ID = " + ActiveApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {
                MessageBox.Show($@"Choose another License Class, The Selected Person Already has a license for the selected class ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            if (_Mode == enMode.AddNew)
                _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.NewDrivingLicense;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;

            //Check if the person age matches the Minimum Allowed license class age
            if(!_DoesPersonAgeMatchTheLicenseClass())
            {
                MessageBox.Show("Person Age does not match the License Class Minimum Allowed Age", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Save the Local Driving License Application data
            if (_LocalDrivingLicenseApplication.Save())
            {
                //Change the mode to Update after saving the data
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
               
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            else
            {
                MessageBox.Show("Failed To Save The Data!", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = (int)obj;
        }
    }
}
