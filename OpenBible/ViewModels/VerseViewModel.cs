using OpenBible.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.ViewModels
{
	public class VerseViewModel : ViewModelBase
	{
		private int? number = null;

		public int? Number
		{
			get
			{
				return number;
			}

			set
			{
				number = value;
				RaisePropertyChanged("Number");
			}
		}

		private string text = "Default verse text";

		public string Text
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
				RaisePropertyChanged("Text");
			}
		}

		public VerseViewModel ()
		{
			
		}
	}
}
