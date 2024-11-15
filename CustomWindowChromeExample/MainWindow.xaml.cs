using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomWindowChromeExample;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : CustomChromeWindow
{
	private readonly Brush _initialTitlebarBackground;

	public MainWindow()
	{
		InitializeComponent();

		_initialTitlebarBackground = this.TitlebarBackground;

		string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "favicon.ico");
		this.Icon = new BitmapImage(new Uri(iconPath));
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		if (sender is Button button)
		{
			MessageBox.Show($"{button.Content} clicked");
		}
	}

	private void Theme_Click(object sender, RoutedEventArgs e)
	{
		if (this.TitlebarBackground == _initialTitlebarBackground)
		{
			this.TitlebarBackground = Brushes.Fuchsia;
		}
		else
		{
			this.TitlebarBackground = _initialTitlebarBackground;
		}
	}
}