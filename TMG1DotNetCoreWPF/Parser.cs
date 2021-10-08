using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TMG1DotNetCoreWPF
{
    internal class Parser
    {
        private HashSet<int> _identificators;
        private readonly List<char> _charsUnicodeSpecials;
        private readonly string _regexUnicodeSpecials;
        private readonly string token = "";
        private const int MIN_INDEX = 1;
        private const int MAX_INDEX = 20;
        private const int ERROR_RESPONSE = -1;
        //Unicode Range: x00c0-x00ff
        private const int DIACRITICAL_RANGE_MIN = 192;
        private const int DIACRITICAL_RANGE_MAX = 255;

        internal Parser()
        {
            //Unicode Single Chars:
            _charsUnicodeSpecials = new List<char> { '\u04e7', '\u0456' };
            for (int i = DIACRITICAL_RANGE_MIN; i <= DIACRITICAL_RANGE_MAX; i++)
            {
                _charsUnicodeSpecials.Add((char)i);
            }
            _regexUnicodeSpecials = new string(_charsUnicodeSpecials.ToArray());
        }

        internal List<string> GetDataFromServer()
        {
            List<string> result = new();

            try
            {
                using WebClient wc = new WebClient { Proxy = null };
                wc.Headers.Add("TMG-Api-Key", token);
                foreach (int id in _identificators)
                {
                    result.Add(wc.DownloadString("https://tmgwebtest.azurewebsites.net/api/textstrings/" + id.ToString()));
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
                foreach (var r in _identificators)
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
            _identificators = new();
            foreach (string identificator in text)
            {
                if (Int32.TryParse(identificator, out int id))
                {
                    if (id is >= MIN_INDEX and <= MAX_INDEX)
                    {
                        //Added only correct Ids
                        _identificators.Add(id);
                    }
                }

            }
        }

        internal string ParseJson(string input)
        {
            if (!Regex.IsMatch(input, "^({\"text\":\").*(\"})$"))
            {
                MessageBox.Show("Sorry, server goes bad..");
                return string.Empty;
            }
            var x = Regex.Replace(input, "^({\"text\":\")|(\"})$", "");
            StringBuilder result = new(x);
            return result.ToString();
        }

        internal int CountVowels(string input)
        {
            //I can count only chars, but not "vowels" - because it's the sound.
            return new Regex("[aeiouAEIOUяиюыаоэуеёЯИЮЫАОЭУЕЁ" + _regexUnicodeSpecials + "]").Matches(input).Count;
        }

        internal int CountWords(string input)
        {
            return input.Split(' ').Count();
        }

        private void PrintHeaders(WebClient wc)
        {
            foreach (string n in wc.ResponseHeaders.Keys)
            {
                //textBlock.Text += n + " = " + wc.ResponseHeaders[n] + "\n";}
            }
        }
    }

}