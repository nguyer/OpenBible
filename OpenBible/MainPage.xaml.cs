using OpenBible.Data;
using OpenBible.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OpenBible
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainPageViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel = new MainPageViewModel();
            DataContext = viewModel;
            //Window.Current.SizeChanged += new WindowSizeChangedEventHandler(sizeChangedHandler);
        }

        private void sizeChangedHandler(object sender, WindowSizeChangedEventArgs e)
        {

        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
			viewModel.ActiveChapterCode = viewModel.ActiveChapter.NextChapterCode;
        }

        private void Button_Previous_Click(object sender, RoutedEventArgs e)
        {
			viewModel.ActiveChapterCode = viewModel.ActiveChapter.PreviousChapterCode;
        }

        private void Button_Browse_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
