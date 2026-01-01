
using R3;

namespace TestShellFileDialogCore.ViewModels;

/// <summary>
/// ReactiveTrigger向けのトリガソース補助クラス
/// </summary>
/// <typeparam name="T">パラメータの型</typeparam>
public class ReactiveInteraction<T>
{
    // 構築
    #region コンストラクタ
    /// <summary>
    /// デフォルトコンストラクタ
    /// </summary>
    public ReactiveInteraction()
    {
        this.Source = Observable.FromEvent<T>(h => this.pulse += h, h => this.pulse -= h).AsSystemObservable();
    }
    #endregion

    // 公開プロパティ
    #region トリガ
    /// <summary>トリガソースシーケンス</summary>
    public IObservable<T> Source { get; }
    #endregion

    // 公開メソッド
    #region トリガ
    /// <summary>
    /// トリガを発生させる(トリガソースに値を流す)
    /// </summary>
    /// <param name="parameter">トリガパラメータ</param>
    public void Raise(T parameter)
    {
        this.pulse?.Invoke(parameter);
    }
    #endregion

    // 非公開フィールド
    #region トリガ
    /// <summary>トリガソースシーケンスに値を流すためのデリゲート</summary>
    private Action<T>? pulse;
    #endregion
}

/// <summary>
/// ReactiveTrigger向けのトリガソース補助クラス
/// </summary>
public class ReactiveInteraction
{
    // 構築
    #region コンストラクタ
    /// <summary>
    /// デフォルトコンストラクタ
    /// </summary>
    public ReactiveInteraction()
    {
        this.Source = Observable.FromEvent<object>(h => this.pulse += h, h => this.pulse -= h).AsSystemObservable();
    }
    #endregion

    // 公開プロパティ
    #region トリガ
    /// <summary>トリガソースシーケンス</summary>
    public IObservable<object> Source { get; }
    #endregion

    // 公開メソッド
    #region トリガ
    /// <summary>
    /// トリガを発生させる(トリガソースに値を流す)
    /// </summary>
    /// <param name="parameter">トリガパラメータ</param>
    public void Raise()
    {
        this.pulse?.Invoke(null);
    }
    #endregion

    // 非公開フィールド
    #region トリガ
    /// <summary>トリガソースシーケンスに値を流すためのデリゲート</summary>
    private Action<object?>? pulse;
    #endregion
}
