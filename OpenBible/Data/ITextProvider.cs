using OpenBible.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data
{
	public interface ITextProvider
	{
		Task<ChapterViewModel> GetChapter (string chapterCode);
		Task<List<VerseViewModel>> Search (SearchParameters searchParameters);
	}
}
