using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            List<char> _charsUnicodeDiacritical;
            //Single Unicode accented characters:
            _charsUnicodeDiacritical = new List<char> { '\u04e7', '\u0456' };
            for (int i = ACCENTED_RANGE_MIN; i <= ACCENTED_RANGE_MAX; i++)
            {
                _charsUnicodeDiacritical.Add((char)i);
            }
            return new Regex("[" + REGULAR_VOWELS + new string(_charsUnicodeDiacritical.ToArray()) + "]");
        }
    }
}