using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsLicense
    {
        private enum enMode { AddNew = 0 , Update = 1}
        private enMode _Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 }

        public int LicenseID {  get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public clsDriver DriverInfo;
        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFeed { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public string IssueReasonText
        {
            get { return GetIssueReasonText(this.IssueReason); }
        }
        public int CreatedByUserID { get; set; }
        public clsDetainedLicense DetainedLicenseInfo;

        public bool IsDetained
        {
            get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID); }
        }

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFeed = -1;
            this.IsActive = false;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            _Mode = enMode.AddNew;
        }

        private clsLicense(int LicenseID , int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFeed, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClassID = LicenseClassID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFeed = PaidFeed;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDriver.Find(DriverID);
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            this.DetainedLicenseInfo = clsDetainedLicense.FindByLicenseID(LicenseID);

            _Mode = enMode.Update;
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClassID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFeed = -1;
            bool IsActive = false;
            int IssueReason = -1;
            int CreatedByUserID = -1;

            if(clsLicenseDataAccess.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID,
                ref LicenseClassID, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFeed,
                ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate,
                    ExpirationDate, Notes, PaidFeed, IsActive, (enIssueReason) IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsLicense FindByApplicationID(int ApplicationID)
        {
            int LicenseID = -1, DriverID = -1, LicenseClassID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFeed = -1;
            bool IsActive = false;
            int IssueReason = -1;
            int CreatedByUserID = -1;

            if (clsLicenseDataAccess.GetLicenseInfoByApplicationID(ApplicationID, ref LicenseID, ref DriverID,
                ref LicenseClassID, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFeed,
                ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate,
                    ExpirationDate, Notes, PaidFeed, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseDataAccess.AddNewLicense(this.ApplicationID, this.DriverID,
                this.LicenseClassID, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFeed,
                this.IsActive, (int)this.IssueReason, this.CreatedByUserID);

            return (this.LicenseID != -1);
        }

        private bool _UpdateLicense()
        {
            return clsLicenseDataAccess.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID,
                this.LicenseClassID, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFeed,
                this.IsActive, (int)this.IssueReason, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    {
                        if(_AddNewLicense())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        if(_UpdateLicense())
                        {
                            return true;
                        }
                        else
                            return false;
                    }
            }

            return false;
        }
        
        public static DataTable ShowDriverLocalLicensesHistory(int DriverID)
        {
            return clsLicenseDataAccess.ShowDriverLocalLicensesHistory(DriverID);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseDataAccess.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (clsLicenseDataAccess.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public bool IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }

        public bool DeactivateCurrentLicense()
        {
            return clsLicenseDataAccess.DeactivateLicense(this.LicenseID);
        }

        public bool ActivateCurrentLicense()
        {
            return clsLicenseDataAccess.ActivateLicense(this.LicenseID);
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {
            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement For Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement For Lost";
                default:
                    return "Unknown Reason";
            }
        }

        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {
            // Check if the license is expired or not
            if (!this.IsLicenseExpired())
                return null;

            // Check if the license is active or not
            if (!this.IsActive)
                return null;

            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if(!Application.Save())
            {
                return null;
            }

            //After creating the application, we can create the new license
            clsLicense NewLicnese = new clsLicense();

            NewLicnese.ApplicationID = Application.ApplicationID;
            NewLicnese.DriverID = this.DriverID;
            NewLicnese.LicenseClassID = this.LicenseClassID;
            NewLicnese.IssueDate = DateTime.Now;
            NewLicnese.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicnese.Notes = Notes;
            NewLicnese.PaidFeed = this.LicenseClassInfo.ClassFees;
            NewLicnese.IsActive = true;
            NewLicnese.IssueReason = enIssueReason.Renew;
            NewLicnese.CreatedByUserID = CreatedByUserID;

            if(!NewLicnese.Save())
            {
                return null;
            }

            //now we have to deactivate the current license
            DeactivateCurrentLicense();

            return NewLicnese;
        }

        public clsLicense ReplaceLicense(clsLicense.enIssueReason IssueReason, int ApplicationTypeID
            , int CreatedByUserID)
        {
            // Check if the license is expired or not
            if (this.IsLicenseExpired())
                return null;

            // Check if the license is active or not
            if (!this.IsActive)
                return null;

            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = ApplicationTypeID;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(ApplicationTypeID).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            //After creating the application, we can create the new license
            clsLicense NewLicnese = new clsLicense();

            NewLicnese.ApplicationID = Application.ApplicationID;
            NewLicnese.DriverID = this.DriverID;
            NewLicnese.LicenseClassID = this.LicenseClassID;
            NewLicnese.IssueDate = DateTime.Now;
            NewLicnese.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicnese.Notes = "";
            NewLicnese.PaidFeed = 0; // PaidFees will be set to 0, as it is not required for replacement
            NewLicnese.IsActive = true;
            NewLicnese.IssueReason = IssueReason;
            NewLicnese.CreatedByUserID = CreatedByUserID;

            if (!NewLicnese.Save())
            {
                return null;
            }

            //now we have to deactivate the current license
            DeactivateCurrentLicense();

            return NewLicnese;
        }

        public int DetainLicense(int FineFees, int CreatedByUserID)
        {
            if(this.IsDetained)
            {
                //License is already detained, cannot detain again
                return -1;
            }

            clsDetainedLicense DetainedLicense = new clsDetainedLicense();

            DetainedLicense.LicenseID = this.LicenseID;
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = FineFees;
            DetainedLicense.CreatedByUserID = CreatedByUserID;
            DetainedLicense.IsReleased = false;

            if(!DetainedLicense.Save())
            {
                return -1;
            }

            // After saving the detained license, we now update the current license status to not active
            this.DeactivateCurrentLicense();

            return DetainedLicense.DetainID;
        }

        public bool ReleaseDetainedLicense( int ReleasedByUserID, ref int ApplicationID)
        {
            clsDetainedLicense DetainedLicense = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            if(DetainedLicense == null || DetainedLicense.IsReleased)
            {
                // Detained license not found or already released
                return false;
            }

            //Now creating the release application
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;
            // After releasing the detained license, we change the current license status to active
            this.ActivateCurrentLicense();

            // Now we can release the detained license
            return this.DetainedLicenseInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);
        }
    }
}
