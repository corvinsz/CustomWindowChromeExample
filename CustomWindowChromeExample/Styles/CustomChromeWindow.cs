using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace CustomWindowChromeExample.Styles;

public class CustomChromeWindow : Window
{
	static CustomChromeWindow()
	{
		BorderBrushProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(Brushes.Silver));
		TemplateProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(CreateTemplate()));
		BorderThicknessProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(new Thickness(1), UpdateWindowChrome));
		WindowStateProperty.OverrideMetadata(typeof(CustomChromeWindow), new FrameworkPropertyMetadata(WindowState.Normal, UpdateWindowChrome));
	}
	public CustomChromeWindow()
	{
		this.OverridesDefaultStyle = true;
		UpdateWindowChrome(this, default(DependencyPropertyChangedEventArgs));
	}

	#region Dependency-Properties
	public double TitlebarHeight
	{
		get => (double)GetValue(TitlebarHeightProperty);
		set => SetValue(TitlebarHeightProperty, value);
	}
	public static readonly DependencyProperty TitlebarHeightProperty =
		DependencyProperty.Register("TitlebarHeight", typeof(double), typeof(CustomChromeWindow), new PropertyMetadata(30d, UpdateWindowChrome));

	public TextAlignment TitlebarTextAlignment
	{
		get => (TextAlignment)GetValue(TitlebarTextAlignmentProperty);
		set => SetValue(TitlebarTextAlignmentProperty, value);
	}
	public static readonly DependencyProperty TitlebarTextAlignmentProperty =
		DependencyProperty.Register("TitlebarTextAlignment", typeof(TextAlignment), typeof(CustomChromeWindow), new PropertyMetadata(TextAlignment.Left));

	public Brush TitlebarForeground
	{
		get => (Brush)GetValue(TitlebarForegroundProperty);
		set => SetValue(TitlebarForegroundProperty, value);
	}
	public static readonly DependencyProperty TitlebarForegroundProperty =
		DependencyProperty.Register("TitlebarForeground", typeof(Brush), typeof(CustomChromeWindow), new PropertyMetadata(new SolidColorBrush(new Color() { R = 32, G = 32, B = 32, A = 255 })));

	public Brush TitlebarBackground
	{
		get => (Brush)GetValue(TitlebarBackgroundProperty);
		set => SetValue(TitlebarBackgroundProperty, value);
	}
	public static readonly DependencyProperty TitlebarBackgroundProperty =
		DependencyProperty.Register("TitlebarBackground", typeof(Brush), typeof(CustomChromeWindow), new PropertyMetadata(Brushes.Silver));

	public object TitlebarContent
	{
		get => GetValue(TitlebarContentProperty);
		set => SetValue(TitlebarContentProperty, value);
	}
	public static readonly DependencyProperty TitlebarContentProperty =
		DependencyProperty.Register("TitlebarContent", typeof(object), typeof(CustomChromeWindow), new PropertyMetadata(null));
	#endregion

	private static void UpdateWindowChrome(object sender, DependencyPropertyChangedEventArgs e)
	{
		var window = (CustomChromeWindow)sender;

		//Calculate and set the TitlebarHeight so the window chrome updates correctly
		double capHeight = window.TitlebarHeight + (window.WindowState == WindowState.Maximized ? 2 : -6) + window.BorderThickness.Top;
		capHeight = capHeight < 0 ? 0 : capHeight;
		window.SetValue(WindowChrome.WindowChromeProperty, new WindowChrome()
		{
			ResizeBorderThickness = new Thickness(5),
			UseAeroCaptionButtons = false,
			CaptionHeight = capHeight
		});
	}

	public static ControlTemplate CreateTemplate()
	{
		var controlTemplate = new ControlTemplate(typeof(CustomChromeWindow));

		FrameworkElementFactory _maximizedSpacingBorder = new(typeof(Border), "_maximizedSpacingBorder");
		_maximizedSpacingBorder.SetValue(Border.BorderBrushProperty, Brushes.Transparent);
		_maximizedSpacingBorder.SetValue(Border.BorderThicknessProperty, new Thickness(0));

		FrameworkElementFactory _windowBorder = new(typeof(Border), "_windowBorder");
		_windowBorder.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(CustomChromeWindow.BorderThicknessProperty));
		_windowBorder.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(CustomChromeWindow.BorderBrushProperty));
		_windowBorder.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(CustomChromeWindow.BackgroundProperty));

		FrameworkElementFactory _windowSeperation = new FrameworkElementFactory(typeof(DockPanel), "_windowSeperation");

		FrameworkElementFactory _titlebar = new FrameworkElementFactory(typeof(DockPanel), "_titlebar");
		_titlebar.SetValue(DockPanel.DockProperty, Dock.Top);
		_titlebar.SetValue(DockPanel.MarginProperty, new Thickness(-1, -1, -1, 0));
		_titlebar.SetValue(DockPanel.BackgroundProperty, new TemplateBindingExtension(CustomChromeWindow.TitlebarBackgroundProperty));
		_titlebar.SetValue(DockPanel.HeightProperty, new TemplateBindingExtension(CustomChromeWindow.TitlebarHeightProperty));

		Style _titlebarControlBaseStyle = new Style(typeof(Button))
		{
			Setters =
		{
			new Setter(DockPanel.DockProperty, Dock.Right),
			new Setter(Button.WidthProperty, 40d),
			new Setter(Button.HeightProperty, 30d),
			new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Top),
			new Setter(WindowChrome.IsHitTestVisibleInChromeProperty, true),
			new Setter(Button.BackgroundProperty, Brushes.Transparent),
			new Setter(Button.ForegroundProperty, new Binding("TitlebarForeground"){ RelativeSource = new RelativeSource(){ AncestorType = typeof(CustomChromeWindow) } }),
			new Setter(Button.TemplateProperty, CreateTitlebarControlTemplate())
		}
		};
		Style _titlebarControlStyleDefaultControl = new Style(typeof(Button), _titlebarControlBaseStyle)
		{
			Triggers =
		{
			new Trigger()
			{
				Property = Button.IsMouseOverProperty,
				Value = true,
				Setters =
				{
					new Setter(Button.BackgroundProperty, new SolidColorBrush(new Color(){ R = 255, G = 255, B = 255, A = 64 }))
				}
			},
			new Trigger()
			{
				Property = Button.IsPressedProperty,
				Value = true,
				Setters =
				{
					new Setter(Button.BackgroundProperty, new SolidColorBrush(new Color(){ R = 0, G = 0, B = 0, A = 64 }))
				}
			}
		}
		};
		Style _titlebarControlStyleCloseControl = new Style(typeof(Button), _titlebarControlBaseStyle)
		{
			Triggers =
		{
			new Trigger()
			{
				Property = Button.IsMouseOverProperty,
				Value = true,
				Setters =
				{
					new Setter(Button.ForegroundProperty, Brushes.WhiteSmoke),
					new Setter(Button.BackgroundProperty, new SolidColorBrush(new Color(){ R = 221, G = 51, B = 51, A = 255 }))
				}
			},
			new Trigger()
			{
				Property = Button.IsPressedProperty,
				Value = true,
				Setters =
				{
					new Setter(Button.BackgroundProperty, new SolidColorBrush(new Color(){ R = 255, G = 119, B = 119, A = 255 }))
				}
			}
		}
		};

		FrameworkElementFactory _closeButton = new FrameworkElementFactory(typeof(Button), "_closeTitlebarControl");
		_closeButton.SetValue(Button.StyleProperty, _titlebarControlStyleCloseControl);
		_closeButton.SetValue(Button.ContentProperty, Geometry.Parse("M0,0 L12,12 M0,12 L12,0"));
		_closeButton.AddHandler(Button.ClickEvent, (RoutedEventHandler)TitlebarControlClose_Click);
		_titlebar.AppendChild(_closeButton);

		FrameworkElementFactory _maximizeResizeButton = new FrameworkElementFactory(typeof(Button), "_maximizeResizeTitlebarControl");
		_maximizeResizeButton.SetValue(Button.StyleProperty, _titlebarControlStyleDefaultControl);
		_maximizeResizeButton.SetValue(Button.ContentProperty, Geometry.Parse("M0,0 H12 V12 H0 Z"));
		_maximizeResizeButton.AddHandler(Button.ClickEvent, (RoutedEventHandler)TitlebarControlMaximizeResize_Click);
		_titlebar.AppendChild(_maximizeResizeButton);

		FrameworkElementFactory _minimizeButton = new FrameworkElementFactory(typeof(Button), "_minimizeTitlebarControl");
		_minimizeButton.SetValue(Button.StyleProperty, _titlebarControlStyleDefaultControl);
		_minimizeButton.SetValue(Button.ContentProperty, Geometry.Parse("M0,12 H12"));
		_minimizeButton.AddHandler(Button.ClickEvent, (RoutedEventHandler)TitlebarControlMinimize_Click);
		_titlebar.AppendChild(_minimizeButton);

		FrameworkElementFactory _titlebarContent = new FrameworkElementFactory(typeof(ContentPresenter), "_titlebarContent");
		_titlebarContent.SetValue(DockPanel.DockProperty, Dock.Left);
		_titlebarContent.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(CustomChromeWindow.TitlebarContentProperty));
		_titlebar.AppendChild(_titlebarContent);

		var _icon = new FrameworkElementFactory(typeof(Image), "_icon");
		_icon.SetValue(System.Windows.Controls.Image.MarginProperty, new Thickness(8, 0, 0, 0));
		_icon.SetValue(System.Windows.Controls.Image.HeightProperty, 16d);
		_icon.SetValue(System.Windows.Controls.Image.WidthProperty, 16d);
		_icon.SetValue(System.Windows.Controls.Image.SourceProperty, new TemplateBindingExtension(CustomChromeWindow.IconProperty));
		_titlebar.AppendChild(_icon);


		FrameworkElementFactory _windowTitle = new FrameworkElementFactory(typeof(TextBlock), "_windowTitle");
		_windowTitle.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
		_windowTitle.SetValue(TextBlock.MarginProperty, new Thickness(8, 0, 8, 0));
		_windowTitle.SetValue(TextBlock.TextAlignmentProperty, new TemplateBindingExtension(CustomChromeWindow.TitlebarTextAlignmentProperty));
		_windowTitle.SetValue(TextBlock.ForegroundProperty, new TemplateBindingExtension(CustomChromeWindow.TitlebarForegroundProperty));
		_windowTitle.SetValue(TextBlock.TextProperty, new TemplateBindingExtension(CustomChromeWindow.TitleProperty));
		_titlebar.AppendChild(_windowTitle);


		FrameworkElementFactory _contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter), "_contentPresenter");

		var _adornerGrid = new FrameworkElementFactory(typeof(Grid), "_adornerGrid");
		_adornerGrid.SetValue(Grid.NameProperty, "PART_MainContentGrid");
		_adornerGrid.SetValue(Grid.BackgroundProperty, Brushes.Transparent);
		_adornerGrid.SetValue(Panel.ZIndexProperty, 10);

		var _adornerDectorator = new FrameworkElementFactory(typeof(System.Windows.Documents.AdornerDecorator), "_adornerDectorator");

		var _adornerContent = new FrameworkElementFactory(typeof(ContentPresenter), "_adornerContent");
		_adornerContent.SetValue(ContentPresenter.NameProperty, "PART_MainContentPresenter");

		_adornerDectorator.AppendChild(_adornerContent);
		_adornerGrid.AppendChild(_adornerDectorator);

		_contentPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(CustomChromeWindow.ContentProperty));



		_windowSeperation.AppendChild(_titlebar);
		_windowSeperation.AppendChild(_contentPresenter);
		_windowSeperation.AppendChild(_adornerGrid);

		_windowBorder.AppendChild(_windowSeperation);

		_maximizedSpacingBorder.AppendChild(_windowBorder);

		controlTemplate.VisualTree = _maximizedSpacingBorder;

		controlTemplate.Triggers.Add(new Trigger()
		{
			Property = CustomChromeWindow.WindowStateProperty,
			Value = WindowState.Maximized,
			Setters =
			{
				new Setter(Border.BorderThicknessProperty, new Thickness(8), "_maximizedSpacingBorder"),
				new Setter(Button.ContentProperty, Geometry.Parse("M0,3 H9 V12 H0 Z M3,3 V0 H12 V9 H9"), "_maximizeResizeTitlebarControl")
				//new Setter(WindowChrome.WindowChromeProperty, new WindowChrome() { ResizeBorderThickness = new Thickness(5), UseAeroCaptionButtons = false, CaptionHeight = this.TitlebarHeight + 4 })
            }
		});
		controlTemplate.Triggers.Add(new Trigger()
		{
			Property = CustomChromeWindow.IsActiveProperty,
			Value = false,
			Setters =
			{
				new Setter(CustomChromeWindow.TitlebarForegroundProperty, Brushes.Gray)
			}
		});
		controlTemplate.Triggers.Add(new Trigger()
		{
			Property = CustomChromeWindow.ResizeModeProperty,
			Value = ResizeMode.CanMinimize,
			Setters =
			{
				new Setter(Button.VisibilityProperty, Visibility.Collapsed, "_maximizeResizeTitlebarControl")
			}
		});
		controlTemplate.Triggers.Add(new Trigger()
		{
			Property = CustomChromeWindow.ResizeModeProperty,
			Value = ResizeMode.NoResize,
			Setters =
			{
				new Setter(Button.VisibilityProperty, Visibility.Collapsed, "_maximizeResizeTitlebarControl"),
				new Setter(Button.VisibilityProperty, Visibility.Collapsed, "_minimizeTitlebarControl")
			}
		});

		return controlTemplate;

		static ControlTemplate CreateTitlebarControlTemplate()
		{
			ControlTemplate controlTemplate = new ControlTemplate(typeof(Button));

			FrameworkElementFactory _border = new FrameworkElementFactory(typeof(Border), "_border");
			_border.SetValue(Border.WidthProperty, new TemplateBindingExtension(Button.WidthProperty));
			_border.SetValue(Border.HeightProperty, new TemplateBindingExtension(Button.HeightProperty));
			//_border.SetValue(Border.PaddingProperty, new TemplateBindingExtension(Button.PaddingProperty));
			_border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));

			FrameworkElementFactory _path = new FrameworkElementFactory(typeof(Path), "_path");
			_path.SetValue(Path.HorizontalAlignmentProperty, HorizontalAlignment.Center);
			_path.SetValue(Path.VerticalAlignmentProperty, VerticalAlignment.Center);
			_path.SetValue(Path.DataProperty, new TemplateBindingExtension(Button.ContentProperty));
			_path.SetValue(Path.StrokeProperty, new TemplateBindingExtension(Button.ForegroundProperty));

			_border.AppendChild(_path);

			controlTemplate.VisualTree = _border;

			return controlTemplate;
		}

		static void TitlebarControlClose_Click(object sender, RoutedEventArgs e)
		{
			GetWindow((Button)sender).Close();
		}

		static void TitlebarControlMaximizeResize_Click(object sender, RoutedEventArgs e)
		{
			Window window = GetWindow((Button)sender);
			window.WindowState = window.WindowState switch
			{
				WindowState.Maximized => WindowState.Normal,
				WindowState.Normal => WindowState.Maximized,
				_ => window.WindowState
			};
		}

		static void TitlebarControlMinimize_Click(object sender, RoutedEventArgs e)
		{
			GetWindow((Button)sender).WindowState = WindowState.Minimized;
		}
	}
}