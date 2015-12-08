using OpenBible.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data
{
	public abstract class WebTextProvider : ITextProvider
	{
		protected static async Task<string> MakeWebRequest (string url)
		{
			HttpClient http = new HttpClient();
			HttpResponseMessage response = await http.GetAsync(url);
			return await response.Content.ReadAsStringAsync();
		}

		public abstract Task<ChapterViewModel> GetChapter (string chapterCode);
		public abstract Task<List<VerseViewModel>> Search (SearchParameters searchParameters);
	}
}
