using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RealworldConduit.Infrastructure.Helpers
{
    public class GenerateSlugHelper
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
    }
}
