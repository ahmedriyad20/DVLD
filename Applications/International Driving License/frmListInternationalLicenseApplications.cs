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
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.People;

namespace DVLD_Project.Applications.International_Driving_License
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private DataTable _dtAllInternationalLicenses;

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            _dtAllInternationalLicenses = clsInternationalLicense.GetAllInternationalLicenses();
            dgvInternationalLicenseApplication.DataSource = _dtAllInternationalLicenses;

            lblRecordCount.Text = dgvInternationalLicenseApplication.Rows.Count.ToString();

            if (dgvInternationalLicenseApplication.Rows.Count > 0)
            {
                dgvInternationalLicenseApplication.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenseApplication.Columns[0].Width = 150;

                dgvInternationalLicenseApplication.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenseApplication.Columns[1].Width = 160;

                dgvInternationalLicenseApplication.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenseApplication.Columns[2].Width = 120;

                dgvInternationalLicenseApplication.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenseApplication.Columns[3].Width = 150;

                dgvInternationalLicenseApplication.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenseApplication.Columns[4].Width = 200;

                dgvInternationalLicenseApplication.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenseApplication.Columns[5].Width = 200;

                dgvInternationalLicenseApplication.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenseApplication.Columns[6].Width = 120;

                dgvInternationalLicenseApplication.Columns[7].Visible = false;
                
            }
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsDriver Driver = clsDriver.Find((int)dgvInternationalLicenseApplication.CurrentRow.Cells[2].Value);

            if(Driver != null )
            {
                frmShowPersonInfo frm = new frmShowPersonInfo(Driver.PersonID);
                frm.ShowDialog();
            }
            
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo
                ((int)dgvInternationalLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsDriver Driver = clsDriver.Find((int)dgvInternationalLicenseApplication.CurrentRow.Cells[2].Value);

            if(Driver == null )
                return;

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(Driver.PersonID);
            frm.ShowDialog();
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
            else if(cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilterValue.Visible=true;
                cbIsActive.Visible=false;
            }

            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsActive.Text == "All")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
            }
            else if (cbIsActive.Text == "Yes")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "IsActive = true";
                lblRecordCount.Text = dgvInternationalLicenseApplication.Rows.Count.ToString();
            }
            else
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "IsActive = false";
                lblRecordCount.Text = dgvInternationalLicenseApplication.Rows.Count.ToString();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            //here we do mapping between the filter type and the column name in the DataTable
            string FilterCloumn = "";
            switch (cbFilterBy.Text)
            {
                case "International License ID":
                    {
                        FilterCloumn = "InternationalLicenseID";
                        break;
                    }
                case "Application ID":
                    {
                        FilterCloumn = "ApplicationID";
                        break;
                    }
                case "Driver ID":
                    {
                        FilterCloumn = "DriverID";

                        break;
                    }
                case "Local License ID":
                    {
                        FilterCloumn = "IssuedUsingLocalLicenseID";

                        break;
                    }
                default:
                    {
                        FilterCloumn = "None";
                        break;
                    }
            }

            if (txtFilterValue.Text == "" || cbFilterBy.Text == "None")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
                lblRecordCount.Text = dgvInternationalLicenseApplication.Rows.Count.ToString();
                return;
            }

             _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterCloumn, txtFilterValue.Text);

            lblRecordCount.Text = dgvInternationalLicenseApplication.Rows.Count.ToString(); 
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

       
    }
}
