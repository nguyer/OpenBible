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

namespace OpenBible.Data
{
    class ChapterParser
    {
        public static Chapter ParseChapter(string html)
        {
            Chapter chapter = new Chapter();
            XmlDocument document = new XmlDocument();
            document.LoadXml(html);

            ParseElement((XmlElement)(document.GetElementsByTagName("body")[0]), chapter);

            return chapter;
        }

        private static void ParseElement(XmlElement element, Chapter chapter)
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

                    // Add a verse?

                    //parent.Add(new Run { Text = ((XmlText)child).InnerText });
                }
                else if (child is XmlElement)
                {
                    XmlElement e = (XmlElement)child;
                    switch (e.TagName.ToUpper())
                    {
                        case "DIV":
                            if (e.GetAttribute("class") == "p")
                            {
                                // Start of a new paragraph
                                chapter.Sections.Add(new Section());
                                ParseElement(e, chapter);

                            }
                            else if (e.GetAttribute("class") == "label" && element.GetAttribute("class").Contains("chapter"))
                            {
                                // Set chapter name
                                //chapter.BookName = e.InnerText;

                            }
                            else
                            {
                                ParseElement(e, chapter);
                            }
                            break;
                        case "SPAN":
                            //ParseElement(e, parent);

                            if (e.GetAttribute("class") == "heading")
                            {
                                chapter.Sections.Add(new Section(e.InnerText));
                            }
                            else if (e.GetAttribute("class") == "label" && element.GetAttribute("class").Contains("verse"))
                            {
                                Verse verse = new Verse();
                                verse.Number = int.Parse(e.InnerText);
                                chapter.Verses.Add(verse);
                                chapter.Sections.Last().Verses.Add(verse);
                            }
                            else if (e.GetAttribute("class") == "content" && element.GetAttribute("class").Contains("verse"))
                            {
                                if (chapter.Verses.Last().TextSpans == null || chapter.Verses.Last().TextSpans.Count == 0)
                                {
                                    chapter.Verses.Last().TextSpans.Add(new TextSpan(e.InnerText));
                                }
                            }
                            else if (e.GetAttribute("class") == "wj" && element.GetAttribute("class").Contains("verse"))
                            {
                                chapter.Verses.Last().TextSpans.Add(new WordsOfJesus(e.InnerText));
                            }
                            else
                            {
                                ParseElement(e, chapter);
                            }
                            //else
                            //{
                            //    Run run = new Run();
                            //    run.Text = e.InnerText;
                            //    parent.Add(run);
                            //}
                            break;
                        //case "P":
                        //    var p = new Paragraph();
                        //    parent.Add(p);
                        //    ParseElement(e, new ParagraphTextContainer(p));
                        //    break;
                        //case "STRONG":
                        //    var bold = new Bold();
                        //    parent.Add(bold);
                        //    ParseElement(e, new SpanTextContainer(bold));
                        //    break;
                        //case "U":
                        //    var underline = new Underline();
                        //    parent.Add(underline);
                        //    ParseElement(e, new SpanTextContainer(underline));
                        //    break;
                        //case "A":
                        //    ParseElement(e, parent);
                        //    break;
                        //case "BR":
                        //    parent.Add(new LineBreak());
                        //    break;
                    }
                }


            }
        }
    }
}
