using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;
using DVLD_Project.Classes;

namespace DVLD_Project.People
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        //We made it global static field to be loaded the enire lifespan of the program, they will be loaded on the RAM
        //because we don't want to go and grab the data from the database every time we need them
        private static DataTable _AllPeopleDataTable = clsPerson.GetAllPeople().DefaultView.ToTable();

        private void _RefreshAllPeople()
        {
            _AllPeopleDataTable = clsPerson.GetAllPeople();
            dgvListPeople.DataSource = _AllPeopleDataTable.DefaultView;
            
            lblRecordCount.Text = dgvListPeople.Rows.Count.ToString();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            _RefreshAllPeople();

            if(dgvListPeople.Rows.Count > 0)
            {
                dgvListPeople.Columns[0].HeaderText = "Person ID";
                dgvListPeople.Columns[0].Width = 120;

                dgvListPeople.Columns[1].HeaderText = "National No.";
                dgvListPeople.Columns[1].Width = 130;

                dgvListPeople.Columns[2].HeaderText = "First Name";
                dgvListPeople.Columns[2].Width = 130;

                dgvListPeople.Columns[3].HeaderText = "Second Name";
                dgvListPeople.Columns[3].Width = 130;

                dgvListPeople.Columns[4].HeaderText = "Third Name";
                dgvListPeople.Columns[4].Width = 130;

                dgvListPeople.Columns[5].HeaderText = "Last Name";
                dgvListPeople.Columns[5].Width = 120;

                dgvListPeople.Columns[6].HeaderText = "Gender";
                dgvListPeople.Columns[6].Width = 80;

                dgvListPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvListPeople.Columns[7].Width = 150;

                dgvListPeople.Columns[8].HeaderText = "Nationality";
                dgvListPeople.Columns[8].Width = 100;

                dgvListPeople.Columns[9].HeaderText = "Phone";
                dgvListPeople.Columns[9].Width = 100;

                dgvListPeople.Columns[10].HeaderText = "Email";
                dgvListPeople.Columns[10].Width = 190;
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshAllPeople();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshAllPeople();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshAllPeople();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshAllPeople();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete This Person?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsPerson Person = clsPerson.Find((int)dgvListPeople.CurrentRow.Cells[0].Value);

                if (clsPerson.DeletePerson(Person.PersonID))
                {
                    MessageBox.Show("Person Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if(clsUtil.DeletePersonImageFile(Person.ImagePath))
                    {

                    }
                    
                    _RefreshAllPeople();
                }
                else
                    MessageBox.Show("Failed To Delete The Person!", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

            _RefreshAllPeople();
        }

        private void dgvListPeople_DoubleClick(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshAllPeople();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if(txtFilterValue.Visible)
            {
                txtFilterValue.Focus();
                txtFilterValue.Text = "";
            }
            
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            
            
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
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
                case "First Name":
                    {
                        FilterColumn = "FirstName";
                        break;
                    }
                case "Second Name":
                    {
                        FilterColumn = "SecondName";
                        break;
                    }
                case "Third Name":
                    {
                        FilterColumn = "ThirdName";
                        break;
                    }
                case "Last Name":
                    {
                        FilterColumn = "LastName";
                        break;
                    }
                case "Nationality":
                    {
                        FilterColumn = "CountryName";
                        break;
                    }
                case "Gender":
                    {
                        FilterColumn = "GenderCaption";
                        break;
                    }
                case "Phone":
                    {
                        FilterColumn = "Phone";
                        break;
                    }
                case "Email":
                    {
                        FilterColumn = "Email";
                        break;
                    }

                default:
                    {
                        FilterColumn = "None";
                        break;
                    }
            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text == "" || cbFilterBy.Text == "None")
            {
                _AllPeopleDataTable.DefaultView.RowFilter = "";
                lblRecordCount.Text = dgvListPeople.Rows.Count.ToString();
                return;
            }

            if(FilterColumn == "PersonID")
            {
                //in this case we deal with integer not string
                _AllPeopleDataTable.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text);
            }
            else
            {
                _AllPeopleDataTable.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text);
            }

            lblRecordCount.Text = dgvListPeople.Rows.Count.ToString();
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
    }
}
