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

            ParseElement((XmlElement)(document.GetElementsByTagName("body")[0]), chapter, Style.NORMAL);

            return chapter;
        }

        private static void ParseElement(XmlElement element, Chapter chapter, Style currentTextStyle)
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
                                ParseElement(e, chapter, currentTextStyle);
                            }
                            if (e.GetAttribute("class") == "s1")
                            {
                                // New heading section
                                chapter.Sections.Add(new Section());
                                ParseElement(e, chapter, currentTextStyle);
                            }
                            else
                            {
                                ParseElement(e, chapter, currentTextStyle);
                            }
                            break;
                        case "SPAN":
                            if (e.GetAttribute("class") == "heading" && element.GetAttribute("class").Contains("s1")) //(e.GetAttribute("class") == "heading")
                            {
                                chapter.Sections.Last().Heading.Add(new TextSpan(e.InnerText, currentTextStyle));
                            }
                            else if (e.GetAttribute("class") == "label" && element.GetAttribute("class").Contains("verse"))
                            {
                                Verse verse = new Verse();
                                verse.Number = int.Parse(e.InnerText);
                                chapter.Verses.Add(verse);
                                if (chapter.Sections.Count == 0)
                                {
                                    chapter.Sections.Add(new Section());
                                }
                                chapter.Sections.Last().Verses.Add(verse);
                            }
                            else if (e.GetAttribute("class") == "content" && element.GetAttribute("class").Contains("verse"))
                            {
                                chapter.Verses.Last().TextSpans.Add(new TextSpan(e.InnerText, currentTextStyle));
                            }
                            else if (e.GetAttribute("class") == "content" && element.GetAttribute("class").Contains("nd"))
                            {
                                chapter.Verses.Last().TextSpans.Add(new TextSpan(e.InnerText, Style.NAME_OF_GOD));
                            }
                            else if (e.GetAttribute("class") == "heading" && element.GetAttribute("class").Contains("nd"))
                            {
                                //chapter.Sections.Last().Heading.Add(new TextSpan(e.InnerText, Style.NAME_OF_GOD));
                            }
                            else if (element.GetAttribute("class") == "q1" && e.GetAttribute("class").Contains("verse"))
                            {
                                ParseElement(e, chapter, Style.QUOTE1);
                            }
                            else if (element.GetAttribute("class") == "q2" && e.GetAttribute("class").Contains("verse"))
                            {
                                ParseElement(e, chapter, Style.QUOTE2);
                            }
                            else if (e.GetAttribute("class") == "wj" && element.GetAttribute("class").Contains("verse"))
                            {
                                chapter.Verses.Last().TextSpans.Add(new TextSpan(e.InnerText, Style.WORDS_OF_JESUS));
                            }
                            else
                            {
                                ParseElement(e, chapter, currentTextStyle);
                            }
                            break;
                    }
                }
            }
        }
    }
}
