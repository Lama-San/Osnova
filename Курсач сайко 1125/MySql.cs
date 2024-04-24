using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CollegeAdmissionAutomation
{
    public class MySqlDB
    {
        public class ApplicantRepository
        {
            public void InsertApplicants(ObservableCollection<Applicant> applicants)
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=student;database=dayn1"))
                {
                    connection.Open();

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (Applicant applicant in applicants)
                            {
                                MySqlCommand command = new MySqlCommand("INSERT INTO Applicants (ApplicantID, Name, GPA) VALUES (@ApplicantID, @Name, @GPA)", connection);
                                command.Parameters.AddWithValue("@ApplicantID", applicant.ApplicantID);
                                command.Parameters.AddWithValue("@Name", applicant.Name);
                                command.Parameters.AddWithValue("@GPA", applicant.GPA);

                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
        MySqlConnection mySqlConnection;

        private MySqlDB()
        {
            MySqlConnectionStringBuilder stringBuilder = new();
            stringBuilder.UserID = "student";
            stringBuilder.Password = "student";
            stringBuilder.Database = "drinks_1125";
            stringBuilder.Server = "192.168.200.13";
            stringBuilder.CharacterSet = "utf8mb4";
            //MySqlConnection = new MySqlConnection("server=192.168.200.13;user=student;password=student;database=drinks_1125;Character Set=utf8mb4");
            mySqlConnection = new MySqlConnection(stringBuilder.ToString());
            OpenConnection();
        }

        private bool OpenConnection()
        {
            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void CloseConnection()
        {
            try
            {
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal MySqlConnection GetConnection()
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                if (!OpenConnection())
                    return null;

            return mySqlConnection;
        }

        static MySqlDB instance;
        public static MySqlDB Instance
        {
            get
            {
                if (instance == null)
                    instance = new MySqlDB();
                return instance;
            }
        }

        public int GetAutoID(string table)
        {
            try
            {
                string sql = "SHOW TABLE STATUS WHERE `Name` = '" + table + "'";
                using (var mc = new MySqlCommand(sql, mySqlConnection))
                using (var reader = mc.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32("Auto_increment");
                }
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        //public void Test()
        //{
        //    try
        //    { 
        //        // безопасное выполнение кода, 
        //        // если произойдет ошибка, она будет перехвачена и выполнится блок catch
        //        mySqlConnection.Open();
        //        string str = "С'ироп";
        //        string sql = "INSERT INTO TagsTable VALUES (0, @tag)";
        //        using (MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection))
        //        {
        //            cmd.Parameters.Add(new MySqlParameter("tag", str));
        //            cmd.ExecuteNonQuery();
        //        }

        //        mySqlConnection.Close();
        //    }
        //    catch (Exception ex) 
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
    }
}