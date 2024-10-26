using System.Security.Cryptography;
using System.Text;

public class UserModel
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Salt { get; set; }
    public string? Groups { get; set; }

    public void SetPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            // Combine the salt and password for hashing
            var saltBytes = Encoding.UTF8.GetBytes(Salt ?? string.Empty);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();
            var hashBytes = sha256.ComputeHash(combinedBytes);
            PasswordHash = Convert.ToBase64String(hashBytes);
        }
    }

    public bool VerifyPassword(string password)
    {
        if (PasswordHash == null) return false;

        using (var sha256 = SHA256.Create())
        {
            // Hash the provided password with the same salt
            var saltBytes = Encoding.UTF8.GetBytes(Salt ?? string.Empty);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();
            var hashBytes = sha256.ComputeHash(combinedBytes);
            var computedHash = Convert.ToBase64String(hashBytes);

            // Compare computed hash with stored hash
            return computedHash == PasswordHash;
        }
    }
}
