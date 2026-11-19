# AI Chat Studio (WPF + C#)

C# と WPF (Windows Presentation Foundation) を使用して構築された、モダンで直感的なAIチャットデスクトップアプリケーションです。

![.NET 8](https://img.shields.io/badge/.NET-8.0--windows-blue)
![WPF](https://img.shields.io/badge/UI-WPF%20XAML-blueviolet)
![MVVM](https://img.shields.io/badge/Architecture-MVVM-green)

---

## 🌟 主な特徴

- **モダンで美しいUI / UX**
  - チャット吹き出し（ユーザー / AI の左右分割・スタイリッシュな角丸デザイン）
  - 自動最下部スクロール
  - 「AIが考え中...」のステータスインジケーター表示
  - `Enter` キーで即座に送信、`Shift + Enter` で改行入力
  - コピーしやすいメッセージテキスト表示
  - 会話履歴のクリア機能
- **複数プロバイダー対応の柔軟なAI接続**
  - **モックAIモード (初期状態)**: APIキーがなくてもすぐに動作確認・対話を試せます。
  - **OpenAI (ChatGPT)**: `gpt-4o`, `gpt-4o-mini` などの最新モデルに対応。
  - **Google Gemini**: `gemini-1.5-flash`, `gemini-1.5-pro` に対応。
  - **Ollama / ローカルLLM**: `localhost:11434` や OpenAI 互換のカスタムエンドポイントに対応。
- **洗練されたMVVM設計**
  - Microsoft公式の `CommunityToolkit.Mvvm` を採用
  - `[ObservableProperty]`, `[RelayCommand]` によるクリーンで保守性の高いコード
  - 非同期キャンセル（回答生成の中止ボタン）対応

---

## 📁 ディレクトリ構造

```
WRFPractice/
├── WRFPractice.sln                  # Visual Studio ソリューションファイル
├── WRFPractice/
│   ├── WRFPractice.csproj           # .NET 8 WPF プロジェクト定義
│   ├── App.xaml / App.xaml.cs       # アプリケーションエントリ・共通リソース
│   ├── Models/                      # データモデル
│   │   ├── ChatMessage.cs           # メッセージデータ
│   │   ├── MessageRole.cs           # ユーザー/アシスタント区分
│   │   ├── AiProvider.cs            # プロバイダー種別Enum
│   │   └── AppSettings.cs           # 設定保存・ロード (JSON)
│   ├── Services/                    # AI通信・ロジック層
│   │   ├── IAiService.cs            # AIサービスクライアント共通インターフェース
│   │   ├── MockAiService.cs         # オフライン動作検証用モック
│   │   ├── OpenAiCompatibleService.cs # OpenAI / Ollama 互換API通信
│   │   ├── GeminiAiService.cs       # Google Gemini REST API通信
│   │   └── AiServiceFactory.cs      # プロバイダー切り替えファクトリ
│   ├── ViewModels/                  # MVVM ビューモデル
│   │   ├── MainViewModel.cs         # チャット画面のメインロジック
│   │   ├── ChatMessageViewModel.cs  # メッセージ表示用ViewModel
│   │   └── SettingsViewModel.cs     # 設定画面ViewModel
│   ├── Views/                       # XAML 画面
│   │   ├── MainWindow.xaml (.cs)    # メインチャット画面
│   │   └── SettingsWindow.xaml (.cs)# AI・APIキー設定画面
│   ├── Converters/                  # XAML バインディング用コンバーター
│   │   ├── BooleanToVisibilityConverter.cs
│   │   ├── InverseBooleanConverter.cs
│   │   └── RoleConverters.cs
│   └── Styles/                      # デザイン・テーマスタイル
│       ├── Colors.xaml              # カラーパレット定義
│       └── ChatStyles.xaml          # ボタン・入力欄のスタイル
└── README.md
```

---

## 🚀 実行方法 (Windows 環境)

WPF は Windows 専用のUIフレームワークのため、Windows PCで実行してください。

### 前提条件
- **.NET 8.0 SDK** (または .NET 9.0 SDK)
- **Visual Studio 2022** (「.NET デスクトップ開発」ワークロードにチェック) または **Visual Studio Code** (C# Dev Kit 拡張機能)

### 起動手順

#### 方法1: Visual Studio を使用する場合
1. リポジトリをクローンまたはダウンロードします。
2. `WRFPractice.sln` を Visual Studio で開きます。
3. `F5` キーを押すか、緑色の「開始」ボタンをクリックして実行します。

#### 方法2: コマンドライン (CLI) を使用する場合
```bash
# プロジェクトフォルダへ移動
cd WRFPractice/WRFPractice

# 依存パッケージの復元とビルド・起動
dotnet run
```

---

## ⚙️ AI APIキーの設定方法

1. アプリを起動後、右上の **「⚙ 設定」** ボタンをクリックします。
2. 利用したいプロバイダー（OpenAI / Google Gemini / Ollama 等）を選択します。
3. お手持ちの API キーを入力し、**「保存」** をクリックします。
4. 設定内容は自動的にローカルの `%AppData%\WRFPractice\settings.json` に安全に保存され、次回起動時も保持されます。

---

## 🛠 カスタマイズ・拡張

- **新しいAIプロバイダーの追加**: `IAiService` を実装した新しいサービスクラスを作成し、`AiServiceFactory.cs` に追加するだけで拡張できます。
- **UIテーマの変更**: `Styles/Colors.xaml` 内のカラーコード（`PrimaryColor`, `WindowBackgroundColor` など）を変更することで、簡単にダークモード化やオリジナルカラーに変更可能です。
