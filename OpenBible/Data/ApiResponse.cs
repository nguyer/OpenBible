using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data
{
    public class ApiResponse
    {
        public int reader_chapter
        {
            get;
            set;
        }

        public string reader_version
        {
            get;
            set;
        }

        private string _reader_html;
        public string reader_html
        {
            get { return _reader_html; }
            set
            {
                _reader_html = "<body>" + value + "</body>";
            }
        }

        public string to_path
        {
            get;
            set;
        }

        public ChapterHash previous_chapter_hash
        {
            get;
            set;
        }

        public ChapterHash next_chapter_hash
        {
            get;
            set;
        }

        public ReaderAudio reader_audio
        {
            get;
            set;
        }

        public string reader_book
        {
            get;
            set;
        }

        public string human
        {
            get;
            set;
        }

        public class ChapterHash
        {
            public bool toc
            {
                get;
                set;
            }
            public bool canonical
            {
                get;
                set;
            }
            public int version_id
            {
                get;
                set;
            }
            public string human
            {
                get;
                set;
            }
            public List<string> usfm
            {
                get;
                set;
            }
        }

        public class ReaderAudio
        {
            public Dictionary<string, string> download_urls
            {
                get;
                set;
            }
            public bool timing_available
            {
                get;
                set;
            }
            public int id
            {
                get;
                set;
            }
            public int version_id
            {
                get;
                set;
            }
            public string title
            {
                get;
                set;
            }
            public Copyright copyright_long
            {
                get;
                set;
            }
            public Copyright copyright_short
            {
                get;
                set;
            }
            public string publisher_link
            {
                get;
                set;
            }
            public string url
            {
                get;
                set;
            }

            public class Copyright
            {
                public string text
                {
                    get;
                    set;
                }
                public string html
                {
                    get;
                    set;
                }
            }
        }
    }
}
