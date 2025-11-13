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
using DVLD_Project.Licenses.Detain_License;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.People;

namespace DVLD_Project.Applications.Release_Detained_License
{
    public partial class frmListDetainedLicenses : Form
    {
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private DataTable _dtAllDetainedLicenses;

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _dtAllDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvListDetainedLicenses.DataSource = _dtAllDetainedLicenses;

            cbFilterBy.SelectedIndex = 0;
            cbIsReleased.SelectedIndex = 0;

            lblRecordCount.Text = dgvListDetainedLicenses.Rows.Count.ToString();

            if (dgvListDetainedLicenses.Rows.Count > 0 )
            {
                dgvListDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvListDetainedLicenses.Columns[0].Width = 80;

                dgvListDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvListDetainedLicenses.Columns[1].Width = 80;

                dgvListDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvListDetainedLicenses.Columns[2].Width = 200;

                dgvListDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvListDetainedLicenses.Columns[3].Width = 100;

                dgvListDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvListDetainedLicenses.Columns[4].Width = 120;

                dgvListDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvListDetainedLicenses.Columns[5].Width = 200;

                dgvListDetainedLicenses.Columns[6].HeaderText = "N.No";
                dgvListDetainedLicenses.Columns[6].Width = 80;

                dgvListDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvListDetainedLicenses.Columns[7].Width = 250;

                dgvListDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
                dgvListDetainedLicenses.Columns[8].Width = 140;
            }
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text == "None")
            {
                txtFilterValue.Visible = false;
            }
            else if (cbFilterBy.Text == "Is Released")
            {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
            }
            else
            {
                txtFilterValue.Visible = true;
                cbIsReleased.Visible = false;
            }

            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbIsReleased.Text == "All")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
            }
            else if ( cbIsReleased.Text == "Yes")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "IsReleased = true";
                lblRecordCount.Text = dgvListDetainedLicenses.Rows.Count.ToString();
            }
            else
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "IsReleased = false";
                lblRecordCount.Text = dgvListDetainedLicenses.Rows.Count.ToString();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            //here we do mapping between the filter type and the column name in the DataTable
            string FilterCloumn = "";
            switch(cbFilterBy.Text)
            {
                case "Detain ID":
                    {
                        FilterCloumn = "DetainID";
                        break;
                    }
                case "National No.":
                    {
                        FilterCloumn = "NationalNo";
                        break;
                    }
                case "Full Name":
                    {
                        FilterCloumn = "FullName";
                        break;
                    }
                case "Release Application ID":
                    {
                        FilterCloumn = "ReleaseApplicationID";
                        break;
                    }
                default:
                    {
                        FilterCloumn = "None";
                        break;
                    }
            }

            if(txtFilterValue.Text == "" || cbFilterBy.Text == "None")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
                lblRecordCount.Text = dgvListDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (FilterCloumn == "DetainID" || FilterCloumn == "ReleaseApplicationID")
                //means we deal with integer not string
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterCloumn, txtFilterValue.Text);
            else
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", FilterCloumn, txtFilterValue.Text);
            
            lblRecordCount.Text = dgvListDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Detain ID" || cbFilterBy.Text == "Release Application ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsPerson Person = clsPerson.Find((string)dgvListDetainedLicenses.CurrentRow.Cells[6].Value);

            frmShowPersonInfo frm = new frmShowPersonInfo(Person.PersonID);
            frm .ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            frm .ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsPerson Person = clsPerson.Find((string)dgvListDetainedLicenses.CurrentRow.Cells[6].Value);

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(Person.PersonID);
            frm .ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            clsLicense License = clsLicense.Find((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);

            //Enable/Disable Release Detained License
            if(License.IsDetained)
                releaseDetainedLicenseToolStripMenuItem.Enabled = true;
            else
                releaseDetainedLicenseToolStripMenuItem.Enabled = false;
        }
    }
}
