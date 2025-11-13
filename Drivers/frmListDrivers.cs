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
using DVLD_Project.Licenses;
using DVLD_Project.People;

namespace DVLD_Project.Drivers
{
    public partial class frmListDrivers : Form
    {
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private DataTable _dtAllDrivers;

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            _dtAllDrivers = clsDriver.GetAllDrivers();
            dgvListDrivers.DataSource = _dtAllDrivers;

            lblRecordCount.Text = dgvListDrivers.Rows.Count.ToString();

            if(dgvListDrivers.Rows.Count > 0 )
            {
                dgvListDrivers.Columns[0].HeaderText = "Driver ID";
                dgvListDrivers.Columns[0].Width = 120;

                dgvListDrivers.Columns[1].HeaderText = "Person ID";
                dgvListDrivers.Columns[1].Width = 120;

                dgvListDrivers.Columns[2].HeaderText = "National No.";
                dgvListDrivers.Columns[2].Width = 120;

                dgvListDrivers.Columns[3].HeaderText = "Full Name";
                dgvListDrivers.Columns[3].Width = 300;

                dgvListDrivers.Columns[4].HeaderText = "Date";
                dgvListDrivers.Columns[4].Width = 250;

                dgvListDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvListDrivers.Columns[5].Width = 130;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text == "None")
            {
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            //now we do mapping between column names in database and column names in combo box
            string FilterColumn;
            switch(cbFilterBy.Text)
            {
                case "Driver ID":
                    {
                        FilterColumn = "DriverID";
                        break;
                    }
                case "Person ID":
                    {
                        FilterColumn = "PersonID";
                        break;
                    }
                case "National No.":
                    {
                        FilterColumn = "NationalNo";
                        break;
                    }
                case "Full Name":
                    {
                        FilterColumn = "FullName";
                        break;
                    }
                default:
                    {
                        FilterColumn = "None";
                        break;
                    }
            }

            if(txtFilterValue.Text == "" || cbFilterBy.Text == "None")
            {
                _dtAllDrivers.DefaultView.RowFilter = "";
                lblRecordCount.Text = dgvListDrivers.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "DriverID" || FilterColumn == "PersonID")
                //now we deal with integer values not string
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text);
            else
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text);

            lblRecordCount.Text = dgvListDrivers.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvListDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();

            frmListDrivers_Load(null, null);
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return; 
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory((int)dgvListDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();

            frmListDrivers_Load(null, null);
        }
    }
}
