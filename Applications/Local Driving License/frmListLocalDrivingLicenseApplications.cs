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
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.Tests;

namespace DVLD_Project.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private DataTable _dtAllLocalDrivingLicenseApplications;

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {

            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplication.DataSource = _dtAllLocalDrivingLicenseApplications;

            lblRecordCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;

            if(dgvLocalDrivingLicenseApplication.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplication.Columns[0].HeaderText = "L.D.L AppID";
                dgvLocalDrivingLicenseApplication.Columns[0].Width = 120;

                dgvLocalDrivingLicenseApplication.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplication.Columns[1].Width = 300;

                dgvLocalDrivingLicenseApplication.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplication.Columns[2].Width = 125;

                dgvLocalDrivingLicenseApplication.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplication.Columns[3].Width = 350;

                dgvLocalDrivingLicenseApplication.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplication.Columns[4].Width = 170;

                dgvLocalDrivingLicenseApplication.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplication.Columns[5].Width = 120;

                dgvLocalDrivingLicenseApplication.Columns[6].HeaderText = "Status";
                dgvLocalDrivingLicenseApplication.Columns[6].Width = 120;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo(
                (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[5].Value);
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication(
                (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(
                    (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);

                if (localDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to Delete the Application!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApplication = clsLocalDrivingLicenseApplication.Find(
                (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);

            if (LDLApplication == null)
            {
                MessageBox.Show("Error: No Application Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to cancel this application?", "Cancel Application",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                if (LDLApplication.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Error: Unable to cancel application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = 
                clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseApplicationID);

            bool IsLicenseIssued = LocalDrivingLicenseApplication.IsLicenseIssued();

            clsApplication Application = clsApplication.FindBaseApplication(LocalDrivingLicenseApplication.ApplicationID);

            int PassedTests = (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[5].Value;

            //Enable/Disable Edit, Delete Application menu items
            editApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            deleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New || LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.Cancelled);

            //Enable/Disable Cancel Application menu item
            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            //Enable Disable Schedule menue and it's sub menue
            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest); ;
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            //Enable/Disable Schedule Tests menu item based on Application Status and License
            scheduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            //Enable/Disable Schedule Vision, Written, Street Tests menu items
            if (scheduleTestsToolStripMenuItem.Enabled)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }

            //Enable/Disable Issue Driving License (First Time) menu item
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (!IsLicenseIssued && PassedTests == 3);

            //Enable/Disable Show license menu item
            showLicenseToolStripMenuItem.Enabled = IsLicenseIssued;

        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments(
                (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, clsTestType.enTestType.VisionTest);

            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments(
               (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, clsTestType.enTestType.WrittenTest);

            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments(
               (int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, clsTestType.enTestType.StreetTest);

            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApplication = clsLocalDrivingLicenseApplication.Find
                ((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);

            clsLicense License = clsLicense.FindByApplicationID(LDLApplication.ApplicationID);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(License.LicenseID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApplication = clsLocalDrivingLicenseApplication.Find
                ((int)dgvLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);

            clsApplication Application = clsApplication.FindBaseApplication(LDLApplication.ApplicationID);

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(Application.ApplicantPersonID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecordCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            DataView DataView = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications().DefaultView;
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    {
                        FilterColumn = "LocalDrivingLicenseApplicationID";
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
                case "Status":
                    {
                        FilterColumn = "Status";
                        break;
                    }

                default:
                    {
                        FilterColumn = "None";
                        break;
                    }
            }

            if (txtFilterValue.Text == "" || cbFilterBy.Text == "None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecordCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
                return;
            }

            if(FilterColumn == "LocalDrivingLicenseApplicationID")
                //means we deal with integer not string
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = FilterColumn + " = " + txtFilterValue.Text;
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = FilterColumn + " LIKE '%" + txtFilterValue.Text + "%'";

            lblRecordCount.Text = dgvLocalDrivingLicenseApplication.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
