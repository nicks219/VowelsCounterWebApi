using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace TMG1DotNetCoreWPF
{
    // Accepts Unicode accented characters
    internal class Parser
    {
        private readonly List<char> _charsUnicodeDiacritical;
        private readonly string _unicodeVowelsPattern;
        //Unicode Diacritical Range: x00c0-x00ff
        private const int ACCENTED_RANGE_MIN = 192;
        private const int ACCENTED_RANGE_MAX = 255;
        //Standart vowels:
        private const string REGULAR_VOWELS = "aeiouAEIOUяиюыаоэуеёЯИЮЫАОЭУЕЁ";

        /// <summary>
        /// Creates a regex pattern to parse text with accented characters
        /// </summary>
        internal Parser()
        {
            //Single Unicode accented characters:
            _charsUnicodeDiacritical = new List<char> { '\u04e7', '\u0456' };
            for (int i = ACCENTED_RANGE_MIN; i <= ACCENTED_RANGE_MAX; i++)
            {
                _charsUnicodeDiacritical.Add((char)i);
            }
            _unicodeVowelsPattern = "[" + REGULAR_VOWELS + new string(_charsUnicodeDiacritical.ToArray()) + "]";
        }

        /// <summary>
        /// Trims json brackets
        /// </summary>
        /// <param name="input">String to process</param>
        /// <returns>String without json brackets</returns>
        internal string TrimJson(string input)
        {
            if (!Regex.IsMatch(input, _jsonTestRegex))
            {
                MessageBox.Show("Sorry, server goes bad..");
                return string.Empty;
            }
            return Regex.Replace(input, _jsonReplaceRegex, "");
        }

        /// <summary>
        /// Counts vowels in a line
        /// </summary>
        /// <param name="input">String to process</param>
        /// <returns>Number of vowels</returns>
        internal int CountVowels(string input)
        {
            //I can only count symbols, not "vowels" - because it is sound.
            return new Regex(_unicodeVowelsPattern).Matches(input).Count;
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

        //RegEx patterns:
        private static readonly string _jsonTestRegex = "^({\"text\":\").*(\"})$";
        private static readonly string _jsonReplaceRegex = "^({\"text\":\")|(\"})$";
    }
}