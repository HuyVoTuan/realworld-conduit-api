using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace RealworldConduit.Infrastructure.Helpers
{
    public class GenerateSlugHelper
    {
        public static string GenerateSlug(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var sanitizedString = Regex.Replace(stringBuilder.ToString().ToLower(), @"[^a-z0-9\s-]", "");

            sanitizedString = Regex.Replace(sanitizedString, @"\s+", "-").Trim();

            return sanitizedString;
        }
    }
}
