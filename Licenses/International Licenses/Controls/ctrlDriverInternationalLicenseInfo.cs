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

namespace DVLD_Project.Licenses.International_Licenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        private clsInternationalLicense _InternationalLicense;

        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        private void _HandlePersonImage()
        {
            if (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            string ImagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                {
                    pbPersonImage.ImageLocation = ImagePath;
                }
                else
                {
                    MessageBox.Show("Could not find the image file at: " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void LoadDriverInternationalLicenseInfo(int IntLicenseID)
        {
            _InternationalLicense = clsInternationalLicense.Find(IntLicenseID);

            if(_InternationalLicense == null)
            {
                MessageBox.Show("International Licnese Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsPerson Person = clsPerson.Find(_InternationalLicense.DriverInfo.PersonID);

            lblName.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " + Person.LastName;
            lblInternationalLicenseID.Text = IntLicenseID.ToString();
            lblLocalLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = Person.NationalNo;
            lblGendor.Text = (Person.Gendor == 0) ? "Male" : "Female";
            lblIssueDate.Text = clsFormat.DateToShort(_InternationalLicense.IssueDate);
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = (_InternationalLicense.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = clsFormat.DateToShort(Person.DateOfBirth);
            lblDriverID.Text = _InternationalLicense.DriverInfo.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(_InternationalLicense.ExpirationDate);

            _HandlePersonImage();

        }
    }
}
