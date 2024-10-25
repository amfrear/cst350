namespace RegisterAndLoginApp.Models
{
    public interface IUserManager
    {
        List<UserModel> GetAllUsers();
        UserModel GetUserById(int id);
        void AddUser(UserModel user);
        void DeleteUser(int id);
        void UpdateUser(UserModel user);
        bool CheckCredentials(string username, string password);
    }

}
