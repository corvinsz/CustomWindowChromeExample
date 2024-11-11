using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows;
using System.Windows.Shapes;

namespace CustomWindowChromeExample.Styles;

public static class MetroWindowTemplateCreator
{
	public static ControlTemplate Create()
	{
		ControlTemplate controlTemplate = new ControlTemplate(typeof(CustomChromeWindow));

		FrameworkElementFactory _maximizedSpacingBorder = new FrameworkElementFactory(typeof(Border), "_maximizedSpacingBorder");
		_maximizedSpacingBorder.SetValue(Border.BorderBrushProperty, Brushes.Transparent);
		_maximizedSpacingBorder.SetValue(Border.BorderThicknessProperty, new Thickness(0));

		FrameworkElementFactory _windowBorder = new FrameworkElementFactory(typeof(Border), "_windowBorder");
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
		_closeButton.AddHandler(Button.ClickEvent, (RoutedEventHandler)CustomChromeWindow.TitlebarControlClose_Click);
		_titlebar.AppendChild(_closeButton);

		FrameworkElementFactory _maximizeResizeButton = new FrameworkElementFactory(typeof(Button), "_maximizeResizeTitlebarControl");
		_maximizeResizeButton.SetValue(Button.StyleProperty, _titlebarControlStyleDefaultControl);
		_maximizeResizeButton.SetValue(Button.ContentProperty, Geometry.Parse("M0,0 H12 V12 H0 Z"));
		_maximizeResizeButton.AddHandler(Button.ClickEvent, (RoutedEventHandler)CustomChromeWindow.TitlebarControlMaximizeResize_Click);
		_titlebar.AppendChild(_maximizeResizeButton);

		FrameworkElementFactory _minimizeButton = new FrameworkElementFactory(typeof(Button), "_minimizeTitlebarControl");
		_minimizeButton.SetValue(Button.StyleProperty, _titlebarControlStyleDefaultControl);
		_minimizeButton.SetValue(Button.ContentProperty, Geometry.Parse("M0,12 H12"));
		_minimizeButton.AddHandler(Button.ClickEvent, (RoutedEventHandler)CustomChromeWindow.TitlebarControlMinimize_Click);
		_titlebar.AppendChild(_minimizeButton);

		FrameworkElementFactory _titlebarAdditionalElementPresenter = new FrameworkElementFactory(typeof(ContentPresenter), "_titlebarAdditionalElementPresenter");
		_titlebarAdditionalElementPresenter.SetValue(DockPanel.DockProperty, Dock.Left);
		_titlebarAdditionalElementPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(CustomChromeWindow.TitlebarAdditionalElementProperty));
		_titlebar.AppendChild(_titlebarAdditionalElementPresenter);

		FrameworkElementFactory _windowTitle = new FrameworkElementFactory(typeof(TextBlock), "_windowTitle");
		_windowTitle.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
		_windowTitle.SetValue(TextBlock.MarginProperty, new Thickness(8, 0, 8, 0));
		_windowTitle.SetValue(TextBlock.TextAlignmentProperty, new TemplateBindingExtension(CustomChromeWindow.TitlebarTitleTextAlignmentProperty));
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
	}
	private static ControlTemplate CreateTitlebarControlTemplate()
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
}