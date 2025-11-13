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

namespace DVLD_Project.Users
{
    public partial class frmListUsers : Form
    {
        public frmListUsers()
        {
            InitializeComponent();
        }

        private static DataTable _dtAllUsers;

        private void _RefreshAllUsers()
        {
            _dtAllUsers = clsUser.GetAllUsers();

            dgvListUsers.DataSource = _dtAllUsers;

            lblRecordCount.Text = dgvListUsers.Rows.Count.ToString();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            _RefreshAllUsers();

            if(dgvListUsers.Rows.Count > 0)
            {
                dgvListUsers.Columns[0].HeaderText = "User ID";

                dgvListUsers.Columns[1].HeaderText = "Person ID";
                dgvListUsers.Columns[1].Width = 110;

                dgvListUsers.Columns[2].HeaderText = "Full Name";
                dgvListUsers.Columns["FullName"].Width = 255;

                dgvListUsers.Columns["UserName"].Width = 110;

                dgvListUsers.Columns[4].HeaderText = "Is Active";
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshAllUsers();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();

            _RefreshAllUsers();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();

            _RefreshAllUsers();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshAllUsers();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this User?", "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK )
            {
                clsUser.DeleteUser((int)dgvListUsers.CurrentRow.Cells[0].Value);
                MessageBox.Show("User Deleted Successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            _RefreshAllUsers();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "User ID" || cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text == "None")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;
                cbIsActive.Visible = false;
            }

            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
            }

            if(txtFilterValue.Visible)
            {
                txtFilterValue.Focus();
                txtFilterValue.Text = "";
            }

            //dgvListUsers.DataSource = clsUser.GetAllUsers();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            //Map Selected Filter to real Column name 
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "User ID":
                    {
                        FilterColumn = "UserID";
                        break;
                    }
                case "Username":
                    {
                        FilterColumn = "UserName";
                        break;
                    }
                case "Person ID":
                    {
                        FilterColumn = "PersonID";
                        break;
                    }
                case "Full Name":
                    {
                        FilterColumn = "FullName";
                        break;
                    }
                case "Is Active":
                    {
                        FilterColumn = "IsActive";
                        break;
                    }
                default:
                    {
                        FilterColumn = "None";
                        break;
                    }

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordCount.Text = dgvListUsers.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "UserID" || FilterColumn == "PersonID")
                //in this case we deal with integer not string
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text);
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text);

            lblRecordCount.Text = dgvListUsers.Rows.Count.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsActive.Text == "All")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
            }
            else if (cbIsActive.Text == "Yes")
            {
                _dtAllUsers.DefaultView.RowFilter = "IsActive = 1";
            }
            else
            {
                _dtAllUsers.DefaultView.RowFilter = "IsActive = 0";
            }

            lblRecordCount.Text = dgvListUsers.Rows.Count.ToString();
        }
    }
}
