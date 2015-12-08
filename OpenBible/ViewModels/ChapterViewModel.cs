using OpenBible.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBible.ViewModels
{
    public class ChapterViewModel : ViewModelBase
    {
		private ObservableCollection<SectionViewModel> sections
			= new ObservableCollection<SectionViewModel>();

		public ObservableCollection<SectionViewModel> Sections
		{
			get
			{
				return sections;
			}

			set
			{
				sections = value;
				RaisePropertyChanged("Sections");
			}
		}

		private string chapterCode;

		public string ChapterCode
		{
			get
			{
				return chapterCode;
			}

			set
			{
				chapterCode = value;
				RaisePropertyChanged("ChapterCode");
			}
		}

		private string nextChapterCode;

		public string NextChapterCode
		{
			get
			{
				return nextChapterCode;
			}

			set
			{
				nextChapterCode = value;
				RaisePropertyChanged("NextChapterCode");
			}
		}


		private string previousChapterCode;

		public string PreviousChapterCode
		{
			get
			{
				return previousChapterCode;
			}

			set
			{
				previousChapterCode = value;
				RaisePropertyChanged("PreviousChapterCode");
			}
		}

		private string bookName;

		public string BookName
		{
			get { return bookName; }
			set
			{
				bookName = value;
				RaisePropertyChanged("BookName"); 
			}
		}

		private int number;

		public int Number
		{
			get { return number; }
			set
			{
				number = value;
				RaisePropertyChanged("Number");
			}
		}


		public ChapterViewModel()
        {
        }
    }
}
