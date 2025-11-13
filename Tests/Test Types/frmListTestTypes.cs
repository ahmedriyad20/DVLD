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


namespace DVLD_Project.Tests.Test_Types
{
    public partial class frmListTestTypes : Form
    {
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private DataTable _dtAllTestTypes;

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes = clsTestType.GetAllTestTypes();

            dgvListTestTypes.DataSource = _dtAllTestTypes;

            lblRecordCount.Text = dgvListTestTypes.Rows.Count.ToString();

            if (dgvListTestTypes.Rows.Count > 0)
            {
                dgvListTestTypes.Columns[0].HeaderText = "ID";
                dgvListTestTypes.Columns[0].Width = 90;

                dgvListTestTypes.Columns[1].HeaderText = "Title";
                dgvListTestTypes.Columns[1].Width = 180;

                dgvListTestTypes.Columns[2].HeaderText = "Description";
                dgvListTestTypes.Columns[2].Width = 430;

                dgvListTestTypes.Columns[3].HeaderText = "Fees";
                dgvListTestTypes.Columns[3].Width = 105;
            }
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestType frm = new frmEditTestType((int)dgvListTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListTestTypes_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
