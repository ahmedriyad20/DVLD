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
using Microsoft.Win32;

namespace DVLD_Project.Login
{
    public partial class frmLogin : Form
    {
        private int _Count = 0;
        private clsUser User;

        public frmLogin()
        {
            InitializeComponent();
        }

        public delegate void LoginSuccessEventHandler(object sender, clsUser User);

        public event LoginSuccessEventHandler OnLoginSuccess;

        private void _SaveCredentials()
        {
            //if(chkRemeberMe.Checked)
            //{
            //    File.WriteAllText("credentials.txt", $"{txtUsername.Text.Trim()}\n{txtPassword.Text.Trim()}");
            //}
            //else
            //{
            //    if (File.Exists("credentials.txt"))
            //    {
            //        File.Delete("credentials.txt");
            //    }

            //    txtUsername.Text = "";
            //    txtPassword.Text = "";
            //}

            //We will save the credentials to the registry instead of a file

            string keyPath = @"HKEY_CURRENT_USER\Software\DVLDcredentials";

            

            if(chkRemeberMe.Checked)
            {
                string Username = txtUsername.Text;
                string Password = txtPassword.Text;

                try
                {
                    //Save the Username
                    Registry.SetValue(keyPath, "Username", Username, RegistryValueKind.String);

                    //Save the Password
                    Registry.SetValue(keyPath, "Password", Password, RegistryValueKind.String);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"error occured: {ex.Message}", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    string keyPathForDelete = @"Software\DVLDcredentials";

                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                    {
                        
                        using (RegistryKey key = baseKey.OpenSubKey(keyPathForDelete, true))
                        {
                            if (key != null)
                            {
                                key.DeleteValue("Username", false);
                                key.DeleteValue("Password", false);

                                
                            }
                            else
                            {
                                MessageBox.Show($"Registry key '{keyPath}' not found", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("UnauthorizedAccessException: Run the program with administrative privileges.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error occured: {ex.Message}", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                txtUsername.Text = "";
                txtPassword.Text = "";
            }
        }

        private void _LoadCredentials()
        {
            //if(File.Exists("credentials.txt"))
            //{
            //    string[] Lines = File.ReadAllLines("credentials.txt");
            //    if(Lines.Length >= 2)
            //    {
            //        txtUsername.Text = Lines[0];
            //        txtPassword.Text = Lines[1];
            //        chkRemeberMe.Checked = true;
            //    }
            //}

            string keyPath = @"HKEY_CURRENT_USER\Software\DVLDcredentials";
            
            try
            {
                string Username = Registry.GetValue(keyPath, "Username", null) as string;
                string Password =  Registry.GetValue(keyPath, "Password", null) as string;

                if(Username != null && Password != null)
                {
                    txtUsername.Text = Username;
                    txtPassword.Text = Password;
                    chkRemeberMe.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error occured: {ex.Message}", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _HandleLoggingTimes()
        {
            if(_Count == 3)
            {
                MessageBox.Show("Account is locked! Due to 3 failed trils!", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);

                clsGlobal.LogExecption("Account is locked! Due to 3 failed trils!");
            }
        }

        private bool _ValidateUsernameAndPassword()
        {
            User = clsUser.FindByUserName(txtUsername.Text);

            if(User != null)
            {
                string HashedPassword = clsUtil.ComputeHash(txtPassword.Text);

                if(User.Password == HashedPassword)
                {
                    if (User.IsActive)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error, User is not Active!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                }
                else
                {
                    MessageBox.Show("Invalid Username/Password!", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _Count++;
                    _HandleLoggingTimes();
                    return false;
                }
                
            }
            else
            {
                MessageBox.Show("Invalid Username/Password!", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _Count++;
                _HandleLoggingTimes();
                return false;
            }
        }

        private void _SaveLoginToUserRegisterHistory(object sender, clsUser User)
        {
            string logFileName = "UserLoginHistory.txt";

            string LogUserInfo = $"Username: {User.UserName}, PersonID: {User.PersonID}, Login Time: {DateTime.Now}\n";

            try
            {
                File.AppendAllText(logFileName, LogUserInfo);
            }
            catch (Exception ex) { }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_ValidateUsernameAndPassword())
            {
                _SaveCredentials();
                clsGlobal.CurrentUser = User;
                
                OnLoginSuccess += _SaveLoginToUserRegisterHistory;
                OnLoginSuccess?.Invoke(this, User);

                frmMain frm = new frmMain(this);
                this.Hide();
                frm.ShowDialog();
            }
            else
                return;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            _LoadCredentials();
        }
    }
}
