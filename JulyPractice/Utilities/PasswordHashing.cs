using System;
using System.Security.Cryptography;

namespace JulyPractice
{
    internal class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 24;
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Пароль не идентифицирован.");

            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(enteredPassword))
                throw new ArgumentNullException(nameof(enteredPassword), "Пароль не идентифицирован.");
            if (string.IsNullOrEmpty(storedHash))
                throw new ArgumentNullException(nameof(storedHash), "Сохраненный хэш не идентифицирован.");

            try
            {
                byte[] hashBytes = Convert.FromBase64String(storedHash);

                byte[] salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                byte[] storedHashBytes = new byte[HashSize];
                Array.Copy(hashBytes, SaltSize, storedHashBytes, 0, HashSize);

                var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations);
                byte[] enteredHashBytes = pbkdf2.GetBytes(HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (storedHashBytes[i] != enteredHashBytes[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Ошибка верификации: " + ex.Message);
                return false;
            }
        }
    }
}
