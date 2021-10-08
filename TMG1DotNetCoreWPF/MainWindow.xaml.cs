using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TMG1DotNetCoreWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TheMostGame : Window
    {
        private readonly Parser _parser;

        public TheMostGame()
        {
            InitializeComponent();
            Button c = LayoutRoot.Children.OfType<Button>().First(i => i.Name == "send");
            c.Click += Button_Click;
            _parser = new();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = string.Empty;
            words.Text = string.Empty;
            vowels.Text = string.Empty;
            _parser.GrubIdFrom(textBox);

            foreach (string json in _parser.GetDataFromServer())
            {
                string text = _parser.ParseJson(json);
                textBlock.Text += text;
                textBlock.Text += "\n\n";
                words.Text += _parser.CountWords(text).ToString() + "\n\n";
                vowels.Text += _parser.CountVowels(text).ToString() + "\n\n";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextPreview(object sender, TextCompositionEventArgs e)
        {
            _parser.CreateIdList(e, textBox);
        }
    }
}