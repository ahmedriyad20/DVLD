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

namespace DVLD_Project.Applications.Applications_Types
{
    public partial class frmListApplicationTypes : Form
    {
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private DataTable _dtAllApplicationTypes;

        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            dgvListApplicationTypes.DataSource = _dtAllApplicationTypes;

            lblRecordCount.Text = dgvListApplicationTypes.Rows.Count.ToString();

            if(dgvListApplicationTypes.Rows.Count > 0)
            {
                dgvListApplicationTypes.Columns[0].HeaderText = "ID";
                dgvListApplicationTypes.Columns[0].Width = 90;

                dgvListApplicationTypes.Columns[1].HeaderText = "Title";
                dgvListApplicationTypes.Columns[1].Width = 320;

                dgvListApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvListApplicationTypes.Columns[2].Width = 105;
            }
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateApplicationType frm = new frmUpdateApplicationType((int)dgvListApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListApplicationTypes_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
