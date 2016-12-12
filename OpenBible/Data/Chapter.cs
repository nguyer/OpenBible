using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace OpenBible.Data
{
    public class Chapter
    {

        public string ChapterCode
        {
            get;
            set;
        }

        public string NextChapterCode
        {
            get;
            set;
        }

        public string PreviousChapterCode
        {
            get;
            set;
        }

        public Chapter()
        {
            Verses = new List<Verse>();
            Sections = new List<Section>();
        }

        public Chapter(string bookName, int number)
        {
            BookName = bookName;
            Number = number;
            Verses = new List<Verse>();
            Sections = new List<Section>();
        }

        public List<Block> GetBlocks()
        {
            List<Block> blocks = new List<Block>();

            Paragraph chapterHeader = new Paragraph();
            chapterHeader.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe UI");
            chapterHeader.FontSize = 30;
            chapterHeader.Margin = new Thickness(0, 10, 0, 5);
            Run chapterHeaderRun = new Run();
            chapterHeaderRun.Text = this.BookName + " " + this.Number;
            chapterHeader.Inlines.Add(chapterHeaderRun);
            blocks.Add(chapterHeader);

            foreach (Section section in Sections)
            {
                Paragraph paragraph = getNewParagraph();
                blocks.Add(paragraph);
                if (section.Heading != null)
                {
                    Paragraph header = new Paragraph();
                    header.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe UI");
                    header.FontSize = 20;
                    header.Margin = new Thickness(0, 10, 0, 5);
                    foreach (TextSpan textSpan in section.Heading)
                    {
                        Run run = new Run();
                        run.Text = textSpan.Text;
                        if (textSpan.Style == Style.NAME_OF_GOD)
                        {
                            Typography.SetCapitals(run, FontCapitals.SmallCaps);
                        }
                        header.Inlines.Add(run);
                        blocks.Add(header);
                    }
                }
                if (section.Verses.Count > 0)
                {
                    foreach (Verse verse in section.Verses)
                    {
                        // Correctly align verse number with indented text
                        if (verse.TextSpans.First().Style == Style.QUOTE1)
                        {
                            paragraph = getNewParagraph();
                            blocks.Add(paragraph);
                            paragraph.TextIndent = 25;
                        }
                        else if (verse.TextSpans.First().Style == Style.QUOTE2)
                        {
                            paragraph = getNewParagraph();
                            blocks.Add(paragraph);
                            paragraph.TextIndent = 50;
                        }
                        Run verseLabel = new Run();
                        verseLabel.Text = verse.Number.ToString();
                        verseLabel.FontSize = 8;
                        paragraph.Inlines.Add(verseLabel);

                        foreach (TextSpan textSpan in verse.TextSpans)
                        {
                            Run spanRun = new Run();
                            if (textSpan.Style == Style.WORDS_OF_JESUS)
                            {
                                spanRun.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 200, 0, 0));
                            }
                            else if (textSpan.Style == Style.NAME_OF_GOD)
                            {
                                Typography.SetCapitals(spanRun, FontCapitals.SmallCaps);
                            }
                            else if (textSpan.Style == Style.QUOTE1 && !textSpan.Equals(verse.TextSpans.First()))
                            {
                                paragraph = getNewParagraph();
                                blocks.Add(paragraph);
                                paragraph.TextIndent = 25;
                            }
                            if (textSpan.Style == Style.QUOTE2)
                            {
                                paragraph = getNewParagraph();
                                blocks.Add(paragraph);
                                paragraph.TextIndent = 50;
                            }
                            spanRun.Text = textSpan.Text;
                            paragraph.Inlines.Add(spanRun);
                        }

                    }
                }
            }
            return blocks;
        }

        private Paragraph getNewParagraph()
        {
            Paragraph paragraph = new Paragraph();
            paragraph.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Cambria");
            paragraph.FontSize = 16;
            paragraph.LineHeight = 25;
            return paragraph;
        }

        public string BookName
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }

        public List<Verse> Verses
        {
            get;
            set;
        }

        public List<Section> Sections
        {
            get;
            set;
        }
    }
}
