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
using System.IO;
using DVLD_Project.Classes;

namespace DVLD_Project.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public enum enMode { AddNew = 0, Update = 1}
        public enum enGender { Male = 0, Female = 1}
        private enMode _Mode;

        private int _PersonID = -1;
        private clsPerson _Person;

        public frmAddUpdatePerson()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;

        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            _Mode = enMode.Update;
        }

        //Declare a delegate
        public delegate void PersonIDSendBackHandler(int PersonID);

        //Declare an event using the delegate
        public event PersonIDSendBackHandler PersonIDSendBack;

        private void _FillCountriesInComboBox()
        {
            DataTable dataTable = clsCountry.GetAllCountries();

            foreach(DataRow row in dataTable.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }

        }

        private void _ResetDefaultValues()
        {
            _FillCountriesInComboBox();
            
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
                
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            //hide/show the remove link label incase there is an image
            llRemove.Visible = (pbPersonImage.ImageLocation != null);

            //Assign the combo box to default country = Egypt
            cbCountry.SelectedIndex = cbCountry.FindString("Egypt");

            //Set the date time picker max value to 18 years before
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";

            txtFirstName.Focus();
        }

        private bool _HandlePersonImage()
        {
            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.

            if(_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                //This means the image changed in the picture box

                if(_Person.ImagePath != "")
                {
                    //first we delete the old image from the folder in case there is any.
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if(pbPersonImage.ImageLocation != null)
                {
                    string sourceImageFile = pbPersonImage.ImageLocation.ToString();
                    if(clsUtil.CopyImageToProjectImagesFolder(ref sourceImageFile))
                    {
                        pbPersonImage.ImageLocation = sourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to Copy the person image to project folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private void _LoadPersonInfo()
        {
            _Person = clsPerson.Find(_PersonID);

            //Check if any other user using the system now has deleted the record
            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            //the following code will not be executed if the person was not found
            lblPersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Text = _Person.DateOfBirth.ToString();
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;

            if (_Person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            if(_Person.CountryInfo != null)
                cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);

            //set the image in case there is an image path
            if (_Person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
            }

            //set the default image in case there is no image path
            if (_Person.ImagePath == "")
            {
                if (rbMale.Checked)
                    pbPersonImage.Image = Resources.Male_512;
                else
                    pbPersonImage.Image = Resources.Female_512;
            }

            //hide/show the remove link in case there is an image or not
            llRemove.Visible = (_Person.ImagePath != "");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Error!, One of the Fields not Valid, Please Validate All the required Fileds.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
                return;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked)
                _Person.Gendor = (short)enGender.Male;
            else
                _Person.Gendor = (short)enGender.Female;

            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();

            _Person.NationalityCountryID = clsCountry.Find(cbCountry.Text).CountryID;
            _Person.Address = txtAddress.Text.Trim();
            _Person.ImagePath = pbPersonImage.ImageLocation;

            if (_Person.Save())
            {
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblPersonID.Text = _Person.PersonID.ToString();

                lblTitle.Text = "Update Person";
                _Mode = enMode.Update;

                //Trigger the event and Send the PersonID back to the caller
                PersonIDSendBack?.Invoke(_Person.PersonID);
            }
            else
            {
                MessageBox.Show("Failed To Save The Data!", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {

            _ResetDefaultValues();

            if(_Mode == enMode.Update)
                _LoadPersonInfo();
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.ImageLocation = openFileDialog1.FileName;
                llRemove.Visible = true;
            }
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemove.Visible = false;
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This Field is required!");
                return;
            }

            clsPerson Person = clsPerson.Find(txtNationalNo.Text.Trim());

            if (Person != null && txtNationalNo.Text.ToLower() == Person.NationalNo.ToLower() && _PersonID != Person.PersonID)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is Used for Another Person!");
            }
            else
                errorProvider1.SetError(txtNationalNo, null);
        }

        private void _ValidateEmptyTextBox(TextBox textBox, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            //TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(textBox, null);
            }
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            _ValidateEmptyTextBox(txtFirstName, e);
        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            _ValidateEmptyTextBox(txtSecondName, e);
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            _ValidateEmptyTextBox(txtLastName, e);
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            _ValidateEmptyTextBox(txtPhone, e);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text == "")
                return;

            string Email = txtEmail.Text;

            if(Email.ToLower().EndsWith("@gmail.com") && Email.IndexOf("@") > 0 )
            {
                
                errorProvider1.SetError(txtEmail, null);
                
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invaild Email Address Format!");
            }
                
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            _ValidateEmptyTextBox(txtAddress, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {


            this.Close();
        }

        
    }
}
