using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data.YouVersion
{
    public class YouVersionApiResponse
    {
        public Response response
        {
            get;
            set;
        }

        public class Response
        {
            public int code
            {
                get;
                set;
            }
            public Data data
            {
                get;
                set;
            }

            public String buildtime
            {
                get;
                set;
            }

            public class Data
            {
                public Reference reference
                {
                    get;
                    set;
                }

                public Copyright copyright
                {
                    get;
                    set;
                }

                public string content
                {
					get;
					set;
                }
                public List<Audio> audio
                {
                    get;
                    set;
                }
                public Link previous
                {
                    get;
                    set;
                }
                public Link next
                {
                    get;
                    set;
                }

                public class Link
                {
                    public bool toc
                    {
                        get;
                        set;
                    }
                    public string usfm
                    {
                        get;
                        set;
                    }
                    public string human
                    {
                        get;
                        set;
                    }
                    public bool canonical
                    {
                        get;
                        set;
                    }
                }

                public class Audio
                {
                    public Dictionary<String, String> download_urls
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
                }

                public class Reference
                {
                    public string human
                    {
                        get;
                        set;
                    }
                    public string usfm
                    {
                        get;
                        set;
                    }
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
}
