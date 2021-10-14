using System.Collections.Generic;
using System.Net;
using System.Windows;

namespace TMG1DotNetCoreWPF
{
    internal class Web
    {
        //Net Access:
        private readonly string _tokenName;
        private readonly string _tokenValue;
        private readonly string _url;

        internal Web(string tokenName, string tokenValue, string url)
        {
            _tokenName = tokenName;
            _tokenValue = tokenValue;
            _url = url;
        }

        /// <summary>
        /// Retrieves data from a remote server based on a list of text identifiers
        /// </summary>
        /// <returns>List of strings</returns>
        internal List<string> GetDataFromServer(HashSet<int> _idsForRequest)
        {
            List<string> result = new();
            try
            {
                using WebClient wc = new() { Proxy = null };
                wc.Headers.Add(_tokenName, _tokenValue);
                //List with text identifiers:
                foreach (int id in _idsForRequest)
                {
                    result.Add(wc.DownloadString(_url + id.ToString()));
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }
    }

    internal class Token
    {
        internal string TokenValue { get; set; }
        internal string TokenName { get; set; }
    }
}