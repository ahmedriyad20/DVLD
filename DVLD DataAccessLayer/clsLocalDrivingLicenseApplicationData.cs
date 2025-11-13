using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    public static class clsLocalDrivingLicenseApplicationDataAccess
    {
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID,
            ref int ApplicationID , ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From LocalDrivingLicenseApplications Where
                                        LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                    LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                }
                else
                    isFound = false;

                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID, 
            ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From LocalDrivingLicenseApplications Where
                                                        ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LocalDrivingLicenseApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                    LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                }
                else
                    isFound = false;

                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                             Values (@ApplicationID, @LicenseClassID);
                             Select Scope_Identity()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                
                object result = command.ExecuteScalar();

                if(result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }
                else
                {
                    LocalDrivingLicenseApplicationID = -1;
                }

            }
            catch (Exception ex) { LocalDrivingLicenseApplicationID = -1; }
            finally
            {
                connection.Close();
            }

            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID,
            int ApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update LocalDrivingLicenseApplications
                             Set ApplicationID = @ApplicationID,
                             LicenseClassID = @LicenseClassID
                             Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { rowsAffected = 0; }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID,
                             LicenseClasses.ClassName, People.NationalNo, FullName = (People.FirstName + ' ' +
                             People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName),
                             Applications.ApplicationDate,
                             PassedTests = Count(CASE WHEN Tests.TestResult = 1 THEN 1 END), Status =

                             case
                        	 When Applications.ApplicationStatus = 1 Then 'New'
                        	 When Applications.ApplicationStatus = 2 Then 'Canceled'
                        	 When Applications.ApplicationStatus = 3 Then 'Completed'
                             End

                             from LocalDrivingLicenseApplications 
                             inner join LicenseClasses on LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                             inner join Applications on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             inner join People on Applications.ApplicantPersonID = People.PersonID
                             left join TestAppointments on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                             left join Tests on TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             Group by LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID, LicenseClasses.ClassName, People.NationalNo,
                             (People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName), Applications.ApplicationDate, 
                             Applications.ApplicationStatus";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dataTable.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex) { }
            finally
            {
                connection.Close();
            }

            return dataTable;
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete From LocalDrivingLicenseApplications
                                    Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally {  connection.Close(); }

            return rowsAffected > 0;
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Top 1 TestResult
                             From LocalDrivingLicenseApplications
                             Inner Join TestAppointments on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                             Inner Join Tests ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             AND TestAppointments.TestTypeID = @TestTypeID
                             order by Tests.TestID desc;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && bool.TryParse(result.ToString(), out bool returnedResult))
                {
                    Result = returnedResult;
                }
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return Result;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Top 1 Found = 1
                             From LocalDrivingLicenseApplications
                             Inner Join TestAppointments on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                             Inner Join Tests ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             AND TestAppointments.TestTypeID = @TestTypeID
                             order by Tests.TestID desc;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null )
                {
                    IsFound = true;
                }
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return IsFound;
        }

        public static int GetNumberOfTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int NumOfTrialsPerTests = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT NumOfTests = count(TestID)
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = 
                                 TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int Number))
                {
                    NumOfTrialsPerTests = Number;
                }
                else
                    NumOfTrialsPerTests = 0;
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return NumOfTrialsPerTests;
        }

        public static bool IsThereAnActiveTestAppointment(int LocalDrivingLicenseApplicationID,
            int TestTypeID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Top 1 Found = 1 From TestAppointments
                             Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             And TestTypeID = @TestTypeID
                             And IsLocked = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isFound = true;
                }
                else
                    isFound = false;

                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool IsLicenseIssued(int LocalDrivingLicenseApplicationID)
        {
            bool isIssued = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Top 1 Found = 1
                             From LocalDrivingLicenseApplications
                             inner join Applications on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             inner join Licenses on Applications.ApplicationID = Licenses.ApplicationID
                             Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isIssued = true;
                }
                else
                    isIssued = false;

                reader.Close();
            }
            catch (Exception ex) { isIssued = false; }
            finally { connection.Close(); }

            return isIssued;
        }

        public static int GetLocalDrivingLicenseApplicationIDLinkedToDriver(int DriverID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             From LocalDrivingLicenseApplications
                             Inner Join Licenses on LocalDrivingLicenseApplications.ApplicationID = Licenses.ApplicationID
                             Where Licenses.DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("DriverID", DriverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int LDLAppID))
                {
                    LocalDrivingLicenseApplicationID = LDLAppID;
                }
                else
                    LocalDrivingLicenseApplicationID = -1;
            }
            catch { }
            finally { connection.Close(); }

            return LocalDrivingLicenseApplicationID;
        }

    }
}
