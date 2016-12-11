using OpenBible.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace OpenBible.ViewModel
{
    class ChapterViewModel
    {
        private FlipView flipView;
        private Chapter chapter;

        private SizeChangedEventHandler sizeChangedEventHandler;
        private RoutedEventHandler loadedEventHandler;

        public Chapter Chapter
        {
            get
            {
                return chapter;
            }
            set
            {
                chapter = value;
                updateContent();
            }
        }

        public RichTextBlock TextBlock
        {
            get;
            set;
        }

        public List<RichTextBlockOverflow> Overflows
        {
            get;
            set;
        }

        public ChapterViewModel(Chapter chapter, FlipView flipView)
        {
            sizeChangedEventHandler = new SizeChangedEventHandler(textBlockResized);
            loadedEventHandler = new RoutedEventHandler(checkOverflow);
            this.flipView = flipView;
            this.chapter = chapter;
            this.Overflows = new List<RichTextBlockOverflow>();
            //Html = "<body></body>".Replace("\n", "");

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string currentChapter = (string)localSettings.Values["currentChapter"];
            if (currentChapter == null || currentChapter == "")
            {
                currentChapter = "jhn.1.esv";
            }
            ChangeChapter(currentChapter);
        }

        public async void ChangeChapter(string chapterCode)
        {
            try
            {
                this.chapter = await ChapterProvider.GetChapter(chapterCode);
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["currentChapter"] = chapterCode;
                updateContent();
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog("Unable to load chapter. Check your internet connection?");
                await dialog.ShowAsync();
            }
        }

        public void updateContent()
        {
            flipView.Items.Clear();
            Overflows.Clear();
            TextBlock = new RichTextBlock();
            TextBlock.Margin = new Thickness(50);
            TextBlock.Loaded += new RoutedEventHandler(textBlockLoaded);
            foreach (Block block in Chapter.GetBlocks())
            {
                TextBlock.Blocks.Add(block);
            }
            flipView.Items.Add(TextBlock);
        }

        private void textBlockLoaded(object sender, RoutedEventArgs e)
        {
            checkTextBlock(sender, null);
            TextBlock.SizeChanged -= sizeChangedEventHandler;
            TextBlock.SizeChanged += sizeChangedEventHandler;
        }

        private void textBlockResized(object sender, SizeChangedEventArgs e)
        {
            checkTextBlock(sender, null);
        }

        private void checkTextBlock(object sender, EventArgs e)
        {
            RichTextBlock textBlock = (RichTextBlock)sender;
            if (textBlock.HasOverflowContent)
            {
                if (Overflows.Count == 0)
                {
                    // If there is no existing overflow columns, create the first one
                    RichTextBlockOverflow overflow = new RichTextBlockOverflow();
                    overflow.Margin = new Thickness(50);
                    textBlock.OverflowContentTarget = overflow;
                    Overflows.Add(overflow);
                    flipView.Items.Add(overflow);
                    overflow.Loaded -= loadedEventHandler;
                    overflow.Loaded += loadedEventHandler;
                }
                else
                {
                    for (int i = 0; i < Overflows.Count; i++)
                    {
                        if (Overflows[i].HasOverflowContent && i + 1 == Overflows.Count)
                        {
                            // We still have content but we're on the last overflow - create a new one
                            RichTextBlockOverflow overflow = new RichTextBlockOverflow();
                            overflow.Margin = new Thickness(50);
                            overflow.Loaded -= loadedEventHandler;
                            overflow.Loaded += loadedEventHandler;
                            Overflows[i].OverflowContentTarget = overflow;
                            Overflows.Add(overflow);
                            flipView.Items.Add(overflow);
                            break;
                        }
                        else if (!Overflows[i].HasOverflowContent && i + 1 != Overflows.Count)
                        {
                            // There are more overflows but we ran out of content! - remove the extra ones
                            if (flipView.SelectedIndex > i)
                            {
                                flipView.SelectedIndex = i;
                            }
                                
                            while(Overflows.Count > i)
                            {
                                flipView.Items.Remove(flipView.Items.Last());
                                Overflows.Remove(Overflows.Last());
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                // We don't need extra columns
                Overflows.Clear();
                for (int i = 1; i < flipView.Items.Count; i++)
                {
                    flipView.Items.RemoveAt(i);
                }
            }
            
        }

        private void checkOverflow(object sender, RoutedEventArgs e)
        {
            RichTextBlockOverflow textBlock = (RichTextBlockOverflow)sender;
            if (textBlock.HasOverflowContent)
            {
                RichTextBlockOverflow overflow = new RichTextBlockOverflow();
                overflow.Margin = new Thickness(50);
                overflow.Loaded -= loadedEventHandler;
                overflow.Loaded += loadedEventHandler;
                textBlock.OverflowContentTarget = overflow;
                Overflows.Add(overflow);
                flipView.Items.Add(overflow);
            }
        }
    }
}
