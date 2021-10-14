using Microsoft.Extensions.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TMG1DotNetCoreWPF.DTO;

namespace TMG1DotNetCoreWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TextParser : Window
    {
        private static readonly Web _web = BuildWeb();
        private readonly Parser _parser;
        private readonly TextHandler _textHandler;

        public TextParser()
        {
            InitializeComponent();
            Button c = LayoutRoot.Children.OfType<Button>().First(i => i.Name == "submitButton");
            c.Click += Button_Click;
            _parser = new();
            _textHandler = new();
        }

        private static Web BuildWeb()
        {
            IConfiguration _config = new ConfigurationBuilder()
                //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                //.AddJsonFile("secrets.json")
                .AddUserSecrets<App>()
                .Build();
            return new Web(_config["TokenName"], _config["TokenValue"], _config["Url"]);
        }

        /// <summary>
        /// Handles the submit button
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ids = _textHandler.GetIdListFrom(textBox);
            ObservableCollection<Line> lines = new();
            foreach(string oneString in _web.GetDataFromServer(ids))
            {
                string text = _parser.ConvertJson(oneString);
                lines.Add(new Line()
                {
                    Text = text,
                    Words = _parser.CountWords(text),
                    Vowels = _parser.CountVowels(text)
                });
            }
            lineList.ItemsSource = lines;
        }

        /// <summary>
        /// Handles an input text field
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        private void TextBox_TextPreview(object sender, TextCompositionEventArgs e)
        {
            _textHandler.RecreateTextData(e, textBox);
        }

        [Obsolete]
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Empty
        }
    }
}