using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegisterAndLoginApp.Models
{
    public class UserCollection : IUserManager
    {
        // This is an in-memory list of users. In a real application, this would be a database connection.
        // By convention, the underscore prefix indicates a private field.
        private List<UserModel> _users;

        // Constructor that initializes the list of users
        public UserCollection()
        {
            _users = new List<UserModel>();
            // Create some user accounts
            GenerateUserData();
        }

        // Method to generate initial user data
        private void GenerateUserData()
        {
            UserModel user1 = new UserModel();
            user1.Username = "Harry";
            user1.Salt = Encoding.UTF8.GetBytes("defaultSalt"); // Use a default salt for testing
            user1.SetPassword("prince");
            user1.Groups = "Admin";
            AddUser(user1);

            UserModel user2 = new UserModel();
            user2.Username = "Megan";
            user2.Salt = Encoding.UTF8.GetBytes("defaultSalt"); // Use the same salt for simplicity in testing
            user2.SetPassword("princess");
            user2.Groups = "Admin, User";
            AddUser(user2);
        }

        // Add a new user to the list, generating an ID automatically
        public void AddUser(UserModel user)
        {
            user.Id = _users.Count + 1;
            _users.Add(user);
        }

        // Check user credentials for login
        public int CheckCredentials(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user != null && user.VerifyPassword(password)) // Use VerifyPassword for checking
            {
                return user.Id;
            }
            return 0; // Return 0 if credentials are invalid
        }

        // Delete a user from the list
        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        // Get all users
        public List<UserModel> GetAllUsers()
        {
            return _users;
        }

        // Get user by ID using a lambda expression
        public UserModel? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        // Update an existing user's details
        public void UpdateUser(UserModel user)
        {
            // Find the user with the matching ID and replace it
            _users[_users.FindIndex(u => u.Id == user.Id)] = user;
        }
    }
}
