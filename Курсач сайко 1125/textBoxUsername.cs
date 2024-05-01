using Microsoft.Data.SqlClient;
using MySqlConnector;
namespace Курсач_сайко_1125
{
    internal class textBoxUsername
    {
        public static string Text { get; internal set; }

        public void GetDataFromDatabase()
        {
            // Create a connection to the database
            string connectionString = "server=localhost;user=root;password=student;database=dayn1";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // Create a command to retrieve the user data
            SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Username = @Username", connection);
            command.Parameters.AddWithValue("@Username", "username_from_textbox");

            // Execute the command and retrieve the results
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                // Data retrieved successfully
                string password = reader["Password"].ToString();
                // Do something with the data
            }
            else
            {
                // Data not found
            }

            // Close the database connection
            connection.Close();
        }
    }
}