using System;
using System.Linq;
using System.Text;

public class SimpleEncryptionHelper
{
    private readonly Random random = new Random();

    public string Encrypt(string plainText)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainBytes);
    }

    public string Decrypt(string cipherText)
    {
        try
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            return Encoding.UTF8.GetString(cipherBytes);
        }
        catch (FormatException)
        {
            // Handle invalid base64 string
            return string.Empty;
        }
    }

    private string Key
    {
        get
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
