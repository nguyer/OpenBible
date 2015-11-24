using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Documents;

namespace OpenBible.Data
{
    public class ChapterProvider
    {
        private static async Task<string> MakeWebRequest(string url)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<ApiResponse> MakeApiRequest(string chapterCode)
        {
            string rawResponse = await MakeWebRequest("http://bible.youversionapi.com/3.0/chapter.json?id=59&reference=" + chapterCode);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ApiResponse));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(@rawResponse));
            ApiResponse response = (ApiResponse)serializer.ReadObject(ms);
            return response;
        }

        public static async Task<string> GetChapterText(string chapterCode)
        {
            return (await MakeApiRequest(chapterCode)).response.data.content;
        }

        public static async Task<Chapter> GetChapter(string chapterCode)
        {
            ApiResponse response = await MakeApiRequest(chapterCode);
            Chapter chapter = ChapterParser.ParseChapter(response.response.data.content);
            chapter.ChapterCode = chapterCode;
            chapter.NextChapterCode = response.response.data.next.usfm;
            chapter.PreviousChapterCode = response.response.data.previous.usfm;
            return chapter;
        }

        public static async Task<BlockCollection> GetChapterRtf(string chapterCode)
        {
            return NewHtmlToRtfConverter.ConvertHtmlToRtf(await GetChapterText(chapterCode));
        }
    }
}
