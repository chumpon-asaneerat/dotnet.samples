#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

#endregion

namespace WpfHighlightTextBlockSample.Controls
{
    public static class TextBlockHighlighter
    {
        public static string GetSelection(DependencyObject obj)
        {
            return (string)obj.GetValue(SelectionProperty);
        }

        public static void SetSelection(DependencyObject obj, string value)
        {
            obj.SetValue(SelectionProperty, value);
        }

        public static readonly DependencyProperty SelectionProperty =
            DependencyProperty.RegisterAttached("Selection", typeof(string), typeof(TextBlockHighlighter),
                new PropertyMetadata(new PropertyChangedCallback(SelectText)));

        private static void SelectText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (!(d is TextBlock)) throw new InvalidOperationException("Only valid for TextBlock");

            TextBlock txtBlock = d as TextBlock;
            string text = txtBlock.Text;
            if (string.IsNullOrEmpty(text)) return;

            string highlightText = (string)d.GetValue(SelectionProperty);
            if (string.IsNullOrEmpty(highlightText)) return;

            int index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);
            if (index < 0) return;

            Brush selectionColor = (Brush)d.GetValue(HighlightColorProperty);
            Brush forecolor = (Brush)d.GetValue(ForecolorProperty);

            txtBlock.Inlines.Clear();
            while (true)
            {
                txtBlock.Inlines.AddRange(new Inline[] 
                {
                    new Run(text.Substring(0, index)),
                    new Run(text.Substring(index, highlightText.Length)) 
                    {
                        Background = selectionColor,
                        Foreground = forecolor
                    }
                });

                text = text.Substring(index + highlightText.Length);
                index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);

                if (index < 0)
                {
                    txtBlock.Inlines.Add(new Run(text));
                    break;
                }
            }
        }

        public static Brush GetHighlightColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HighlightColorProperty);
        }

        public static void SetHighlightColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(HighlightColorProperty, value);
        }

        public static readonly DependencyProperty HighlightColorProperty =
            DependencyProperty.RegisterAttached("HighlightColor", typeof(Brush), typeof(TextBlockHighlighter),
                new PropertyMetadata(Brushes.Yellow, new PropertyChangedCallback(SelectText)));


        public static Brush GetForecolor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForecolorProperty);
        }

        public static void SetForecolor(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForecolorProperty, value);
        }

        public static readonly DependencyProperty ForecolorProperty =
            DependencyProperty.RegisterAttached("Forecolor", typeof(Brush), typeof(TextBlockHighlighter),
                new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(SelectText)));

    }

    /*
    public static class HighlightTermBehavior
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(HighlightTermBehavior),
            new FrameworkPropertyMetadata("", OnTextChanged));

        public static string GetText(FrameworkElement frameworkElement) => (string)frameworkElement.GetValue(TextProperty);
        public static void SetText(FrameworkElement frameworkElement, string value) => frameworkElement.SetValue(TextProperty, value);


        public static readonly DependencyProperty TermToBeHighlightedProperty = DependencyProperty.RegisterAttached(
            "TermToBeHighlighted",
            typeof(string),
            typeof(HighlightTermBehavior),
            new FrameworkPropertyMetadata("", OnTextChanged));

        public static string GetTermToBeHighlighted(FrameworkElement frameworkElement)
        {
            return (string)frameworkElement.GetValue(TermToBeHighlightedProperty);
        }

        public static void SetTermToBeHighlighted(FrameworkElement frameworkElement, string value)
        {
            frameworkElement.SetValue(TermToBeHighlightedProperty, value);
        }


        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
                SetTextBlockTextAndHighlightTerm(textBlock, GetText(textBlock), GetTermToBeHighlighted(textBlock));
        }

        private static void SetTextBlockTextAndHighlightTerm(TextBlock textBlock, string text, string termToBeHighlighted)
        {
            textBlock.Text = string.Empty;

            if (TextIsEmpty(text))
                return;

            if (TextIsNotContainingTermToBeHighlighted(text, termToBeHighlighted))
            {
                AddPartToTextBlock(textBlock, text);
                return;
            }

            var textParts = SplitTextIntoTermAndNotTermParts(text, termToBeHighlighted);

            foreach (var textPart in textParts)
                AddPartToTextBlockAndHighlightIfNecessary(textBlock, termToBeHighlighted, textPart);
        }

        private static bool TextIsEmpty(string text)
        {
            return text.Length == 0;
        }

        private static bool TextIsNotContainingTermToBeHighlighted(string text, string termToBeHighlighted)
        {
            return string.Compare(text, termToBeHighlighted, true) != 0;
        }

        private static void AddPartToTextBlockAndHighlightIfNecessary(TextBlock textBlock, string termToBeHighlighted, 
            string textPart)
        {
            if (string.Compare(textPart, termToBeHighlighted, true) == 0)
                AddHighlightedPartToTextBlock(textBlock, textPart);
            else
                AddPartToTextBlock(textBlock, textPart);
        }

        private static void AddPartToTextBlock(TextBlock textBlock, string part)
        {
            textBlock.Inlines.Add(new Run { Text = part, FontWeight = FontWeights.Light });
        }

        private static void AddHighlightedPartToTextBlock(TextBlock textBlock, string part)
        {
            textBlock.Inlines.Add(new Run { Text = part, FontWeight = FontWeights.ExtraBold });
        }

        public static List<string> SplitTextIntoTermAndNotTermParts(string text, string term)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>() { string.Empty };

            return Regex.Split(text, $@"({Regex.Escape(term)})", RegexOptions.IgnoreCase)
                        .Where(p => p != string.Empty)
                        .ToList();
        }
    }
    */
}
