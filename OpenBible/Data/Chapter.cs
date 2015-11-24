using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

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

                        Run verseContent = new Run();
                        verseContent.Text = verse.Text;
                        verses.Inlines.Add(verseContent);
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
