﻿using CustomWindowChromeExample.Styles;
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
	public MainWindow()
	{
		InitializeComponent();
		this.Icon = new BitmapImage(new Uri("C:\\Git\\C#\\Desktop\\CustomWindowChromeExample\\CustomWindowChromeExample\\favicon.ico"));
		var idk = Icon.GetType().ToString();
	}
}