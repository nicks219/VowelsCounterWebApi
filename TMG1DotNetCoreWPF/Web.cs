using System.Collections.Generic;
using System.Net;
using System.Windows;

namespace TMG1DotNetCoreWPF
{
    internal static class Web
    {
        //Net Access:
        private const string TOKEN_NAME = "TMG-Api-Key";
        private const string TOKEN_VALUE = "";
        private const string URL = "https://tmgwebtest.azurewebsites.net/api/textstrings/";

        /// <summary>
        /// Retrieves data from a remote server based on a list of text identifiers
        /// </summary>
        /// <returns>List of strings</returns>
        internal static List<string> GetDataFromServer(HashSet<int> _idsForRequest)
        {
            List<string> result = new();
            try
            {
                using WebClient wc = new() { Proxy = null };
                wc.Headers.Add(TOKEN_NAME, TOKEN_VALUE);
                //List with text identifiers:
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
    }
}
