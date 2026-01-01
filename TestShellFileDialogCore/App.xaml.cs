using System.Windows;
using Prism.Ioc;
using TestShellFileDialogCore.Views;

namespace TestShellFileDialogCore;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    /// <summary>
    /// 型の登録処理
    /// </summary>
    /// <param name="containerRegistry">型登録</param>
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    /// <summary>
    /// アプリケーションシェルの作成
    /// </summary>
    /// <returns></returns>
    protected override Window CreateShell()
    {
        return this.Container.Resolve<MainWindow>();
    }
}
