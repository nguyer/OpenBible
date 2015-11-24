using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace OpenBible
{
    class BrokenHtmlToRtfConverter
    {
        public static string GetRtfText(DependencyObject obj)
        {
            return (string)obj.GetValue(RtfTextProperty);
        }

        public static void SetRtfText(DependencyObject obj, string value)
        {
            obj.SetValue(RtfTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for Html.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RtfTextProperty =
            DependencyProperty.RegisterAttached("RtfText", typeof(BlockCollection), typeof(HtmlToRtfConverter), new PropertyMetadata("", OnRtfTextChanged));

        private static void OnRtfTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            RichTextBlock parent = (RichTextBlock)sender;
            XmlDocument document = new XmlDocument();
            BlockCollection blocks = (BlockCollection)eventArgs.NewValue;
            foreach (Block block in blocks)
            {
                parent.Blocks.Add(block);
            }
        }

    }
}
