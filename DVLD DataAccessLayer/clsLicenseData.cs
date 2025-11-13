using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class clsLicenseDataAccess
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID,
            ref int LicenseClassID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
            ref float PaidFees, ref bool IsActive, ref int IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From Licenses Where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //License is Found
                    isFound = true;

                    ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                    DriverID = Convert.ToInt32(reader["DriverID"]);
                    LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    if ((string)reader["Notes"] != null)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                        Notes = "";

                    PaidFees = Convert.ToInt32(reader["PaidFees"]);
                    IsActive = Convert.ToBoolean(reader["IsActive"]);
                    IssueReason = Convert.ToInt32(reader["IssueReason"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }

                reader.Close();
            }
            catch { }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetLicenseInfoByApplicationID(int ApplicationID, ref int LicenseID, ref int DriverID,
            ref int LicenseClassID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
            ref float PaidFees, ref bool IsActive, ref int IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From Licenses Where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //License is Found
                    isFound = true;

                    LicenseID = Convert.ToInt32(reader["LicenseID"]);
                    DriverID = Convert.ToInt32(reader["DriverID"]);
                    LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    if ((string)reader["Notes"] != null)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                        Notes = "";

                    PaidFees = Convert.ToInt32(reader["PaidFees"]);
                    IsActive = Convert.ToBoolean(reader["IsActive"]);
                    IssueReason = Convert.ToInt32(reader["IssueReason"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }

                reader.Close();
            }
            catch { }
            finally { connection.Close(); }

            return isFound;
        }

        public static int AddNewLicense( int ApplicationID,  int DriverID,
             int LicenseClassID,  DateTime IssueDate,  DateTime ExpirationDate,  string Notes,
             float PaidFeed,  bool IsActive,  int IssueReason,  int CreatedByUserID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Licenses
                             Values(@ApplicationID, @DriverID, @LicenseClassID,
                             @IssueDate, @ExpirationDate, @Notes , @PaidFeed, @IsActive,
                             @IssueReason, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("DriverID", DriverID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("IssueDate", IssueDate);
            command.Parameters.AddWithValue("ExpirationDate", ExpirationDate);

            if(Notes != "")
            {
                command.Parameters.AddWithValue("Notes", Notes);
            }
            else
                command.Parameters.AddWithValue("Notes", "");

            command.Parameters.AddWithValue("PaidFeed", PaidFeed);
            command.Parameters.AddWithValue("IsActive", IsActive);
            command.Parameters.AddWithValue("IssueReason", IssueReason);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
                else
                    LicenseID = -1;
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID,
             int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes,
             float PaidFees, bool IsActive, int IssueReason, int CreatedByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Licenses
                             Set ApplicationID = @ApplicationID,
                             DriverID = @DriverID,
                             LicenseClassID = @LicenseClassID,
                             IssueDate = @IssueDate,
                             ExpirationDate = @ExpirationDate,
                             Notes = @Notes ,
                             PaidFees = @PaidFees,
                             IsActive = @IsActive,
                             IssueReason = @IssueReason,
                             CreatedByUserID = @CreatedByUserID
                             Where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("DriverID", DriverID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("IssueDate", IssueDate);
            command.Parameters.AddWithValue("ExpirationDate", ExpirationDate);

            if (Notes != "")
            {
                command.Parameters.AddWithValue("Notes", Notes);
            }
            else
                command.Parameters.AddWithValue("Notes", "");

            command.Parameters.AddWithValue("PaidFees", PaidFees);
            command.Parameters.AddWithValue("IsActive", IsActive);
            command.Parameters.AddWithValue("IssueReason", IssueReason);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static DataTable ShowDriverLocalLicensesHistory(int DriverID)
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName,
                             Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
                             From Licenses
                             Inner Join LicenseClasses on Licenses.LicenseClassID = LicenseClasses.LicenseClassID
                             Where DriverID = @DriverID
                             Order By IsActive desc, ExpirationDate desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("DriverID", DriverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return dataTable;
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Licenses.LicenseID
                             From Licenses
                             inner join Drivers on Licenses.DriverID = Drivers.DriverID
                             Where Drivers.PersonID = @PersonID
                             AND Licenses.LicenseClassID = @LicenseClassID
                             AND IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("PersonID", PersonID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int returnedLicense))
                {
                    LicenseID = returnedLicense;
                }
                else
                    LicenseID = -1;
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Licenses
                             Set IsActive = 0
                             Where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static bool ActivateLicense(int LicenseID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Licenses
                             Set IsActive = 1
                             Where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }
    }
}
