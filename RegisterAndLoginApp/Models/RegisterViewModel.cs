namespace RegisterAndLoginApp.Models
{
    // Internal class representing group selection for a user
    public class GroupViewModel
    {
        public bool IsSelected { get; set; }
        public string? GroupName { get; set; }
    }

    // ViewModel for the Registration form
    public class RegisterViewModel
    {
        // Properties for Username and Password
        public string Username { get; set; }
        public string Password { get; set; }

        // List of groups, represented using GroupViewModel
        public List<GroupViewModel> Groups { get; set; }

        // Constructor to initialize default group values
        public RegisterViewModel()
        {
            Username = "";
            Password = "";
            Groups = new List<GroupViewModel>
            {
                new GroupViewModel { GroupName = "Admin", IsSelected = false },
                new GroupViewModel { GroupName = "Users", IsSelected = false },
                new GroupViewModel { GroupName = "Students", IsSelected = false }
            };
        }
    }
}
