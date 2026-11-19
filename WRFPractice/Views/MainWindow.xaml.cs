using System.Windows;
using System.Windows.Input;
using WRFPractice.Models;
using WRFPractice.ViewModels;

namespace WRFPractice.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        _viewModel = (MainViewModel)DataContext;
        _viewModel.RequestScrollToBottom += OnRequestScrollToBottom;
        _viewModel.RequestOpenSettings += OnRequestOpenSettings;

        Loaded += (_, _) => MessageInputBox.Focus();
    }

    private void OnRequestScrollToBottom()
    {
        Dispatcher.InvokeAsync(() =>
        {
            ChatScrollViewer.ScrollToEnd();
        });
    }

    private void OnRequestOpenSettings()
    {
        var settings = AppSettings.Load();
        var settingsWindow = new SettingsWindow(settings)
        {
            Owner = this
        };
        settingsWindow.ShowDialog();
        _viewModel.RefreshSettings();
    }

    private void MessageInputBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Enterキーのみで送信、Shift+Enterで改行
        if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
        {
            e.Handled = true;
            if (_viewModel.SendMessageCommand.CanExecute(null))
            {
                _viewModel.SendMessageCommand.Execute(null);
            }
        }
    }
}
