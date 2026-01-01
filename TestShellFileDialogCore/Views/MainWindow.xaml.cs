using System.Windows;
using TestShellFileDialogCore.ViewModels;

namespace TestShellFileDialogCore.Views;

/// <summary>
/// MainWindow.xaml の相互作用ロジック
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
    }
}
