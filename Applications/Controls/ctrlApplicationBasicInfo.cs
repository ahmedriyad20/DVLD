using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;
using DVLD_Project.Classes;
using DVLD_Project.People;

namespace DVLD_Project.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsPerson _Person;

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_Person.PersonID);
            frm.ShowDialog();
        }

        public void ResetApplicationInfo()
        {
            lblApplicationID.Text = "[???]";
            lblStatus.Text = "[???]";
            lblFees.Text = "[???]";
            lblType.Text = "[???]";
            lblApplicant.Text = "[???]";
            lblDate.Text = "[???]";
            lblStatusDate.Text = "[???]";
            lblCreatedByUser.Text = "[???]";
            _Person = null;
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

            if (Application == null)
            {
                MessageBox.Show("Application not found.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ResetApplicationInfo();
                return;
            }

            clsPerson Person = clsPerson.Find(Application.ApplicantPersonID);
            _Person = Person;

            lblApplicationID.Text = Application.ApplicationID.ToString();

            lblStatus.Text =  (Application.ApplicationStatus == clsApplication.enApplicationStatus.New)? "New" : 
                (Application.ApplicationStatus == clsApplication.enApplicationStatus.Cancelled) ? "Cancelled" : "Completed";

            lblFees.Text = Application.PaidFees.ToString();
            lblType.Text = clsApplicationType.Find(Application.ApplicationTypeID).ApplicationTypeTitle;

            lblApplicant.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " +
                Person.LastName;

            lblDate.Text = clsFormat.DateToShort(Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(Application.LastStatusDate);

            lblCreatedByUser.Text = clsUser.Find(Application.CreatedByUserID).UserName;
        }
    }
}
