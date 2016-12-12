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
            Heading = new List<TextSpan>();
            Verses = new List<Verse>();
        }

        public Section(List<TextSpan> heading)
        {
            Heading = heading;
            Verses = new List<Verse>();
        }

        public Section(List<TextSpan> heading, List<Verse> verses)
        {
            Heading = heading;
            Verses = verses;
        }

        public List<TextSpan> Heading
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
