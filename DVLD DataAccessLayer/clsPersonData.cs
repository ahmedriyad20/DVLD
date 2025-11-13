using System;
using System.Data;
using System.Data.SqlClient;


namespace DVLD_DataAccessLayer
{
    public static class clsPersonDataAccess
    {
        public static bool GetPersonInfoByID(int PersonID, ref string NationalNo, ref string FirstName, 
            ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref int Gendor,
            ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From People Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //Person is Found
                    isFound = true;

                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                        ThirdName = "";

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = Convert.ToInt32(reader["Gendor"]);
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                        Email = "";

                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                        ImagePath = "";
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName,
            ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref int Gendor,
            ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From People Where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //Person is Found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                        ThirdName = "";

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = Convert.ToInt32(reader["Gendor"]);
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                        Email = "";

                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                        ImagePath = "";
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewPerson( string NationalNo, string FirstName, string SecondName,
            string ThirdName, string LastName, DateTime DateOfBirth, int Gendor, string Address, 
            string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            //This Function will return the new contact id if succeeded and -1 if not
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into People
                             Values (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth,
                             @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("NationalNo", NationalNo);
            command.Parameters.AddWithValue("FirstName", FirstName);
            command.Parameters.AddWithValue("SecondName", SecondName);

            if(ThirdName != "")
            {
                command.Parameters.AddWithValue("ThirdName", ThirdName);
            }
            else
                command.Parameters.AddWithValue("ThirdName", "");

            command.Parameters.AddWithValue("LastName", LastName);
            command.Parameters.AddWithValue("DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("Gendor", Gendor);
            command.Parameters.AddWithValue("Address", Address);
            command.Parameters.AddWithValue("Phone", Phone);

            if(Email != "")
            {
                command.Parameters.AddWithValue("Email", Email);
            }
            else
                command.Parameters.AddWithValue("Email", "");

            command.Parameters.AddWithValue("NationalityCountryID", NationalityCountryID);

            if(!string.IsNullOrEmpty(ImagePath))
            {
                command.Parameters.AddWithValue("ImagePath", ImagePath);
            }
            else
                command.Parameters.AddWithValue("ImagePath", "");


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }
                else
                    PersonID = -1;
            }
            catch (Exception ex) { PersonID = -1; }
            finally { connection.Close(); }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName,
            string ThirdName, string LastName, DateTime DateOfBirth, int Gendor, string Address,
            string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            bool isUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update People
                             Set NationalNo = @NationalNo,
                             FirstName = @FirstName,
                             SecondName = @SecondName,
                             ThirdName = @ThirdName, 
                             LastName = @LastName, 
                             DateOfBirth = @DateOfBirth,
                             Gendor = @Gendor,
                             Address = @Address, 
                             Phone = @Phone, 
                             Email = @Email,
                             NationalityCountryID = @NationalityCountryID,
                             ImagePath = @ImagePath
                             Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("NationalNo", NationalNo);
            command.Parameters.AddWithValue("FirstName", FirstName);
            command.Parameters.AddWithValue("SecondName", SecondName);

            if (ThirdName != "")
            {
                command.Parameters.AddWithValue("ThirdName", ThirdName);
            }
            else
                command.Parameters.AddWithValue("ThirdName", "");

            command.Parameters.AddWithValue("LastName", LastName);
            command.Parameters.AddWithValue("DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("Gendor", Gendor);
            command.Parameters.AddWithValue("Address", Address);
            command.Parameters.AddWithValue("Phone", Phone);

            if (Email != "")
            {
                command.Parameters.AddWithValue("Email", Email);
            }
            else
                command.Parameters.AddWithValue("Email", "");

            command.Parameters.AddWithValue("NationalityCountryID", NationalityCountryID);

            if (!string.IsNullOrEmpty(ImagePath))
            {
                command.Parameters.AddWithValue("ImagePath", ImagePath);
            }
            else
                command.Parameters.AddWithValue("ImagePath",  "");

            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    isUpdated = true;
                }
                else
                    isUpdated = false;
            }
            catch (Exception ex) { isUpdated = false; }
            finally { connection.Close(); }

            return isUpdated;
        }

        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete From People 
                                        Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex) { return false; }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select People.PersonID, People.NationalNo, People.FirstName, People.SecondName,
                             People.ThirdName, People.LastName, GenderCaption =
                             Case
                             	When People.Gendor = 0 Then 'Male'
                             	When People.Gendor = 1 Then 'Female'
                             End,
                             People.DateOfBirth, Countries.CountryName, People.Phone, People.Email
                             From People Inner Join Countries On People.NationalityCountryID = Countries.CountryID
                             Order By People.FirstName";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {           
                    dt.Load(reader);
                }

                reader.Close();

            }
            catch (Exception ex) 
            {
                clsLogger.LogExecption(ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From People Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("PersonID", PersonID);

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
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From People Where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("NationalNo", NationalNo);

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
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
