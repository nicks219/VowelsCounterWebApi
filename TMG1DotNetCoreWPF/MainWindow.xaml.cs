using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TMG1DotNetCoreWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TextParser : Window
    {
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

        /// <summary>
        /// Handles the submit button
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ids = _textHandler.GetIdListFrom(textBox);
            ObservableCollection<Line> lines = new();
            foreach(string json in Web.GetDataFromServer(ids))
            {
                string text = _parser.TrimJson(json);
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