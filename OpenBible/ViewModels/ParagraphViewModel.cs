using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.ViewModels
{
	public class ParagraphViewModel : ViewModelBase
	{
		private ObservableCollection<VerseViewModel> verses
					= new ObservableCollection<VerseViewModel>();

		public ObservableCollection<VerseViewModel> Verses
		{
			get
			{
				return verses;
			}

			set
			{
				verses = value;
				RaisePropertyChanged("Verses");
			}
		}

		private int indentLevel = 0;

		public int IndentLevel
		{
			get { return indentLevel; }
			set
			{
				indentLevel = value;
				RaisePropertyChanged("IndentLevel");
			}
		}
	}
}
