using System;
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

        }

        public Verse(int number, string text)
        {
            this.Number = number;
            this.Text = text;
        }

        public int Number
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}
