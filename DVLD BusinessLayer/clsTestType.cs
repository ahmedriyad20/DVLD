using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }

        public enTestType TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public int TestTypeFees { get; set; }

        public clsTestType()
        {
            this.TestTypeID = enTestType.VisionTest; // Default to VisionTest
            this.TestTypeTitle = "";
            this.TestTypeFees = -1;

            Mode = enMode.AddNew;
        }

        public clsTestType(clsTestType.enTestType TestTypeID, string TestTypeTitle, string TestTypeDescription, int TestTypeFees)
        {
            this.TestTypeID = enTestType.VisionTest;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;

            Mode = enMode.Update;
        }

        public static clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            string TestTypeTitle = "", TestTypeDescription = "";
            int TestTypeFees = -1;

            if (clsTestTypeDataAccess.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeTitle, ref TestTypeDescription,
                ref TestTypeFees))
            {
                return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewTestType()
        {
            return true;
        }

        private bool _UpdateTestType()
        {
            return clsTestTypeDataAccess.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription,
                this.TestTypeFees);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if(_AddNewTestType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }

                case enMode.Update:
                    {
                        if (_UpdateTestType())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
            }

            return false;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeDataAccess.GetAllTestTypes();
        }
    }
}
