using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;
using DVLD_Project.Classes;
using DVLD_Project.Properties;

namespace DVLD_Project.Licenses.Local_Licenses.Control
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LicenseID = -1;
        private clsLicense _License;

        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return _License; }
        }

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        private void _LoadPersonImage()
        {
            if (_License.DriverInfo.PersonInfo.Gendor == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;
            if(ImagePath != "")
            {
                if(File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find the image file at: " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDriverLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = clsLicense.Find(LicenseID);

            if (_License == null)
            {
                MessageBox.Show("No License found with ID = " + _LicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            //Load Driver License Info
            lblClass.Text = _License.LicenseClassInfo.ClassName;
            lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = (_License.DriverInfo.PersonInfo.Gendor == 0) ? "Male" : "Female";
            lblIssueDate.Text = clsFormat.DateToShort(_License.IssueDate);
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = (_License.Notes == "")? "No Notes" : _License.Notes;
            lblIsActive.Text = (_License.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(_License.ExpirationDate);
            lblIsDetained.Text = (_License.IsDetained) ? "Yes" : "No";
            _LoadPersonImage();
        }
    }
}
