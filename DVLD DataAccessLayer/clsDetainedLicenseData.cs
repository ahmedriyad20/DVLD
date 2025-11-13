using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class clsDetainedLicenseData
    {
        public static bool GetDetainedLicenseInfoByID(int DetainID, ref int LicenseID, ref DateTime DetainDate,
            ref int FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From DetainedLicenses Where DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("DetainID", DetainID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    //Detained License is Found

                    isFound = true;

                    LicenseID = Convert.ToInt32(reader["LicneseID"]);
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = Convert.ToInt32(reader["FineFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    IsReleased = Convert.ToBoolean(reader["IsReleased"]);

                    if (reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)reader["ReleaseDate"];
                    }
                    else
                        ReleaseDate = DateTime.MaxValue;

                    if (reader["ReleasedByUserID"] != DBNull.Value)
                        ReleasedByUserID = Convert.ToInt32(reader["ReleasedByUserID"]);
                    else
                        ReleasedByUserID = -1;

                    if (reader["ReleaseApplicationID"] != DBNull.Value)
                        ReleaseApplicationID = Convert.ToInt32(reader["ReleaseApplicationID"]);
                    else
                        ReleaseApplicationID = -1;

                }

                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate,
            ref int FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From DetainedLicenses Where LicenseID = @LicenseID And IsReleased = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //Detained License is Found

                    isFound = true;

                    DetainID = Convert.ToInt32(reader["DetainID"]);
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = Convert.ToInt32(reader["FineFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    IsReleased = Convert.ToBoolean(reader["IsReleased"]);

                    if (reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)reader["ReleaseDate"];
                    }
                    else
                        ReleaseDate = DateTime.MaxValue;

                    if (reader["ReleasedByUserID"] != DBNull.Value)
                        ReleasedByUserID = Convert.ToInt32(reader["ReleasedByUserID"]);
                    else
                        ReleasedByUserID = -1;

                    if (reader["ReleaseApplicationID"] != DBNull.Value)
                        ReleaseApplicationID = Convert.ToInt32(reader["ReleaseApplicationID"]);
                    else
                        ReleaseApplicationID = -1;

                }

                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally { connection.Close(); }

            return isFound;
        }

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, int FineFees,
            int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,int ReleasedByUserID, int ReleaseApplicationID)
        {
            int DetainID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into DetainedLicenses
                             Values(@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate,
                             @ReleasedByUserID, @ReleaseApplicationID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);
            command.Parameters.AddWithValue("DetainDate", DetainDate);
            command.Parameters.AddWithValue("FineFees", FineFees);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("IsReleased", IsReleased);

            command.Parameters.AddWithValue("ReleaseDate", DBNull.Value);

            command.Parameters.AddWithValue("ReleasedByUserID", DBNull.Value);

            command.Parameters.AddWithValue("ReleaseApplicationID", DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DetainID = insertedID;
                }
                else
                    DetainID = -1;
            }
            catch (Exception ex) { DetainID = -1; } 
            finally { connection.Close(); }

            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID ,int LicenseID, DateTime DetainDate, int FineFees,
            int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update DetainedLicenses
                             Set LicenseID = @LicenseID,
                             DetainDate = @DetainDate,
                             FineFees = @FineFees,
                             CreatedByUserID = @CreatedByUserID,
                             IsReleased = @IsReleased,
                             ReleaseDate = @ReleaseDate,
                             ReleasedByUserID = @ReleasedByUserID,
                             ReleaseApplicationID = @ReleaseApplicationID
                             Where DetainID = @DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);
            command.Parameters.AddWithValue("DetainDate", DetainDate);
            command.Parameters.AddWithValue("FineFees", FineFees);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("IsReleased", IsReleased);
            command.Parameters.AddWithValue("ReleaseDate", ReleaseDate);
            command.Parameters.AddWithValue("ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("DetainID", DetainID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { rowsAffected = 0; }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select DetainedLicenses.DetainID, DetainedLicenses.LicenseID,
                             DetainedLicenses.DetainDate, DetainedLicenses.IsReleased,DetainedLicenses.FineFees,
                             DetainedLicenses.ReleaseDate, People.NationalNo, FullName = (People.FirstName + ' ' +
                             People.SecondName + ' '+ People.ThirdName + ' ' + People.LastName),
                             DetainedLicenses.ReleaseApplicationID
                             From DetainedLicenses Inner Join Licenses On DetainedLicenses.LicenseID = Licenses.LicenseID
                             Inner Join Drivers On Licenses.DriverID = Drivers.DriverID
                             Inner Join People On Drivers.PersonID = People.PersonID
                             Order by IsReleased Asc;";

            SqlCommand command = new SqlCommand(query, connection);

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

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool isDetained = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select  IsDetained = 1 from DetainedLicenses
                             Where LicenseID = @LicenseID And IsReleased = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    isDetained = true;
                }
                else
                    isDetained = false;

                reader.Close();
            }
            catch (Exception ex) { isDetained = false; }
            finally { connection.Close(); }

            return isDetained;
        }

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update DetainedLicenses
                             Set IsReleased = 1,
                             ReleaseDate = @ReleaseDate,
                             ReleasedByUserID = @ReleasedByUserID,
                             ReleaseApplicationID = @ReleaseApplicationID
                             Where DetainID = @DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("ReleaseDate", DateTime.Now);
            command.Parameters.AddWithValue("ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("DetainID", DetainID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { rowsAffected = 0; }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }
    }
}

