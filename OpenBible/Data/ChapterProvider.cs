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
        private static StorageFolder localCacheFolder = Windows.Storage.ApplicationData.Current.LocalCacheFolder;

        private static async Task<string> MakeWebRequest(string url)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<ChapterApiResponse> GetChapterApiResponse(string chapterCode)
        {
            string json;
            if (await IsJsonCached(chapterCode))
            {
                json = await GetJsonFromCache(chapterCode);
                if (json == null || json == "")
                {
                    json = await GetJsonFromApi(chapterCode);
                    WriteJsonToCache(chapterCode, json);
                }
            }
            else
            {
                json = await GetJsonFromApi(chapterCode);
                WriteJsonToCache(chapterCode, json);
            }
            return JsonConvert.DeserializeObject<ChapterApiResponse>(json);
        }

        public static async Task<string> GetJsonFromApi(string chapterCode)
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
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }

        private static async Task<String> GetJsonFromCache(string chapterCode)
        {
            StorageFile file = (StorageFile)await localCacheFolder.TryGetItemAsync(chapterCode + ".json");
            return await FileIO.ReadTextAsync(file);
        }

        private static async void WriteJsonToCache(string chapterCode, string json)
        {
            StorageFile file = await localCacheFolder.CreateFileAsync(chapterCode + ".json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        private static async Task<bool> IsJsonCached(string chapterCode)
        {
            StorageFile file = (StorageFile) await localCacheFolder.TryGetItemAsync(chapterCode + ".json");
            return file != null;
        }

        public static async Task<Chapter> GetChapter(string chapterCode)
        {
            ChapterApiResponse response = await GetChapterApiResponse(chapterCode);
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
    }
}
