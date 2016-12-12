using OpenBible.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Documents;

namespace OpenBible
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        public MainPageViewModel()
        {
           
        }

        public string Html
        {
            get;
            set;
        }

        public BlockCollection RtfText
        {
            get;
            set;
        }


        public string ChapterName
        {
            get;
            set;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private CoreDispatcher _dispatcher = null;
        private async Task UIThreadAction(Action act)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => act.Invoke());
        }
        private async void PropertyChangedAsync(string property)
        {
            if (PropertyChanged != null)
                await UIThreadAction(() => PropertyChanged(this, new PropertyChangedEventArgs(property)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
