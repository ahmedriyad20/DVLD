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

namespace DVLD_Project.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonID = -1;
        clsPerson _Person;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();

            LoadPersonInfo(_PersonID);
        }           

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";

            llEditPersonInfo.Enabled = false;

            pbPersonImage.Image = Resources.Male_512;
        }

        public void LoadPersonInfo(int PersonID)
        {
            _PersonID = PersonID;
                
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with Person ID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            llEditPersonInfo.Enabled = true;

            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblName.Text = _Person.FullName;

            if (_Person.Gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

            if (_Person.Email == "")
                lblEmail.Text = "N/A";
            else
                lblEmail.Text = _Person.Email;

            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.Phone;

            if (_Person.ImagePath != "")
                pbPersonImage.ImageLocation = _Person.ImagePath;

            if (_Person.ImagePath == "")
            {
                if (_Person.Gendor == 0)
                    pbPersonImage.Image = Resources.Male_512;
                else
                    pbPersonImage.Image = Resources.Female_512;
            }

            //Check if the country has been removed or changed in the database
            if (_Person.CountryInfo != null)
                lblCountry.Text = _Person.CountryInfo.CountryName;
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            llEditPersonInfo.Enabled = true;

            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblName.Text = _Person.FullName;

            if (_Person.Gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

            if (_Person.Email == "")
                lblEmail.Text = "N/A";
            else
                lblEmail.Text = _Person.Email;

            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.Phone;

            if (_Person.ImagePath != "")
                pbPersonImage.ImageLocation = _Person.ImagePath;

            if (_Person.ImagePath == "")
            {
                if (_Person.Gendor == 0)
                    pbPersonImage.Image = Resources.Male_512;
                else
                    pbPersonImage.Image = Resources.Female_512;
            }

            //Check if the country has been removed or changed in the database
            if (_Person.CountryInfo != null)
                lblCountry.Text = _Person.CountryInfo.CountryName;
        }
    }
}
