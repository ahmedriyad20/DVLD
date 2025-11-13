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
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Licenses.Local_Licenses;

namespace DVLD_Project.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private int _DriverID = -1;
        private clsDriver _Driver;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;

        private void _LoadDriverInternationalLicensesHistory()
        {
            _dtDriverInternationalLicensesHistory = clsDriver.ShowDriverInternationalLicensesHistory(_DriverID);
            dgvInternationalDriverLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory;

            lblInternationalLicensesRecord.Text = dgvInternationalDriverLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalDriverLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalDriverLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalDriverLicensesHistory.Columns[0].Width = 140;

                dgvInternationalDriverLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalDriverLicensesHistory.Columns[1].Width = 110;

                dgvInternationalDriverLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalDriverLicensesHistory.Columns[2].Width = 140;

                dgvInternationalDriverLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalDriverLicensesHistory.Columns[3].Width = 160;

                dgvInternationalDriverLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalDriverLicensesHistory.Columns[4].Width = 160;

                dgvInternationalDriverLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalDriverLicensesHistory.Columns[5].Width = 90;

            }
        }

        private void _LoadDriverLocalLicensesHistory()
        {
            _dtDriverLocalLicensesHistory = clsDriver.ShowDriverLocalLicensesHistory(_DriverID);
            dgvLocalDriverLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            lblLocalLicensesRecord.Text = dgvLocalDriverLicensesHistory.Rows.Count.ToString();

            if(dgvLocalDriverLicensesHistory.Rows.Count > 0 )
            {
                dgvLocalDriverLicensesHistory.Columns[0].HeaderText = "Lic. ID";
                dgvLocalDriverLicensesHistory.Columns[0].Width = 80;

                dgvLocalDriverLicensesHistory.Columns[1].HeaderText = "App. ID";
                dgvLocalDriverLicensesHistory.Columns[1].Width = 85;

                dgvLocalDriverLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalDriverLicensesHistory.Columns[2].Width = 240;

                dgvLocalDriverLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalDriverLicensesHistory.Columns[3].Width = 160;

                dgvLocalDriverLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalDriverLicensesHistory.Columns[4].Width = 160;

                dgvLocalDriverLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalDriverLicensesHistory.Columns[5].Width = 75;
            }
        }

        public void LoadDriverLicensesInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDriver.Find(DriverID);

            if(_Driver == null)
            {
                MessageBox.Show("No driver with ID = " + _DriverID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadDriverLocalLicensesHistory();
            _LoadDriverInternationalLicensesHistory();
        }

        public void LoadDriverLicensesInfoByPersonID(int PersonID)
        {
            _Driver = clsDriver.FindByPersonID(PersonID);

            if (_Driver == null)
            {
                MessageBox.Show("No driver linked with person ID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _DriverID = _Driver.DriverID;

            _LoadDriverLocalLicensesHistory();
            _LoadDriverInternationalLicensesHistory();
        }

        public void Clear()
        {
            _dtDriverLocalLicensesHistory.Clear();
            _dtDriverInternationalLicensesHistory.Clear();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLocalDriverLicensesHistory.Rows.Count <= 0)
                return;

            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvLocalDriverLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvInternationalDriverLicensesHistory.Rows.Count <= 0)
                return;

            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo
                ((int)dgvInternationalDriverLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
