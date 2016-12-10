using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data
{
    public class Verse
    {
        public Verse()
        {
            this.TextSpans = new List<TextSpan>();
        }

        public Verse(int number, List<TextSpan> textSpans)
        {
            this.Number = number;
            this.TextSpans = textSpans;
        }

        public int Number
        {
            get;
            set;
        }

        public List<TextSpan> TextSpans
        {
            get;
            set;
        }
    }
}
