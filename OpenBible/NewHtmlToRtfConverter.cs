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
    class NewHtmlToRtfConverter
    {
        public static BlockCollection ConvertHtmlToRtf(string html)
        {
            RichTextBlock parent = new RichTextBlock();
            XmlDocument document = new XmlDocument();
            document.LoadXml(html);

            ParseElement((XmlElement)(document.GetElementsByTagName("body")[0]), new RichTextBlockTextContainer(parent));

            return parent.Blocks;
        }

        private static void ParseElement(XmlElement element, ITextContainer parent)
        {
            foreach (var child in element.ChildNodes)
            {
                if (child is Windows.Data.Xml.Dom.XmlText)
                {
                    if (string.IsNullOrEmpty(child.InnerText) ||
                        child.InnerText == "\n")
                    {
                        continue;
                    }

                    parent.Add(new Run { Text = ((XmlText)child).InnerText });
                }
                else if (child is XmlElement)
                {
                    XmlElement e = (XmlElement)child;
                    switch (e.TagName.ToUpper())
                    {
                        case "DIV":
                            if (e.GetAttribute("class") == "p")
                            {
                                Paragraph paragraph = new Paragraph();
                                paragraph.Margin = new Thickness(0, 0, 0, 10);
                                //paragraph.LineHeight = 20;
                                parent.Add(paragraph);
                                ParseElement(e, new ParagraphTextContainer(paragraph));

                            }
                            else if (e.GetAttribute("class") == "label" && element.GetAttribute("class").Contains("chapter"))
                            {
                                Paragraph chapterLabel = new Paragraph();
                                chapterLabel.FontFamily = new FontFamily("Segoe UI");
                                chapterLabel.FontSize = 32;
                                chapterLabel.Inlines.Add(new Run { Text = "Chapter " + e.InnerText });
                                parent.Add(chapterLabel);

                            }
                            else
                            {
                                ParseElement(e, parent);
                            }
                            break;
                        case "SPAN":
                            //ParseElement(e, parent);

                            if (e.GetAttribute("class") == "heading")
                            {
                                Paragraph header = new Paragraph();
                                header.FontFamily = new FontFamily("Segoe UI");
                                header.FontSize = 20;
                                header.Margin = new Thickness(0, 10, 0, 5);
                                header.Inlines.Add(new Run { Text = e.InnerText });
                                parent.Add(header);
                            }
                            else if (e.GetAttribute("class") == "label" && element.GetAttribute("class").Contains("verse"))
                            {
                                Run run = new Run();
                                run.Text = e.InnerText;
                                run.FontSize = 8;
                                parent.Add(run);
                            }
                            else
                            {
                                ParseElement(e, parent);
                            }
                            //else
                            //{
                            //    Run run = new Run();
                            //    run.Text = e.InnerText;
                            //    parent.Add(run);
                            //}
                            break;
                        case "P":
                            var p = new Paragraph();
                            parent.Add(p);
                            ParseElement(e, new ParagraphTextContainer(p));
                            break;
                        case "STRONG":
                            var bold = new Bold();
                            parent.Add(bold);
                            ParseElement(e, new SpanTextContainer(bold));
                            break;
                        case "U":
                            var underline = new Underline();
                            parent.Add(underline);
                            ParseElement(e, new SpanTextContainer(underline));
                            break;
                        case "A":
                            ParseElement(e, parent);
                            break;
                        case "BR":
                            parent.Add(new LineBreak());
                            break;
                    }
                }


            }
        }

        private interface ITextContainer
        {
            void Add(Inline text);
            void Add(Block paragraph);
        }

        private sealed class SpanTextContainer : ITextContainer
        {
            private readonly Span _span;

            public SpanTextContainer(Span span)
            {
                _span = span;
            }

            public void Add(Inline text)
            {
                _span.Inlines.Add(text);
            }

            public void Add(Block paragraph)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class ParagraphTextContainer : ITextContainer
        {
            private readonly Paragraph _block;

            public ParagraphTextContainer(Paragraph block)
            {
                _block = block;
            }

            public void Add(Inline text)
            {
                _block.Inlines.Add(text);
            }

            public void Add(Block paragraph)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class RichTextBlockTextContainer : ITextContainer
        {
            private readonly RichTextBlock _richTextBlock;

            public RichTextBlockTextContainer(RichTextBlock richTextBlock)
            {
                _richTextBlock = richTextBlock;
            }

            public void Add(Inline text)
            {
                //throw new NotSupportedException();
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(text);

                _richTextBlock.Blocks.Add(paragraph);
            }

            public void Add(Block paragraph)
            {
                _richTextBlock.Blocks.Add(paragraph);
            }
        }
    }
}
