using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using TMG1DotNetCoreWPF.DTO;

namespace TMG1DotNetCoreWPF
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

        /// <summary>
        /// Trims json brackets
        /// </summary>
        /// <param name="input">String to process</param>
        /// <returns>String without json brackets</returns>
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

        /// <summary>
        /// Counts vowels in a line
        /// </summary>
        /// <param name="input">String to process</param>
        /// <returns>Number of vowels</returns>
        internal int CountVowels(string input)
        {
            //I can only count symbols, not "vowels" - because it is sound.
            return _unicodeVowelsPattern.Matches(input).Count;
        }

        /// <summary>
        /// Counts words in a line
        /// </summary>
        /// <param name="input">String to process</param>
        /// <returns>Word count</returns>
        internal int CountWords(string input)
        {
            return input.Split(' ').Length;
        }

        /// <summary>
        /// Creates a regex pattern to parse text with accented characters
        /// </summary>
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