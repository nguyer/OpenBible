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
                Paragraph paragraph = new Paragraph();
                if (section.Heading != null)
                {
                    Paragraph header = new Paragraph();
                    header.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe UI");
                    header.FontSize = 20;
                    header.Margin = new Thickness(0, 10, 0, 5);
                    Run run = new Run();
                    run.Text = section.Heading;
                    header.Inlines.Add(run);
                    blocks.Add(header);
                }
                if (section.Verses.Count > 0)
                {
                    Paragraph verses = new Paragraph();
                    verses.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Cambria");
                    verses.FontSize = 16;
                    verses.LineHeight = 25;
                    foreach (Verse verse in section.Verses)
                    {
                        Run verseLabel = new Run();
                        verseLabel.Text = verse.Number.ToString();
                        verseLabel.FontSize = 8;
                        verses.Inlines.Add(verseLabel);

                        foreach (TextSpan textSpan in verse.TextSpans)
                        {
                            Run spanRun = new Run();
                            if ((textSpan.GetType() == typeof(WordsOfJesus)))
                            {
                                spanRun.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 200, 0, 0));
                            }
                            spanRun.Text = textSpan.Text;
                            verses.Inlines.Add(spanRun);
                        }
                    }
                    blocks.Add(verses);
                }
                blocks.Add(paragraph);
            }
            return blocks;
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
