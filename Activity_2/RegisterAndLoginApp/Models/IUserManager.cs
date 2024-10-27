using System.Collections.Generic;

namespace RegisterAndLoginApp.Models
{
    public interface IUserManager
    {
        List<UserModel> GetAllUsers();
        UserModel? GetUserById(int id);
        void AddUser(UserModel user);
        void DeleteUser(int id);
        void UpdateUser(UserModel user);
        int CheckCredentials(string username, string password);
    }
}
