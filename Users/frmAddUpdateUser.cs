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

namespace DVLD_Project.Users
{
    public partial class frmAddUpdateUser : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;
        private int _UserID = -1;
        private clsUser _User;

        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _Mode = enMode.Update;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(ctrlPersonCardWithFilter1.SelectedPersonInfo == null)
            {
                MessageBox.Show("Please Select a Person First.", "No Person Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            if(_Mode == enMode.AddNew)
            {
                if (clsUser.IsUserExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person Already has a User, Choose Another One.", "User Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = false;
                    ctrlPersonCardWithFilter1.FilterFocus();
                    return;
                }
                else
                {
                    tbLoginInfo.Enabled = true;
                    tcUserInfo.SelectTab(1);
                    btnSave.Enabled = true;
                }
            }

            if(_Mode == enMode.Update)
            {
                tbLoginInfo.Enabled = true;
                tcUserInfo.SelectTab(1);
                btnSave.Enabled = true;

            }


        }

        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.AddNew)
            {
                tbLoginInfo.Enabled = false;
                btnSave.Enabled = false;
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                ctrlPersonCardWithFilter1.FilterFocus();
                _User = new clsUser();
            }
            else
            {
                tbLoginInfo.Enabled = true;
                btnSave.Enabled = true;
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                ctrlPersonCardWithFilter1.EnableFilter(false);
            }

            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }

        private void _LoadUserInfo()
        {
            _User = clsUser.Find(_UserID);

            //Check if any other user using the system now has deleted the record
            if (_User == null)
            {
                MessageBox.Show("No User Found with this ID!", "Error, No User Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //the following code will not be executed if the user was not found
            //Load Person Info
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);

            //Load User Info
            lblUserID.Text = _User.UserID.ToString();
            txtUsername.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;

        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadUserInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Validate Username and Password
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not Valid!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //if User not exist means we are in Add New Mode, So we need to create a new User
            if (_User == null)
                _User = new clsUser();

            //Check if the person changed after clicking Next button
            if(ctrlPersonCardWithFilter1.SelectedPersonInfo == null)
            {
                MessageBox.Show("Please Select a Person First.", "No Person Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Check if person already related with another user in Add New Mode after clicking Next button
            if (_Mode == enMode.AddNew)
            {
                if (clsUser.IsUserExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Person Already related with Another User!", "Choose Another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUsername.Text;
            _User.Password = clsUtil.ComputeHash(txtPassword.Text.Trim()); //Hashing the password then save it in DB
            _User.IsActive = chkIsActive.Checked;

            if(_User.Save())
            {
                MessageBox.Show("Data Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblUserID.Text = _User.UserID.ToString();
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                ctrlPersonCardWithFilter1.EnableFilter(false);
                _Mode = enMode.Update;
            }
            else
            {
                MessageBox.Show("Faild to Add the User!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    

        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUsername, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUsername, null);
            };

            clsUser User = clsUser.FindByUserName(txtUsername.Text.Trim());

            if (_Mode == enMode.AddNew)
            {
                if (User == null)
                    return;

                if (txtUsername.Text.Trim() == User.UserName)
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUsername, "Username already exists");
                }
                else
                {
                    errorProvider1.SetError(txtUsername, null);
                };
                
            }
            else
            {
                if (User == null)
                    return;

                if (txtUsername.Text.Trim() == User.UserName && lblUserID.Text != User.UserID.ToString())
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUsername, "Updated Uername is Exist in Users!");
                }
                else
                {
                    errorProvider1.SetError(txtUsername, null);
                };
                
            }
            
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation doesn't match Password!");
            }
            else
                errorProvider1.SetError(txtConfirmPassword, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
