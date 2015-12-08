using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace OpenBible.ViewModels
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		protected void RaisePropertyChanged (string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private CoreDispatcher _dispatcher = null;
		protected async Task UIThreadAction (Action act)
		{
			await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => act.Invoke());
		}
		protected async void PropertyChangedAsync (string property)
		{
			if (PropertyChanged != null)
				await UIThreadAction(() => PropertyChanged(this, new PropertyChangedEventArgs(property)));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
