using System.Data.SqlClient;

namespace RegisterAndLoginApp.Models
{
    public class UserDAO : IUserManager
    {
        string connectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void AddUser(UserModel user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO dbo.Users (Username, PasswordHash, Salt, Groups) VALUES (@Username, @PasswordHash, @Salt, @Groups)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public int CheckCredentials(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to find the user by username
                string query = "SELECT * FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                SqlDataReader reader = command.ExecuteReader();

                // If user is found, verify the password
                if (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };

                    bool valid = user.VerifyPassword(password); // Assumes VerifyPassword method in UserModel

                    if (valid)
                        return user.Id; // User found and password is correct
                    else
                        return 0; // User found but password is incorrect
                }
            }

            return 0; // User not found
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        public List<UserModel> GetAllUsers()
        {
            // Initialize a list to store all users
            List<UserModel> users = new List<UserModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to retrieve all records from the Users table
                string query = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Loop through each record and populate the UserModel
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],  // Ensure Salt is retrieved as byte[]
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };

                    // Add the user to the list
                    users.Add(user);
                }
            }

            return users; // Return the list of all users
        }

        public UserModel GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to find a user by Id
                string query = "SELECT * FROM Users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                // If a matching user is found, populate and return the UserModel
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],  // Ensure Salt is retrieved as byte[]
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };

                    return user; // Return the matching user
                }
            }

            return null; // No matching user found
        }

        public void UpdateUser(UserModel user)
        {
            // Find the matching user by Id
            int id = user.Id;
            UserModel found = GetUserById(id);

            if (found != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to update the user's fields (excluding Id)
                    string query = @"UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, 
                             Salt = @Salt, Groups = @Groups WHERE Id = @Id";

                    SqlCommand command = new SqlCommand(query, connection);

                    // Add parameters to update fields
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    // Execute the update command
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
