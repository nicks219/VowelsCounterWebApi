using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TMG1DotNetCoreWPF
{
    internal class Parser
    {
        private HashSet<int> _idsForRequest;
        private readonly List<char> _charsUnicodeDiacritical;
        private readonly string _unicodeVowelsPattern;
        private const int MIN_INDEX = 1;
        private const int MAX_INDEX = 20;
        private const int ERROR_RESPONSE = -1;
        //Unicode Range: x00c0-x00ff
        private const int DIACRITICAL_RANGE_MIN = 192;
        private const int DIACRITICAL_RANGE_MAX = 255;
        //Standart vowels:
        private const string VOWELS = "aeiouAEIOUяиюыаоэуеёЯИЮЫАОЭУЕЁ";
        //Net Access:
        private const string TOKEN_NAME = "TMG-Api-Key";
        private const string TOKEN_VALUE = "";
        private const string URL = "https://tmgwebtest.azurewebsites.net/api/textstrings/";

        internal Parser()
        {
            //Unicode Single Chars:
            _charsUnicodeDiacritical = new List<char> { '\u04e7', '\u0456' };
            for (int i = DIACRITICAL_RANGE_MIN; i <= DIACRITICAL_RANGE_MAX; i++)
            {
                _charsUnicodeDiacritical.Add((char)i);
            }
            _unicodeVowelsPattern = "[" + VOWELS + new string(_charsUnicodeDiacritical.ToArray()) + "]";
        }

        internal List<string> GetDataFromServer()
        {
            List<string> result = new();

            try
            {
                using WebClient wc = new WebClient { Proxy = null };
                wc.Headers.Add(TOKEN_NAME, TOKEN_VALUE);
                foreach (int id in _idsForRequest)
                {
                    result.Add(wc.DownloadString(URL + id.ToString()));
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }

        internal void CreateIdList(TextCompositionEventArgs e, TextBox textBox)
        {
            e.Handled = " ,;0123456789".IndexOf(e.Text) == ERROR_RESPONSE;
            //Correction after input
            if (",;".IndexOf(e.Text) != ERROR_RESPONSE)
            {
                GrubIdFrom(textBox);
                BuildTextFromIds();
                e.Handled = true;
            }

            void BuildTextFromIds()
            {
                StringBuilder result = new();
                foreach (var r in _idsForRequest)
                {
                    result.Append(r);
                    result.Append(", ");
                }
                textBox.Text = result.ToString();
                textBox.Focus();
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        internal void GrubIdFrom(TextBox textBox)
        {
            string[] text = textBox.Text.Split(',', ';');
            _idsForRequest = new();
            foreach (string identificator in text)
            {
                if (Int32.TryParse(identificator, out int id))
                {
                    if (id is >= MIN_INDEX and <= MAX_INDEX)
                    {
                        //Added the correct Ids only
                        _idsForRequest.Add(id);
                    }
                }

            }
        }

        internal string ParseJson(string input)
        {
            if (!Regex.IsMatch(input, _jsonTestRegex))
            {
                MessageBox.Show("Sorry, server goes bad..");
                return string.Empty;
            }
            var x = Regex.Replace(input, _jsonReplaceRegex, "");
            StringBuilder result = new(x);
            return result.ToString();
        }

        internal int CountVowels(string input)
        {
            //I can count only a chars, but not a "vowels" - because it's the sound.
            return new Regex(_unicodeVowelsPattern).Matches(input).Count;
        }

        internal int CountWords(string input)
        {
            return input.Split(' ').Count();
        }

        private readonly string _jsonTestRegex = "^({\"text\":\").*(\"})$";
        private readonly string _jsonReplaceRegex = "^({\"text\":\")|(\"})$";
    }
}