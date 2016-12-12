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

        public TextSpan(string text, Style style)
        {
            this.Text = text;
            this.Style = style;
        }

        public string Text
        {
            get;
            set;
        }

        public Style Style
        {
            get;
            set;
        }

        
    }

    public enum Style
    {
        NORMAL,
        WORDS_OF_JESUS,
        NAME_OF_GOD,
        QUOTE1,
        QUOTE2,
    }
}
