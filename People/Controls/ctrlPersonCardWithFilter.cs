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

namespace DVLD_Project.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
            }
        }

        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.SelectedPersonInfo; }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
            cbFindBy.SelectedIndex = 0;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch(cbFindBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                    break;

                case "National No":
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;
            }

            if (OnPersonSelected != null && gbFilter.Enabled)
                // Raise the event with a parameter
                OnPersonSelected(ctrlPersonCard1.PersonID);

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();

            //Subscribe to the event and get the PersonID back
            frm.PersonIDSendBack += LoadPersonInfo;

            frm.ShowDialog();
        }

        public void LoadPersonInfo(int PersonID)
        {
            txtFilterValue.Text = PersonID.ToString();
            //_Person = clsPerson.Find(PersonID);
            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }

        public void EnableFilter(bool Enabled)
        {
            gbFilter.Enabled = Enabled;
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter (character code 13)
            if(e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }

            if (cbFindBy.SelectedIndex==1)
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This field is required!");
            }
            else
                errorProvider1.SetError(txtFilterValue, null);
        }
    }
}
