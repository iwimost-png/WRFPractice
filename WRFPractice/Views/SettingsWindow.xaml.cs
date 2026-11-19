using System.Windows;
using WRFPractice.Models;
using WRFPractice.ViewModels;

namespace WRFPractice.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow(AppSettings settings)
    {
        InitializeComponent();

        var vm = new SettingsViewModel(settings);
        vm.RequestClose += () => Close();
        DataContext = vm;
    }
}
