using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using WordCounter.DTO;

namespace WordCounter
{
    // Accepts Unicode accented characters
    internal class Parser
    {
        //Unicode Diacritical Range: x00c0-x00ff
        private const int ACCENTED_RANGE_MIN = 192;
        private const int ACCENTED_RANGE_MAX = 255;
        //Standart vowels:
        private const string REGULAR_VOWELS = "aeiouAEIOUяиюыаоэуеёЯИЮЫАОЭУЕЁ";
        private static readonly char[] _includeUnicodeDiacriticals = { '\u04e7', '\u0456' };
        private static readonly char[] _excludeUnicodeDiacriticals = {  };
        //RegEx patterns:
        private static readonly Regex _unicodeVowelsPattern = InitVowelsRegex();

        //
        // Summary:
        //     Trims json brackets
        // 
        // Parameters:
        //     String to process
        //
        // Returns:
        //     String without json brackets
        internal string ConvertJson(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<TextResponse>(input).Text;
            }
            catch(Exception ex)
            {
                MessageBox.Show("SERVER_ERROR: " + ex.Message);
                return string.Empty;
            }
        }

        //
        // Summary:
        //     Counts vowels in a line
        // 
        // Parameters:
        //     String to process
        //
        // Returns:
        //     Number of vowels
        internal int CountVowels(string input)
        {
            //I can only count symbols, not "vowels" - because it is sound.
            return _unicodeVowelsPattern.Matches(input).Count;
        }

        //
        // Summary:
        //     Counts words in a line
        // 
        // Parameters:
        //     String to process
        //
        // Returns:
        //     Word count
        internal int CountWords(string input)
        {
            return input.Split(' ').Length;
        }

        //
        // Summary:
        /// Creates a regex pattern to parse text with accented characters
        private static Regex InitVowelsRegex()
        {
            //Single Unicode accented characters:
            IEnumerable<char> diacriticalRange = new List<int>(Enumerable.Range(ACCENTED_RANGE_MIN, (ACCENTED_RANGE_MAX - ACCENTED_RANGE_MIN) + 1))
                .Select(i => (char)i)
                .ToArray()
                .Union(_includeUnicodeDiacriticals)
                .Except(_excludeUnicodeDiacriticals);
            return new Regex("[" + REGULAR_VOWELS + new string(diacriticalRange.ToArray()) + "]");
        }
    }
}