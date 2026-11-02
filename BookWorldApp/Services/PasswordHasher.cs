using System.Security.Cryptography;
using System.Text;

namespace BookWorld.Services;

public static class PasswordHasher
{
    private const char Separator = ':';
    private const int SaltSize = 16;

    public static string Create(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        return $"{Convert.ToHexString(salt)}{Separator}{Convert.ToHexString(Hash(password, salt))}";
    }

    public static bool Verify(string password, string stored)
    {
        var parts = stored.Split(Separator);
        if (parts.Length != 2) return false;

        try
        {
            var salt = Convert.FromHexString(parts[0]);
            var expected = Convert.FromHexString(parts[1]);
            return CryptographicOperations.FixedTimeEquals(Hash(password, salt), expected);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static byte[] Hash(string password, byte[] salt)
        => SHA256.HashData([.. salt, .. Encoding.Unicode.GetBytes(password)]);
}
