using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenBible.Controls
{
	public class VerseWrapPanel : Panel
	{
		// using lessons learned from this article: http://www.codeproject.com/Articles/463860/WinRT-Custom-WrapPanel

		public double PanelWidth
		{
			get { return (double)GetValue(PanelWidthProperty); }
			set { SetValue(PanelWidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for PanelWidth.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PanelWidthProperty =
			DependencyProperty.Register("PanelWidth", typeof(double), typeof(VerseWrapPanel), new PropertyMetadata(100.0, OnPanelWidthPropertyChanged));

		private static void OnPanelWidthPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			(source as VerseWrapPanel).InvalidateMeasure();
		}

		/// <summary>
		/// Measures the size of all children, and returns the size this element would like to be.
		/// </summary>
		/// <param name="availableSize"></param>
		/// <returns></returns>
		protected override Size MeasureOverride (Size availableSize)
		{
			Size childAvailableSize = new Size(PanelWidth, double.PositiveInfinity);
			int columnCount = 0;
			if (Children.Count > 0)
			{
				columnCount = 1;
			}

			var remainingSpace = availableSize.Height;

			foreach (var child in Children)
			{
				child.Measure(childAvailableSize); // result is child.DesiredSize
				if (child.DesiredSize.Height > remainingSpace)
				{
					if (remainingSpace != availableSize.Height)
					{
						remainingSpace = availableSize.Height;
						columnCount += 1;
					}
				}

				remainingSpace -= child.DesiredSize.Height;
			}

			Size desiredSize = new Size();
			if (columnCount > 0)
			{
				desiredSize.Width = (columnCount * PanelWidth);
			}
			else
			{
				desiredSize.Width = 0;
			}

			desiredSize.Height = availableSize.Height;

			return desiredSize;
		}

		protected override Size ArrangeOverride (Size finalSize)
		{
			double offsetX = 0;
			double offsetY = 0;

			foreach (var child in Children)
			{
				if ((finalSize.Height - offsetY) < child.DesiredSize.Height)
				{
					if (offsetY != 0)
					{
						offsetX += PanelWidth;
						offsetY = 0;
					}
				}

				Rect rect = new Rect(new Point(offsetX, offsetY), new Size(PanelWidth, child.DesiredSize.Height));
				child.Arrange(rect);

				offsetY += child.DesiredSize.Height;
			}

			return base.ArrangeOverride(finalSize);
		}
	}
}
