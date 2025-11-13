using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class clsTestDataAccess
    {
        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult, 
            ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From Tests Where TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("TestID", TestID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //Test is Found
                    isFound = true;

                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    TestResult = Convert.ToBoolean(reader["TestResult"]);

                    if ((string)reader["Notes"] != null)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                        Notes = "";

                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }

                reader.Close();
            }
            catch (Exception ex) {isFound = false;}
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID,
            int TestTypeID, ref int TestID, ref int TestAppointmentID, ref bool TestResult,
            ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT Top 1 Tests.TestID, Tests.TestAppointmentID, Tests.TestResult, Tests.Notes,
                             Tests.CreatedByUserID
                             From Tests
                             INNER JOIN TestAppointments On Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             INNER JOIN LocalDrivingLicenseApplications On TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             INNER JOIN Applications On LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             Where Applications.ApplicantPersonID = @ApplicantPersonID
                             AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                             AND TestAppointments.TestTypeID = @TestTypeID
                             ORDER BY Tests.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("ApplicantPersonID", PersonID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //Test is Found
                    isFound = true;

                    TestID = Convert.ToInt32(reader["TestID"]);
                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    TestResult = Convert.ToBoolean(reader["TestResult"]);

                    if ((string)reader["Notes"] != null)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                        Notes = "";

                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }

                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetTestInfoByTestAppointmentID(int TestAppointmentID, ref int TestID,  ref bool TestResult,
            ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From Tests Where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //Test is Found
                    isFound = true;

                    TestID = Convert.ToInt32(reader["TestID"]);
                    TestResult = Convert.ToBoolean(reader["TestResult"]);

                    if ((string)reader["Notes"] != null)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                        Notes = "";

                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }

                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally { connection.Close(); }

            return isFound;
        }

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Tests
                             Values(@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);

                             Update TestAppointments
                             SET isLocked = 1 Where TestAppointmentID = @TestAppointmentID;

                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("TestResult", TestResult);
            command.Parameters.AddWithValue("Notes", Notes);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
                else
                    TestID = -1;
            }
            catch(Exception ex) { }
            finally { connection.Close(); }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Tests
                             Set TestAppointmentID = @TestAppointmentID, 
                             TestResult = @TestResult,
                             Notes = @Notes, 
                             CreatedByUserID = @CreatedByUserID
                             Where TestID = @TestID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("TestResult", TestResult);
            command.Parameters.AddWithValue("Notes", Notes);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("TestID", TestID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static byte GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestsCount = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT PassedTestsCount = Count(Tests.TestID)
                             From Tests INNER JOIN TestAppointments On Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             Where TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             AND Tests.TestResult = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte PassedTests))
                {
                    PassedTestsCount = PassedTests;
                }
            }
            catch (Exception ex) { }
            finally
            {
                connection.Close();
            }

            return PassedTestsCount;
        }
    }
}
