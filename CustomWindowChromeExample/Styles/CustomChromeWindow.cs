using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;

namespace CustomWindowChromeExample.Styles;

public class CustomChromeWindow : Window
{
	static CustomChromeWindow()
	{
		//BackgroundProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(Brushes.WhiteSmoke));
		BorderBrushProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(Brushes.Silver));
		TemplateProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(MetroWindowTemplateCreator.Create()));
		BorderThicknessProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(new Thickness(1), UpdateWindowChrome));
		WindowStateProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(WindowState.Normal, UpdateWindowChrome));
	}
	public CustomChromeWindow()
	{
		this.OverridesDefaultStyle = true;
		UpdateWindowChrome(this, new DependencyPropertyChangedEventArgs());
	}

	public double TitlebarHeight
	{
		get => (double)GetValue(TitlebarHeightProperty);
		set => SetValue(TitlebarHeightProperty, value);
	}
	public static readonly DependencyProperty TitlebarHeightProperty
		= DependencyProperty.Register("TitlebarHeight", typeof(double), typeof(CustomChromeWindow), new PropertyMetadata(30d, UpdateWindowChrome));

	public TextAlignment TitlebarTitleTextAlignment
	{
		get => (TextAlignment)GetValue(TitlebarTitleTextAlignmentProperty);
		set => SetValue(TitlebarTitleTextAlignmentProperty, value);
	}
	public static readonly DependencyProperty TitlebarTitleTextAlignmentProperty
		= DependencyProperty.Register("TitlebarTitleTextAlignment", typeof(TextAlignment), typeof(CustomChromeWindow), new PropertyMetadata(TextAlignment.Left));

	public Brush TitlebarForeground
	{
		get => (Brush)GetValue(TitlebarForegroundProperty);
		set => SetValue(TitlebarForegroundProperty, value);
	}
	public static readonly DependencyProperty TitlebarForegroundProperty
		= DependencyProperty.Register("TitlebarForeground", typeof(Brush), typeof(CustomChromeWindow), new PropertyMetadata(new SolidColorBrush(new Color() { R = 32, G = 32, B = 32, A = 255 })));

	public Brush TitlebarBackground
	{
		get => (Brush)GetValue(TitlebarBackgroundProperty);
		set => SetValue(TitlebarBackgroundProperty, value);
	}
	public static readonly DependencyProperty TitlebarBackgroundProperty
		= DependencyProperty.Register("TitlebarBackground", typeof(Brush), typeof(CustomChromeWindow), new PropertyMetadata(Brushes.Silver));

	public object TitlebarAdditionalElement
	{
		get => GetValue(TitlebarAdditionalElementProperty);
		set => SetValue(TitlebarAdditionalElementProperty, value);
	}
	public static readonly DependencyProperty TitlebarAdditionalElementProperty
		= DependencyProperty.Register("TitlebarAdditionalElement", typeof(object), typeof(CustomChromeWindow), new PropertyMetadata(null));


	private static void UpdateWindowChrome(object sender, DependencyPropertyChangedEventArgs e)
	{
		var window = (CustomChromeWindow)sender;

		double capHeight = window.TitlebarHeight + (window.WindowState == WindowState.Maximized ? 2 : -6) + window.BorderThickness.Top;
		capHeight = capHeight < 0 ? 0 : capHeight;

		window.SetValue(WindowChrome.WindowChromeProperty, new WindowChrome() { ResizeBorderThickness = new Thickness(5), UseAeroCaptionButtons = false, CaptionHeight = capHeight });
	}

	internal static void TitlebarControlClose_Click(object sender, RoutedEventArgs e)
	{
		GetWindow((Button)sender).Close();
	}

	internal static void TitlebarControlMaximizeResize_Click(object sender, RoutedEventArgs e)
	{
		Window window = GetWindow((Button)sender);
		switch (window.WindowState)
		{
			case WindowState.Maximized:
				window.WindowState = WindowState.Normal;
				break;
			case WindowState.Normal:
				window.WindowState = WindowState.Maximized;
				break;
		}

	}
	internal static void TitlebarControlMinimize_Click(object sender, RoutedEventArgs e)
	{
		GetWindow((Button)sender).WindowState = WindowState.Minimized;
	}
}