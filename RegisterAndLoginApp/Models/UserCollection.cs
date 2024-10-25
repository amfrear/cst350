using System;
using System.Collections.Generic;
using System.Linq;

namespace RegisterAndLoginApp.Models
{
    public class UserCollection : IUserManager
    {
        private List<UserModel> _users;

        public UserCollection()
        {
            _users = new List<UserModel>();
            GenerateUserData();
        }

        private void GenerateUserData()
        {
            AddUser(new UserModel { Username = "User1", PasswordHash = "password1", Salt = "salt1", Groups = "Group1" });
            AddUser(new UserModel { Username = "User2", PasswordHash = "password2", Salt = "salt2", Groups = "Group2" });
        }

        public void AddUser(UserModel user)
        {
            user.Id = _users.Count + 1; // Assign a new ID
            _users.Add(user);
        }

        public bool CheckCredentials(string username, string password)
        {
            return _users.Any(user => user.Username == username && user.PasswordHash == password);
        }

        public void DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        public List<UserModel> GetAllUsers()
        {
            return _users;
        }

        public UserModel GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void UpdateUser(UserModel updatedUser)
        {
            var user = GetUserById(updatedUser.Id);
            if (user != null)
            {
                user.Username = updatedUser.Username;
                user.PasswordHash = updatedUser.PasswordHash;
                user.Salt = updatedUser.Salt;
                user.Groups = updatedUser.Groups;
            }
        }
    }
}
