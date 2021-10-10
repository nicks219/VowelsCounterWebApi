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
    public partial class TheMostGame : Window
    {
        private readonly Parser _parser;

        public TheMostGame()
        {
            InitializeComponent();
            Button c = LayoutRoot.Children.OfType<Button>().First(i => i.Name == "sendButton");
            c.Click += Button_Click;
            _parser = new();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _parser.GrubIdFrom(textBox);
            ObservableCollection<Line> lines = new ObservableCollection<Line>();
            foreach(string json in _parser.GetDataFromServer())
            {
                string text = _parser.ParseJson(json);
                lines.Add(new Line()
                {
                    Text = text,
                    Words = _parser.CountWords(text),
                    Vowels = _parser.CountVowels(text)
                });
            }
            lineList.ItemsSource = lines;
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