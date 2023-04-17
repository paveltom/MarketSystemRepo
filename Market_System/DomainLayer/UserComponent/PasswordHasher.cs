using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    private const int SaltSize = 8;
    private const int Iterations = 10000;
    private const int KeySize = 32;

    public static string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            HashAlgorithmName.SHA256);

        byte[] salt = algorithm.Salt;
        byte[] key = algorithm.GetBytes(KeySize);

        var result = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(key, 0, result, SaltSize, KeySize);

        return Convert.ToBase64String(result);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hashedPasswordBytes, 0, salt, 0, SaltSize);

        byte[] expectedKey = new byte[KeySize];
        Buffer.BlockCopy(hashedPasswordBytes, SaltSize, expectedKey, 0, KeySize);

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256);

        byte[] actualKey = algorithm.GetBytes(KeySize);

        return ByteArraysEqual(expectedKey, actualKey);
    }

    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (a == null && b == null)
        {
            return true;
        }
        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }
        bool equals = true;
        for (int i = 0; i < a.Length; i++)
        {
            equals &= (a[i] == b[i]);
        }
        return equals;
    }
}