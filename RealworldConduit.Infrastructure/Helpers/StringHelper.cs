using System.Security.Cryptography;

namespace RealworldConduit.Infrastructure.Helpers
{
    public class StringHelper
    {
        public static string GenerateSlug(string input)
        {
            int slugLength = 2;

            byte[] randomBytes = new byte[slugLength];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string randomSlug = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();

            string finalSlug = $"{input}-{randomSlug}";

            return finalSlug;
        }

        public static string GenerateRefreshToken()
        {
            const int length = 32;
            char[] randomChars = new char[length];
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


            Random random = new Random();

            {
                for (int i = 0; i < length; i++)
                {
                    randomChars[i] = chars[random.Next(chars.Length)];
                }
            }

            long timestamp = DateTime.UtcNow.Ticks;
            string uniquePart = Convert.ToString(timestamp, 8);

            string uniqueRandomString = new string(randomChars) + uniquePart;

            if (uniqueRandomString.Length > length)
            {
                uniqueRandomString = uniqueRandomString.Substring(0, length);
            }

            return uniqueRandomString;
        }
    }
}
