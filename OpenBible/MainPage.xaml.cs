using OpenBible.Data;
using OpenBible.ViewModel;
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
        ChapterViewModel chapterViewModel;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            chapterViewModel = new ChapterViewModel(new Chapter(), pageFlipView);
            DataContext = chapterViewModel;
            //Window.Current.SizeChanged += new WindowSizeChangedEventHandler(sizeChangedHandler);
        }

        private void sizeChangedHandler(object sender, WindowSizeChangedEventArgs e)
        {
            chapterViewModel.updateContent();
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Previous_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Browse_Click(object sender, RoutedEventArgs e)
        {
            chapterViewModel.ChangeChapter(TextBoxChapterCode.Text);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void NavMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex != -1)
            {
                switch (listBox.SelectedIndex)
                {
                    case 0:
                        // Hamburger Button
                        SideMenu.IsPaneOpen = !SideMenu.IsPaneOpen;
                        listBox.SelectedIndex = -1;
                        return;
                    case 2:
                        // Browse
                        chapterViewModel.ChangeChapter(TextBoxChapterCode.Text);
                        break;
                    case 5:
                        // Next Chapter
                        if (chapterViewModel.Chapter.NextChapterCode != null)
                        {
                            chapterViewModel.ChangeChapter(chapterViewModel.Chapter.NextChapterCode);
                            SideMenu.IsPaneOpen = false;
                        }
                        break;
                    case 6:
                        // Previous Chapter
                        if (chapterViewModel.Chapter.PreviousChapterCode != null)
                        {
                            chapterViewModel.ChangeChapter(chapterViewModel.Chapter.PreviousChapterCode);
                            SideMenu.IsPaneOpen = false;
                        }
                        break;
                }
                listBox.SelectedIndex = -1;
            }
        }
    }
}
