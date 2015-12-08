using System.Linq;
using System.Xml.Linq;
using OpenBible.ViewModels;

namespace OpenBible.Data.YouVersion
{
    class YouVersionChapterParser
    {
        public static ChapterViewModel ParseChapter(string html)
        {
			if (html == null)
			{
				throw new System.ArgumentException("HTML to be parsed must not be null");
			}

            ChapterViewModel chapter = new ChapterViewModel();
			XElement document = XElement.Parse(html);

            ParseElement(document, chapter);

            return chapter;
        }

        private static void ParseElement(XElement element, ChapterViewModel chapter)
        {
			// basic info
			var chapterElement = (from item in element.Descendants()
								  where ((string)item.Attribute("class")).Contains("chapter")
								  select item).FirstOrDefault();

			chapter.Number = int.Parse(
				(from item in chapterElement.Elements()
				where ((string)item.Attribute("class")).Contains("label")
				select item.Value).FirstOrDefault()
				);

			// TODO: I don't see this in the output
			chapter.BookName = "DEFAULT";

			var textElements = from item in chapterElement.Descendants()
							   where (
									((string)item.Attribute("class")).Contains("p") ||
									((string)item.Attribute("class")).Contains("s") ||
									((string)item.Attribute("class")).Contains("q")
								)
							   select item;

			SectionViewModel parsedSection = null;

			foreach (XElement textElement in textElements)
			{
				var className = (string)textElement.Attribute("class");

				switch (className)
				{
					case "s1":
						if (parsedSection != null)
						{
							chapter.Sections.Add(parsedSection);
						}

						parsedSection = new SectionViewModel();
						parsedSection.Title = textElement.Value;
						break;
					case "p":
						if (parsedSection == null)
						{
							// should this maybe throw an error? Idk
							parsedSection = new SectionViewModel();
							parsedSection.Title = "";
						}

						ParagraphViewModel parsedParagraph = new ParagraphViewModel();

						PopulateParagraphFromElement(parsedParagraph, textElement);
						
						parsedSection.Paragraphs.Add(parsedParagraph);
						break;
					case "q1": // poetry
						if (parsedSection == null)
						{
							parsedSection = new SectionViewModel();
							parsedSection.Title = "";
						}

						ParagraphViewModel parsedPoetry = new ParagraphViewModel();
						parsedPoetry.IndentLevel = 1;

						PopulateParagraphFromElement(parsedPoetry, textElement);

						parsedSection.Paragraphs.Add(parsedPoetry);
						break;
					case "q2": // poetry level 2
						if (parsedSection == null)
						{
							parsedSection = new SectionViewModel();
							parsedSection.Title = "";
						}

						ParagraphViewModel parsedIndentedPoetry = new ParagraphViewModel();
						parsedIndentedPoetry.IndentLevel = 2;

						PopulateParagraphFromElement(parsedIndentedPoetry, textElement);

						parsedSection.Paragraphs.Add(parsedIndentedPoetry);
						break;
				}
			}

			chapter.Sections.Add(parsedSection);
        }

		private static void PopulateParagraphFromElement (ParagraphViewModel paragraph, XElement element)
		{
			var verseElements = from item in element.Elements("span")
								where ((string)item.Attribute("class")).Contains("verse")
								select item;

			foreach (XElement verseElement in verseElements)
			{
				if (verseElement.Elements().Count() == 0)
				{
					// corrupted?
					continue;
				}

				VerseViewModel parsedVerse = new VerseViewModel();

				var verseLabel = (from item in verseElement.Elements()
								  where ((string)item.Attribute("class")).Contains("label")
								  select item.Value).FirstOrDefault();
				var verseText = (from item in verseElement.Elements()
								 where ((string)item.Attribute("class")).Contains("content")
								 select item.Value).FirstOrDefault();

				if (verseLabel != null)
				{
					parsedVerse.Number = int.Parse(verseLabel);
				}

				if (verseText != null)
				{
					parsedVerse.Text = verseText;
				}

				if (parsedVerse.Text.Trim().Length == 0)
				{
					// no empty verses please
					continue;
				}

				paragraph.Verses.Add(parsedVerse);
			}
		}
    }
}
