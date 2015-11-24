using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data
{
    public class Section
    {
        public Section()
        {
            Verses = new List<Verse>();
        }

        public Section(string heading)
        {
            Heading = heading;
            Verses = new List<Verse>();
        }

        public Section(string heading, List<Verse> verses)
        {
            Heading = heading;
            Verses = verses;
        }

        public string Heading
        {
            get;
            set;
        }

        public List<Verse> Verses
        {
            get;
            set;
        }
    }
}
