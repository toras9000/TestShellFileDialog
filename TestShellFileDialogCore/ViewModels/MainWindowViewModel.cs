using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CometFlavor.Wpf.Win32.Dialogs;
using CometFlavor.Wpf.Win32.Interactions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace TestShellFileDialogCore.ViewModels
{
    /// <summary>
    /// MainWindowのViewModel
    /// </summary>
    public class MainWindowViewModel : AppViewModelBase
    {
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            // 領域ごとのVM生成
            this.OpenFileDialogContext = new OpenFileDialogViewModel()
                .AddTo(this.Disposables);

            this.SaveFileDialogContext = new SaveFileDialogViewModel()
                .AddTo(this.Disposables);
        }

        /// <summary>ファイルオープンダイアログ領域のViewModel</summary>
        public OpenFileDialogViewModel OpenFileDialogContext { get; }

        /// <summary>ファイルセーブダイアログ領域のViewModel</summary>
        public SaveFileDialogViewModel SaveFileDialogContext { get; }
    }

    /// <summary>
    /// ファイルオープンダイアログ領域のViewModel
    /// </summary>
    public class OpenFileDialogViewModel : AppViewModelBase
    {
        // 構築
        #region コンストラクタ
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public OpenFileDialogViewModel()
        {
            // ダイアログ表示用インタラクションヘルパ
            var showInteraction = new ReactiveInteraction<ShellOpenFileDialogActionParameter>();

            // ダイアログ表示用インタラクションのトリガソース
            this.ShowDialogRequest = showInteraction.Source;

            // ビューのエラー状態
            this.HasViewError = new ReactivePropertySlim<bool>()
                .AddTo(this.Disposables);

            // ダイアログ結果設定用デリゲート
            var dlgResultFilter = default(Action<uint?>);
            var dlgResultItems = default(Action<string>);

            // ダイアログ表示コマンド
            this.ShowDialogCommand = new[]
                {
                    this.HasViewError.Inverse(),
                }
                .CombineLatestValuesAreAllTrue()
                .ToReactiveCommand()
                .WithSubscribe(() => showOpenDialog())
                .AddTo(this.Disposables);

            // デフォルト設定参照用
            var defParam = new ShellOpenFileDialogParameter();

            #region 動作設定：状態設定
            this.Directory = new ReactivePropertySlim<string>(defParam.Directory)
                .AddTo(this.Disposables);

            this.InitialFileName = new ReactivePropertySlim<string>(defParam.InitialFileName)
                .AddTo(this.Disposables);

            this.DefaultExtension = new ReactivePropertySlim<string>(defParam.DefaultExtension)
                .AddTo(this.Disposables);

            this.Filters = new ReactivePropertySlim<string>(string.Join("\n", new[]
                {
                    "画像ファイル|*.png;*.jpg;*.gif;*.bmp",
                    "テキストファイル|*.txt;*.text;",
                    "全てのファイル|*.*",
                }))
                .AddTo(this.Disposables);

            this.InitialFilterIndex = new ReactivePropertySlim<uint>(defParam.InitialFilterIndex)
                .AddTo(this.Disposables);

            this.DefaultDirectory = new ReactivePropertySlim<string>(defParam.DefaultDirectory)
                .AddTo(this.Disposables);

            this.ClientGuid = new ReactivePropertySlim<Guid>(defParam.ClientGuid)
                .AddTo(this.Disposables);
            #endregion

            #region 動作設定：オプション
            this.StrictFileTypes = new ReactivePropertySlim<bool>(defParam.StrictFileTypes)
                .AddTo(this.Disposables);

            this.NoChangeDirectory = new ReactivePropertySlim<bool>(defParam.NoChangeDirectory)
                .AddTo(this.Disposables);

            this.PickFolders = new ReactivePropertySlim<bool>(defParam.PickFolders)
                .AddTo(this.Disposables);

            this.ForceFileSystem = new ReactivePropertySlim<bool>(defParam.ForceFileSystem)
                .AddTo(this.Disposables);

            this.AllNonStorageItems = new ReactivePropertySlim<bool>(defParam.AllNonStorageItems)
                .AddTo(this.Disposables);

            this.NoValidate = new ReactivePropertySlim<bool>(defParam.NoValidate)
                .AddTo(this.Disposables);

            this.AllowMultiSelect = new ReactivePropertySlim<bool>(defParam.AllowMultiSelect)
                .AddTo(this.Disposables);

            this.PathMustExist = new ReactivePropertySlim<bool>(defParam.PathMustExist)
                .AddTo(this.Disposables);

            this.FileMustExist = new ReactivePropertySlim<bool>(defParam.FileMustExist)
                .AddTo(this.Disposables);

            this.CreatePrompt = new ReactivePropertySlim<bool>(defParam.CreatePrompt)
                .AddTo(this.Disposables);

            this.ShareAware = new ReactivePropertySlim<bool>(defParam.ShareAware)
                .AddTo(this.Disposables);

            this.NoReadOnlyReturn = new ReactivePropertySlim<bool>(defParam.NoReadOnlyReturn)
                .AddTo(this.Disposables);

            this.NoTestFileCreate = new ReactivePropertySlim<bool>(defParam.NoTestFileCreate)
                .AddTo(this.Disposables);

            this.HidePinnedPlaces = new ReactivePropertySlim<bool>(defParam.HidePinnedPlaces)
                .AddTo(this.Disposables);

            this.NoDereferenceLinks = new ReactivePropertySlim<bool>(defParam.NoDereferenceLinks)
                .AddTo(this.Disposables);

            this.OkButtonNeedsInteraction = new ReactivePropertySlim<bool>(defParam.OkButtonNeedsInteraction)
                .AddTo(this.Disposables);

            this.DontAddToRecent = new ReactivePropertySlim<bool>(defParam.DontAddToRecent)
                .AddTo(this.Disposables);

            this.ForceShowHidden = new ReactivePropertySlim<bool>(defParam.ForceShowHidden)
                .AddTo(this.Disposables);

            this.ForcePreviewPaneOn = new ReactivePropertySlim<bool>(defParam.ForcePreviewPaneOn)
                .AddTo(this.Disposables);
            #endregion

            #region 動作設定：ダイアログカスタマイズ
            this.Title = new ReactivePropertySlim<string>(defParam.Title)
                .AddTo(this.Disposables);

            this.AcceptButtonLabel = new ReactivePropertySlim<string>(defParam.AcceptButtonLabel)
                .AddTo(this.Disposables);

            this.FileNameLabel = new ReactivePropertySlim<string>(defParam.FileNameLabel)
                .AddTo(this.Disposables);

            this.AdditionalPlaces = new ReactivePropertySlim<string>(string.Join("\n", new[]
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                }))
                .AddTo(this.Disposables);
            #endregion

            #region ダイアログ結果
            this.ResultFilterIndex = Observable.FromEvent<uint?>(h => dlgResultFilter += h, h => dlgResultFilter -= h)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.Disposables);

            this.ResultItems = Observable.FromEvent<string>(h => dlgResultItems += h, h => dlgResultItems -= h)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.Disposables);
            #endregion

            #region ローカル関数
            // ファイルオープンダイアログ表示ローカル関数
            void showOpenDialog()
            {
                // ダイアログパラメータを準備
                var dlgParam = new ShellOpenFileDialogParameter();
                dlgParam.Directory = this.Directory.ValueIfNotEmpty();
                dlgParam.InitialFileName = this.InitialFileName.ValueIfNotEmpty();
                dlgParam.DefaultExtension = this.DefaultExtension.ValueIfNotEmpty();
                dlgParam.SetFiltersFrom(this.Filters.Value);
                dlgParam.InitialFilterIndex = this.InitialFilterIndex.Value;
                dlgParam.DefaultDirectory = this.DefaultDirectory.ValueIfNotEmpty();
                dlgParam.ClientGuid = this.ClientGuid.Value;

                dlgParam.StrictFileTypes = this.StrictFileTypes.Value;
                dlgParam.NoChangeDirectory = this.NoChangeDirectory.Value;
                dlgParam.PickFolders = this.PickFolders.Value;
                dlgParam.ForceFileSystem = this.ForceFileSystem.Value;
                dlgParam.AllNonStorageItems = this.AllNonStorageItems.Value;
                dlgParam.NoValidate = this.NoValidate.Value;
                dlgParam.AllowMultiSelect = this.AllowMultiSelect.Value;
                dlgParam.PathMustExist = this.PathMustExist.Value;
                dlgParam.FileMustExist = this.FileMustExist.Value;
                dlgParam.CreatePrompt = this.CreatePrompt.Value;
                dlgParam.ShareAware = this.ShareAware.Value;
                dlgParam.NoReadOnlyReturn = this.NoReadOnlyReturn.Value;
                dlgParam.NoTestFileCreate = this.NoTestFileCreate.Value;
                dlgParam.HidePinnedPlaces = this.HidePinnedPlaces.Value;
                dlgParam.NoDereferenceLinks = this.NoDereferenceLinks.Value;
                dlgParam.OkButtonNeedsInteraction = this.OkButtonNeedsInteraction.Value;
                dlgParam.DontAddToRecent = this.DontAddToRecent.Value;
                dlgParam.ForceShowHidden = this.ForceShowHidden.Value;
                dlgParam.ForcePreviewPaneOn = this.ForcePreviewPaneOn.Value;

                dlgParam.Title = this.Title.ValueIfNotEmpty();
                dlgParam.AcceptButtonLabel = this.AcceptButtonLabel.ValueIfNotEmpty();
                dlgParam.FileNameLabel = this.FileNameLabel.ValueIfNotEmpty();
                dlgParam.SetAdditionalPlaces(this.AdditionalPlaces.Value);

                // ダイアログ表示インタラクションパラメータを準備
                var actionParam = new ShellOpenFileDialogActionParameter();
                actionParam.Parameter = dlgParam;

                // ダイアログ表示要求を発行
                showInteraction.Raise(actionParam);

                // 結果をプロパティに設定
                if (actionParam.Exception != null)
                {
                    // なんらか例外が生じた
                    dlgResultItems?.Invoke(actionParam.Exception.Message);
                    dlgResultFilter?.Invoke(null);
                }
                else if (actionParam.Result != null)
                {
                    // ダイアログ結果。表示してキャンセルした場合を含む。
                    var items = string.Join("\n", actionParam.Result.Items);
                    if (string.IsNullOrWhiteSpace(items))
                    {
                        items = "<cancelled>";
                    }
                    dlgResultItems?.Invoke(items);
                    dlgResultFilter?.Invoke(actionParam.Result.FilterIndex);
                }
                else
                {
                    // 通常あり得ない状態。
                    dlgResultItems?.Invoke("<no result>");
                    dlgResultFilter?.Invoke(null);
                }
            }
            #endregion
        }
        #endregion

        // 公開プロパティ
        #region インタラクション
        /// <summary>ビューのエラー状態バインド用</summary>
        public ReactivePropertySlim<bool> HasViewError { get; }

        /// <summary>ダイアログ表示要求トリガソース</summary>
        public IObservable<ShellOpenFileDialogActionParameter> ShowDialogRequest { get; }

        /// <summary>ダイアログ表示コマンド</summary>
        public ReactiveCommand ShowDialogCommand { get; }
        #endregion

        #region 動作設定：状態設定
        /// <summary>初期フォルダ</summary>
        public ReactivePropertySlim<string> Directory { get; }

        /// <summary>初期入力ファイル名</summary>
        public ReactivePropertySlim<string> InitialFileName { get; }

        /// <summary>デフォルト拡張子</summary>
        public ReactivePropertySlim<string> DefaultExtension { get; }

        /// <summary>選択できるファイルフィルタ</summary>
        public ReactivePropertySlim<string> Filters { get; }

        /// <summary>ファイルフィルタの初期選択インデックス(1ベース値)</summary>
        public ReactivePropertySlim<uint> InitialFilterIndex { get; }

        /// <summary>最近使用したフォルダーが無い場合のデフォルトフォルダ</summary>
        public ReactivePropertySlim<string> DefaultDirectory { get; }

        /// <summary>ダイアログ状態(最終フォルダや位置/サイズ)の永続化のためのGUID</summary>
        public ReactivePropertySlim<Guid> ClientGuid { get; }
        #endregion

        #region 動作設定：オプション
        /// <summary>設定されたファイルタイプのファイルのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> StrictFileTypes { get; }

        /// <summary>カレントディレクトリを変更しない</summary>
        public ReactivePropertySlim<bool> NoChangeDirectory { get; }

        /// <summary>フォルダを選択する</summary>
        public ReactivePropertySlim<bool> PickFolders { get; }

        /// <summary>ファイルシステムアイテムのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> ForceFileSystem { get; }

        /// <summary>シェルネームスペース内のすべてのアイテムを選択可能とする</summary>
        public ReactivePropertySlim<bool> AllNonStorageItems { get; }

        /// <summary>ファイルを開けない状態(共有不可やアクセス拒否等)を検証しない</summary>
        public ReactivePropertySlim<bool> NoValidate { get; }

        /// <summary>複数選択を許可する</summary>
        public ReactivePropertySlim<bool> AllowMultiSelect { get; }

        /// <summary>存在するフォルダのアイテムのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> PathMustExist { get; }

        /// <summary>存在するアイテムのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> FileMustExist { get; }

        /// <summary>ファイルの作成確認を表示する</summary>
        public ReactivePropertySlim<bool> CreatePrompt { get; }

        /// <summary>共有違反(アプリケーションが開いている場合)のガイダンスを表示する</summary>
        public ReactivePropertySlim<bool> ShareAware { get; }

        /// <summary>読み取り専用アイテムを選択不可とする</summary>
        public ReactivePropertySlim<bool> NoReadOnlyReturn { get; }

        /// <summary>ファイルが作成可能であるかをテストしない</summary>
        public ReactivePropertySlim<bool> NoTestFileCreate { get; }

        /// <summary>ナビゲーションペインのデフォルトアイテムを非表示とする</summary>
        public ReactivePropertySlim<bool> HidePinnedPlaces { get; }

        /// <summary>ショートカットの参照先解決をしない。(ショートカットファイル自体を選択可能とする)</summary>
        public ReactivePropertySlim<bool> NoDereferenceLinks { get; }

        /// <summary>確定ボタン操作のためにユーザ操作を必要とする</summary>
        public ReactivePropertySlim<bool> OkButtonNeedsInteraction { get; }

        /// <summary>選択アイテムを最近使用したファイルのリストに追加しない</summary>
        public ReactivePropertySlim<bool> DontAddToRecent { get; }

        /// <summary>非表示属性のアイテムを表示する</summary>
        public ReactivePropertySlim<bool> ForceShowHidden { get; }

        /// <summary>プレビューペインを常に表示する</summary>
        public ReactivePropertySlim<bool> ForcePreviewPaneOn { get; }
        #endregion

        #region 動作設定：ダイアログカスタマイズ
        /// <summary>ダイアログのタイトル</summary>
        public ReactivePropertySlim<string> Title { get; }

        /// <summary>確定ボタンのラベル</summary>
        public ReactivePropertySlim<string> AcceptButtonLabel { get; }

        /// <summary>ファイル名入力欄のラベル</summary>
        public ReactivePropertySlim<string> FileNameLabel { get; }

        /// <summary>追加の選択場所</summary>
        public ReactivePropertySlim<string> AdditionalPlaces { get; }
        #endregion

        #region ダイアログ結果
        /// <summary>選択時のフィルタインデックス</summary>
        public ReadOnlyReactivePropertySlim<uint?> ResultFilterIndex { get; }

        /// <summary>ダイアログの選択結果</summary>
        public ReadOnlyReactivePropertySlim<string> ResultItems { get; }
        #endregion
    }

    /// <summary>
    /// ファイルセーブダイアログ領域のViewModel
    /// </summary>
    public class SaveFileDialogViewModel : AppViewModelBase
    {
        // 構築
        #region コンストラクタ
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public SaveFileDialogViewModel()
        {
            // ダイアログ表示用インタラクションヘルパ
            var showInteraction = new ReactiveInteraction<ShellSaveFileDialogActionParameter>();

            // ダイアログ表示用インタラクションのトリガソース
            this.ShowDialogRequest = showInteraction.Source;

            // ビューのエラー状態
            this.HasViewError = new ReactivePropertySlim<bool>()
                .AddTo(this.Disposables);

            // ダイアログ結果設定用デリゲート
            var dlgResultFilter = default(Action<uint?>);
            var dlgResultItem = default(Action<string>);

            // ダイアログ表示コマンド
            this.ShowDialogCommand = new[]
                {
                    this.HasViewError.Inverse(),
                }
                .CombineLatestValuesAreAllTrue()
                .ToReactiveCommand()
                .WithSubscribe(() => showSaveDialog())
                .AddTo(this.Disposables);

            // デフォルト設定参照用
            var defParam = new ShellSaveFileDialogParameter();

            #region 動作設定：状態設定
            this.Directory = new ReactivePropertySlim<string>(defParam.Directory)
                .AddTo(this.Disposables);

            this.InitialFileName = new ReactivePropertySlim<string>(defParam.InitialFileName)
                .AddTo(this.Disposables);

            this.DefaultExtension = new ReactivePropertySlim<string>(defParam.DefaultExtension)
                .AddTo(this.Disposables);

            this.Filters = new ReactivePropertySlim<string>(string.Join("\n", new[]
                {
                    "画像ファイル|*.png;*.jpg;*.gif;*.bmp",
                    "テキストファイル|*.txt;*.text;",
                    "全てのファイル|*.*",
                }))
                .AddTo(this.Disposables);

            this.InitialFilterIndex = new ReactivePropertySlim<uint>(defParam.InitialFilterIndex)
                .AddTo(this.Disposables);

            this.DefaultDirectory = new ReactivePropertySlim<string>(defParam.DefaultDirectory)
                .AddTo(this.Disposables);

            this.ClientGuid = new ReactivePropertySlim<Guid>(defParam.ClientGuid)
                .AddTo(this.Disposables);
            #endregion

            #region 動作設定：オプション
            this.OverwritePrompt = new ReactivePropertySlim<bool>(defParam.OverwritePrompt)
                .AddTo(this.Disposables);

            this.StrictFileTypes = new ReactivePropertySlim<bool>(defParam.StrictFileTypes)
                .AddTo(this.Disposables);

            this.NoChangeDirectory = new ReactivePropertySlim<bool>(defParam.NoChangeDirectory)
                .AddTo(this.Disposables);

            this.ForceFileSystem = new ReactivePropertySlim<bool>(defParam.ForceFileSystem)
                .AddTo(this.Disposables);

            this.AllNonStorageItems = new ReactivePropertySlim<bool>(defParam.AllNonStorageItems)
                .AddTo(this.Disposables);

            this.NoValidate = new ReactivePropertySlim<bool>(defParam.NoValidate)
                .AddTo(this.Disposables);

            this.PathMustExist = new ReactivePropertySlim<bool>(defParam.PathMustExist)
                .AddTo(this.Disposables);

            this.FileMustExist = new ReactivePropertySlim<bool>(defParam.FileMustExist)
                .AddTo(this.Disposables);

            this.CreatePrompt = new ReactivePropertySlim<bool>(defParam.CreatePrompt)
                .AddTo(this.Disposables);

            this.ShareAware = new ReactivePropertySlim<bool>(defParam.ShareAware)
                .AddTo(this.Disposables);

            this.NoReadOnlyReturn = new ReactivePropertySlim<bool>(defParam.NoReadOnlyReturn)
                .AddTo(this.Disposables);

            this.NoTestFileCreate = new ReactivePropertySlim<bool>(defParam.NoTestFileCreate)
                .AddTo(this.Disposables);

            this.HidePinnedPlaces = new ReactivePropertySlim<bool>(defParam.HidePinnedPlaces)
                .AddTo(this.Disposables);

            this.NoDereferenceLinks = new ReactivePropertySlim<bool>(defParam.NoDereferenceLinks)
                .AddTo(this.Disposables);

            this.OkButtonNeedsInteraction = new ReactivePropertySlim<bool>(defParam.OkButtonNeedsInteraction)
                .AddTo(this.Disposables);

            this.DontAddToRecent = new ReactivePropertySlim<bool>(defParam.DontAddToRecent)
                .AddTo(this.Disposables);

            this.ForceShowHidden = new ReactivePropertySlim<bool>(defParam.ForceShowHidden)
                .AddTo(this.Disposables);
            #endregion

            #region 動作設定：ダイアログカスタマイズ
            this.Title = new ReactivePropertySlim<string>(defParam.Title)
                .AddTo(this.Disposables);

            this.AcceptButtonLabel = new ReactivePropertySlim<string>(defParam.AcceptButtonLabel)
                .AddTo(this.Disposables);

            this.FileNameLabel = new ReactivePropertySlim<string>(defParam.FileNameLabel)
                .AddTo(this.Disposables);

            this.AdditionalPlaces = new ReactivePropertySlim<string>(string.Join("\n", new[]
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                }))
                .AddTo(this.Disposables);
            #endregion

            #region ダイアログ結果
            this.ResultFilterIndex = Observable.FromEvent<uint?>(h => dlgResultFilter += h, h => dlgResultFilter -= h)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.Disposables);

            this.ResultItems = Observable.FromEvent<string>(h => dlgResultItem += h, h => dlgResultItem -= h)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.Disposables);
            #endregion

            #region ローカル関数
            // ファイルオープンダイアログ表示ローカル関数
            void showSaveDialog()
            {
                // ダイアログパラメータを準備
                var dlgParam = new ShellSaveFileDialogParameter();
                dlgParam.Directory = this.Directory.ValueIfNotEmpty();
                dlgParam.InitialFileName = this.InitialFileName.ValueIfNotEmpty();
                dlgParam.DefaultExtension = this.DefaultExtension.ValueIfNotEmpty();
                dlgParam.SetFiltersFrom(this.Filters.Value);
                dlgParam.InitialFilterIndex = this.InitialFilterIndex.Value;
                dlgParam.DefaultDirectory = this.DefaultDirectory.ValueIfNotEmpty();
                dlgParam.ClientGuid = this.ClientGuid.Value;

                dlgParam.OverwritePrompt = this.OverwritePrompt.Value;
                dlgParam.StrictFileTypes = this.StrictFileTypes.Value;
                dlgParam.NoChangeDirectory = this.NoChangeDirectory.Value;
                dlgParam.ForceFileSystem = this.ForceFileSystem.Value;
                dlgParam.AllNonStorageItems = this.AllNonStorageItems.Value;
                dlgParam.NoValidate = this.NoValidate.Value;
                dlgParam.PathMustExist = this.PathMustExist.Value;
                dlgParam.FileMustExist = this.FileMustExist.Value;
                dlgParam.CreatePrompt = this.CreatePrompt.Value;
                dlgParam.ShareAware = this.ShareAware.Value;
                dlgParam.NoReadOnlyReturn = this.NoReadOnlyReturn.Value;
                dlgParam.NoTestFileCreate = this.NoTestFileCreate.Value;
                dlgParam.HidePinnedPlaces = this.HidePinnedPlaces.Value;
                dlgParam.NoDereferenceLinks = this.NoDereferenceLinks.Value;
                dlgParam.OkButtonNeedsInteraction = this.OkButtonNeedsInteraction.Value;
                dlgParam.DontAddToRecent = this.DontAddToRecent.Value;
                dlgParam.ForceShowHidden = this.ForceShowHidden.Value;

                dlgParam.Title = this.Title.ValueIfNotEmpty();
                dlgParam.AcceptButtonLabel = this.AcceptButtonLabel.ValueIfNotEmpty();
                dlgParam.FileNameLabel = this.FileNameLabel.ValueIfNotEmpty();
                dlgParam.SetAdditionalPlaces(this.AdditionalPlaces.Value);

                // ダイアログ表示インタラクションパラメータを準備
                var actionParam = new ShellSaveFileDialogActionParameter();
                actionParam.Parameter = dlgParam;

                // ダイアログ表示要求を発行
                showInteraction.Raise(actionParam);

                // 結果をプロパティに設定
                if (actionParam.Exception != null)
                {
                    // なんらか例外が生じた
                    dlgResultItem?.Invoke(actionParam.Exception.Message);
                    dlgResultFilter?.Invoke(null);
                }
                else if (actionParam.Result != null)
                {
                    // ダイアログ結果。表示してキャンセルした場合を含む。
                    var item = actionParam.Result.Item ?? "<cancelled>";
                    dlgResultItem?.Invoke(item);
                    dlgResultFilter?.Invoke(actionParam.Result.FilterIndex);
                }
                else
                {
                    // 通常あり得ない状態。
                    dlgResultItem?.Invoke("<no result>");
                    dlgResultFilter?.Invoke(null);
                }
            }
            #endregion
        }
        #endregion

        // 公開プロパティ
        #region インタラクション
        /// <summary>ビューのエラー状態バインド用</summary>
        public ReactivePropertySlim<bool> HasViewError { get; }

        /// <summary>ダイアログ表示要求トリガソース</summary>
        public IObservable<ShellSaveFileDialogActionParameter> ShowDialogRequest { get; }

        /// <summary>ダイアログ表示コマンド</summary>
        public ReactiveCommand ShowDialogCommand { get; }
        #endregion

        #region 動作設定：状態設定
        /// <summary>初期フォルダ</summary>
        public ReactivePropertySlim<string> Directory { get; }

        /// <summary>初期入力ファイル名</summary>
        public ReactivePropertySlim<string> InitialFileName { get; }

        /// <summary>デフォルト拡張子</summary>
        public ReactivePropertySlim<string> DefaultExtension { get; }

        /// <summary>選択できるファイルフィルタ</summary>
        public ReactivePropertySlim<string> Filters { get; }

        /// <summary>ファイルフィルタの初期選択インデックス(1ベース値)</summary>
        public ReactivePropertySlim<uint> InitialFilterIndex { get; }

        /// <summary>最近使用したフォルダーが無い場合のデフォルトフォルダ</summary>
        public ReactivePropertySlim<string> DefaultDirectory { get; }

        /// <summary>ダイアログ状態(最終フォルダや位置/サイズ)の永続化のためのGUID</summary>
        public ReactivePropertySlim<Guid> ClientGuid { get; }
        #endregion

        #region 動作設定：オプション
        /// <summary>上書きの確認を表示する</summary>
        public ReactivePropertySlim<bool> OverwritePrompt { get; }

        /// <summary>設定されたファイルタイプのファイルのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> StrictFileTypes { get; }

        /// <summary>カレントディレクトリを変更しない</summary>
        public ReactivePropertySlim<bool> NoChangeDirectory { get; }

        /// <summary>ファイルシステムアイテムのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> ForceFileSystem { get; }

        /// <summary>シェルネームスペース内のすべてのアイテムを選択可能とする</summary>
        public ReactivePropertySlim<bool> AllNonStorageItems { get; }

        /// <summary>ファイルを開けない状態(共有不可やアクセス拒否等)を検証しない</summary>
        public ReactivePropertySlim<bool> NoValidate { get; }

        /// <summary>存在するフォルダのアイテムのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> PathMustExist { get; }

        /// <summary>存在するアイテムのみを選択可能とする</summary>
        public ReactivePropertySlim<bool> FileMustExist { get; }

        /// <summary>ファイルの作成確認を表示する</summary>
        public ReactivePropertySlim<bool> CreatePrompt { get; }

        /// <summary>共有違反(アプリケーションが開いている場合)のガイダンスを表示する</summary>
        public ReactivePropertySlim<bool> ShareAware { get; }

        /// <summary>読み取り専用アイテムを選択不可とする</summary>
        public ReactivePropertySlim<bool> NoReadOnlyReturn { get; }

        /// <summary>ファイルが作成可能であるかをテストしない</summary>
        public ReactivePropertySlim<bool> NoTestFileCreate { get; }

        /// <summary>ナビゲーションペインのデフォルトアイテムを非表示とする</summary>
        public ReactivePropertySlim<bool> HidePinnedPlaces { get; }

        /// <summary>ショートカットの参照先解決をしない。(ショートカットファイル自体を選択可能とする)</summary>
        public ReactivePropertySlim<bool> NoDereferenceLinks { get; }

        /// <summary>確定ボタン操作のためにユーザ操作を必要とする</summary>
        public ReactivePropertySlim<bool> OkButtonNeedsInteraction { get; }

        /// <summary>選択アイテムを最近使用したファイルのリストに追加しない</summary>
        public ReactivePropertySlim<bool> DontAddToRecent { get; }

        /// <summary>非表示属性のアイテムを表示する</summary>
        public ReactivePropertySlim<bool> ForceShowHidden { get; }
        #endregion

        #region 動作設定：ダイアログカスタマイズ
        /// <summary>ダイアログのタイトル</summary>
        public ReactivePropertySlim<string> Title { get; }

        /// <summary>確定ボタンのラベル</summary>
        public ReactivePropertySlim<string> AcceptButtonLabel { get; }

        /// <summary>ファイル名入力欄のラベル</summary>
        public ReactivePropertySlim<string> FileNameLabel { get; }

        /// <summary>追加の選択場所</summary>
        public ReactivePropertySlim<string> AdditionalPlaces { get; }
        #endregion

        #region ダイアログ結果
        /// <summary>選択時のフィルタインデックス</summary>
        public ReadOnlyReactivePropertySlim<uint?> ResultFilterIndex { get; }

        /// <summary>ダイアログの選択結果</summary>
        public ReadOnlyReactivePropertySlim<string> ResultItems { get; }
        #endregion
    }

    /// <summary>
    /// 上のVMに限定的な補助処理拡張メソッド
    /// </summary>
    internal static class ShellFileDialogHelper
    {
        /// <summary>
        /// stringのReactivePropertyから空でない場合のみ値を得る
        /// </summary>
        /// <param name="self">値を取り出すReactiveProperty</param>
        /// <returns>プロパティ値がnullまたは空ならば null。それ以外はプロパティ値。</returns>
        public static string ValueIfNotEmpty(this IReadOnlyReactiveProperty<string> self)
        {
            var value = self.Value;
            return string.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        /// フィルタパターンリスト文字列からファイルオープンダイアログパラメータのFiltersを設定する。
        /// </summary>
        /// <param name="self">設定対象のパラメータ</param>
        /// <param name="filtersText">フィルタパターンリスト文字列</param>
        public static void SetFiltersFrom(this ShellOpenFileDialogParameter self, string filtersText)
        {
            setFilters(self.Filters, filtersText);
        }

        /// <summary>
        /// パスリスト文字列からファイルオープンダイアログパラメータのAdditionalPlacesを設定する。
        /// </summary>
        /// <param name="self">設定対象のパラメータ</param>
        /// <param name="placesText">パスリスト文字列</param>
        public static void SetAdditionalPlaces(this ShellOpenFileDialogParameter self, string placesText)
        {
            setPlaces(self.AdditionalPlaces, placesText);
        }

        /// <summary>
        /// フィルタパターンリスト文字列からファイル保存ダイアログパラメータのFiltersを設定する。
        /// </summary>
        /// <param name="self">設定対象のパラメータ</param>
        /// <param name="filtersText">フィルタパターンリスト文字列</param>
        public static void SetFiltersFrom(this ShellSaveFileDialogParameter self, string filtersText)
        {
            setFilters(self.Filters, filtersText);
        }

        /// <summary>
        /// パスリスト文字列からファイル保存ダイアログパラメータのAdditionalPlacesを設定する。
        /// </summary>
        /// <param name="self">設定対象のパラメータ</param>
        /// <param name="placesText">パスリスト文字列</param>
        public static void SetAdditionalPlaces(this ShellSaveFileDialogParameter self, string placesText)
        {
            setPlaces(self.AdditionalPlaces, placesText);
        }

        /// <summary>
        /// フィルタパターンリスト文字列をパースしてファイルフィルタデータをリストに格納する。
        /// </summary>
        /// <param name="dlgFilters">格納先リスト</param>
        /// <param name="filtersText">フィルタパターンリスト文字列</param>
        private static void setFilters(IList<ShellFileDialogFilter> dlgFilters, string filtersText)
        {
            // 既存のフィルタ設定をクリア
            dlgFilters.Clear();

            // パースしてフィルタ設定を作成
            // 各行が1つのフィルタを表すものとして、'|'で表示名とファイルパターンを区切るものとする。
            var filters = (filtersText ?? string.Empty).Split('\r', '\n')
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(f => f.Split('|', 2))
                .Select(f => new ShellFileDialogFilter(f.First(), f.Last()))
                ;
            foreach (var filter in filters)
            {
                dlgFilters.Add(filter);
            }
        }

        /// <summary>
        /// パスリスト文字列をパースして追加場所データをリストに格納する。
        /// </summary>
        /// <param name="dlgPlaces">格納先リスト</param>
        /// <param name="placesText">パスリスト文字列</param>
        private static void setPlaces(IList<ShellFileDialogPlace> dlgPlaces, string placesText)
        {
            // 既存の追加場所設定をクリア
            dlgPlaces.Clear();

            // パースして追加場所設定を作成
            // 各行がファイルパスを表すものとする
            var places = (placesText ?? string.Empty).Split('\r', '\n')
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(f => new ShellFileDialogPlace(f, ShellFileDialogPlaceOrder.Botton))
                ;
            foreach (var place in places)
            {
                dlgPlaces.Add(place);
            }
        }
    }
}
