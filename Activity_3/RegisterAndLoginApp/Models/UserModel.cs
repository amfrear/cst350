using System.Security.Cryptography;
using System.Text;
using System;

public class UserModel
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public byte[]? Salt { get; set; }  // Updated to byte[]
    public string? Groups { get; set; }

    public void SetPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            // Use the Salt byte array directly
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = Salt.Concat(passwordBytes).ToArray();
            var hashBytes = sha256.ComputeHash(combinedBytes);
            PasswordHash = Convert.ToBase64String(hashBytes);
        }
    }

    public bool VerifyPassword(string password)
    {
        if (PasswordHash == null || Salt == null) return false;

        using (var sha256 = SHA256.Create())
        {
            // Hash the provided password with the stored salt
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = Salt.Concat(passwordBytes).ToArray();
            var hashBytes = sha256.ComputeHash(combinedBytes);
            var computedHash = Convert.ToBase64String(hashBytes);

            // Compare computed hash with stored hash
            return computedHash == PasswordHash;
        }
    }
}
