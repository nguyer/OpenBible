using OpenBible.Data;
using OpenBible.Data.YouVersion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Documents;

namespace OpenBible.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
			// TODO: select text provider from stored user preference
			textProvider = new YouVersionTextProvider();
			LoadChapter("ROM.1");
        }

		private ITextProvider textProvider;

		private ChapterViewModel activeChapter;
		public ChapterViewModel ActiveChapter
		{
			get
			{
				return activeChapter;
			}

			set
			{
				activeChapter = value;
				RaisePropertyChanged("ActiveChapter");
			}
		}

		public string ActiveChapterCode
		{
			set
			{
				LoadChapter(value);
			}
		}

		protected async void LoadChapter (string chapterCode) {
			ActiveChapter = await textProvider.GetChapter(chapterCode);
		}
    }
}
