using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.ViewModels
{
	public class SectionViewModel : ViewModelBase
	{
		private ObservableCollection<ParagraphViewModel> paragraphs
				= new ObservableCollection<ParagraphViewModel>();

		public ObservableCollection<ParagraphViewModel> Paragraphs
		{
			get
			{
				return paragraphs;
			}

			set
			{
				paragraphs = value;
				RaisePropertyChanged("Paragraphs");
			}
		}

		private string title;

		public string Title
		{
			get { return title; }
			set 
			{ 
				title = value;
				RaisePropertyChanged("Title");
			}
		}

	}
}
