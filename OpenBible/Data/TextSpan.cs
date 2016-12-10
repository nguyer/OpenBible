using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.Data
{
    public class TextSpan
    {
        public TextSpan()
        {

        }

        public TextSpan(string text)
        {
            this.Text = text;
        }

        public string Text
        {
            get;
            set;
        }
    }
}
