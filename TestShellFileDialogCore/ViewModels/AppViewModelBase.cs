using System.Reactive.Disposables;
using Prism.Mvvm;

namespace TestShellFileDialogCore.ViewModels;

/// <summary>
/// アプリケーションのベースViewModel
/// </summary>
public class AppViewModelBase : BindableBase, IDisposable
{
    // 構築
    #region コンストラクタ
    /// <summary>
    /// デフォルトコンストラクタ
    /// </summary>
    public AppViewModelBase()
    {
        this.Disposables = new CompositeDisposable();
    }
    #endregion

    // 公開メソッド
    #region 破棄
    /// <summary>
    /// リソースを破棄する
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion

    // 保護プロパティ
    #region リソース管理
    /// <summary>破棄済みフラグ</summary>
    protected bool IsDisposed { get; private set; }

    /// <summary>破棄予定リソースを格納するコレクション</summary>
    /// <remarks>破棄は登録と逆順で行われる</remarks>
    protected CompositeDisposable Disposables { get; }
    #endregion

    // 保護メソッド
    #region 破棄
    /// <summary>
    /// リソースを破棄する
    /// </summary>
    /// <param name="disposing">マネージリソースの破棄過程であるか</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.IsDisposed)
        {
            if (disposing)
            {
                this.Disposables.Dispose();
            }

            this.IsDisposed = true;
        }
    }
    #endregion
}
