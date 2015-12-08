using OpenBible.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data.YouVersion
{
	public class YouVersionTextProvider : WebTextProvider
	{
		public const int REQUEST_RETRIES = 3;

		private static async Task<YouVersionApiResponse> MakeApiRequest (string chapterCode)
		{
			string rawResponse = await MakeWebRequest("http://bible.youversionapi.com/3.0/chapter.json?id=59&reference=" + chapterCode);
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(YouVersionApiResponse));
			MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(@rawResponse));
			YouVersionApiResponse response = (YouVersionApiResponse)serializer.ReadObject(ms);
			return response;
		}

		public async override Task<ChapterViewModel> GetChapter (string chapterCode)
		{
			int retryCounter = 0;
			YouVersionApiResponse response = null;

			while (retryCounter < REQUEST_RETRIES && (response == null || response.response.code != 200))
			{
				response = await MakeApiRequest(chapterCode);
			}

			if (response.response.code != 200)
			{
				throw new System.Exception(string.Format("API Response Failure: {0}", response.response.code));
			}

			ChapterViewModel chapter = YouVersionChapterParser.ParseChapter(response.response.data.content);

			chapter.ChapterCode = chapterCode;
			chapter.NextChapterCode = response.response.data.next.usfm;
			chapter.PreviousChapterCode = response.response.data.previous.usfm;

			var referenceTokens = response.response.data.reference.human.Split(' ');
			var nameTokens = referenceTokens.Take(referenceTokens.Length - 1);
			chapter.BookName = string.Join(" ", nameTokens);

			return chapter;
		}

		public override Task<List<VerseViewModel>> Search (SearchParameters searchParameters)
		{
			throw new NotImplementedException();
		}
	}
}
