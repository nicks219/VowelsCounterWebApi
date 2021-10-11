﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace TMG1DotNetCoreWPF
{
    internal class TextHandler
    {
        private HashSet<int> _idsForRequest;
        private const int MIN_INDEX = 1;
        private const int MAX_INDEX = 20;

        /// <summary>
        /// Checks the data and leaves only the correct entries
        /// </summary>
        /// <param name="e">Event type</param>
        /// <param name="textBox">XAML type</param>
        internal void RecreateTextData(TextCompositionEventArgs e, TextBox textBox)
        {
            e.Handled = !" ,;0123456789".Contains(e.Text, StringComparison.InvariantCulture);
            //Correction after entering a comma or semicolon
            if (",;".Contains(e.Text, StringComparison.InvariantCulture))
            {
                InitializeIdsFrom(textBox);
                CreateTextFromIds();
                e.Handled = true;
            }

            void CreateTextFromIds()
            {
                StringBuilder result = new();
                foreach (int r in _idsForRequest)
                {
                    result.Append(r);
                    result.Append(", ");
                }
                textBox.Text = result.ToString();
                textBox.Focus();
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        /// <summary>
        /// Creates valid string identifiers from an input text field
        /// </summary>
        /// <param name="textBox">XAML type</param>
        /// <returns>Collection with text Ids</returns>
        internal HashSet<int> GetIdListFrom(TextBox textBox)
        {
            InitializeIdsFrom(textBox);
            return _idsForRequest;
        }

        // Reads and validates string identifiers from an input text field
        private void InitializeIdsFrom(TextBox textBox)
        {
            string[] text = textBox.Text.Split(',', ';');
            _idsForRequest = text
                .Select(str => (isNumber: int.TryParse(str, out int number), id: number))
                .Where(data => data.isNumber && data.id is >= MIN_INDEX and <= MAX_INDEX)
                .Select(data => data.id)
                .ToHashSet();
        }
    }
}