using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
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

        public static async Task<ChapterApiResponse> MakeApiRequest(string chapterCode)
        {
            string json;
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localCacheFolder = applicationData.LocalCacheFolder;

            StorageFile cacheFile = (StorageFile) await localCacheFolder.TryGetItemAsync(chapterCode + ".json");
            if (cacheFile != null)
            {
                json = await FileIO.ReadTextAsync(cacheFile);
            }
            else
            {
                string requestUrl = "https://www.bible.com/bible/59/" + chapterCode + ".json";
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    }
                    json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    StorageFile sampleFile = await localCacheFolder.CreateFileAsync(chapterCode + ".json");
                    await FileIO.WriteTextAsync(sampleFile, json);

                }
            }
            return JsonConvert.DeserializeObject<ChapterApiResponse>(json); ;
        }

        public static async Task<string> GetChapterText(string chapterCode)
        {
            return (await MakeApiRequest(chapterCode)).reader_html;
        }

        public static async Task<Chapter> GetChapter(string chapterCode)
        {
            ChapterApiResponse response = await MakeApiRequest(chapterCode);
            Chapter chapter = ChapterParser.ParseChapter(response.reader_html);
            chapter.BookName = response.reader_book;
            chapter.Number = response.reader_chapter;
            chapter.ChapterCode = chapterCode;

            if (response.next_chapter_hash != null && response.next_chapter_hash.usfm.Count > 0)
            {
                chapter.NextChapterCode = response.next_chapter_hash.usfm[0];
            }
            if (response.previous_chapter_hash != null && response.previous_chapter_hash.usfm.Count > 0)
            {
                chapter.PreviousChapterCode = response.previous_chapter_hash.usfm[0];
            }
            return chapter;
        }

        public static async Task<BlockCollection> GetChapterRtf(string chapterCode)
        {
            return NewHtmlToRtfConverter.ConvertHtmlToRtf(await GetChapterText(chapterCode));
        }
    }
}
