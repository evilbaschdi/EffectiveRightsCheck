using System.IO;
using System.Windows;
using EffectiveRightsCheck.Core;
using EvilBaschdi.About.Core;
using EvilBaschdi.About.Core.Models;
using EvilBaschdi.About.Wpf;
using EvilBaschdi.Core;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Browsers;
using MahApps.Metro.Controls;

namespace EffectiveRightsCheck.Wpf;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class MainWindow : MetroWindow
{
    private string _initialDirectory;

    /// <inheritdoc />
    public MainWindow()
    {
        InitializeComponent();

        IApplicationStyle applicationStyle = new ApplicationStyle(true);
        applicationStyle.Run();

        Load();
    }

    private void Load()
    {
        CheckRights.SetCurrentValue(IsEnabledProperty, !string.IsNullOrWhiteSpace(_initialDirectory) && Directory.Exists(_initialDirectory));

        InitialDirectory.SetCurrentValue(System.Windows.Controls.TextBox.TextProperty, _initialDirectory ?? string.Empty);
    }

    private void AboutWindowClick(object sender, RoutedEventArgs e)
    {
        ICurrentAssembly currentAssembly = new CurrentAssembly();
        IAboutContent aboutContent = new AboutContent(currentAssembly);
        IAboutModel aboutModel = new AboutViewModel(aboutContent);
        var aboutWindow = new AboutWindow(aboutModel);

        aboutWindow.ShowDialog();
    }

    private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
    {
        if (!Directory.Exists(InitialDirectory.Text))
        {
            return;
        }

        _initialDirectory = InitialDirectory.Text;
        Load();
    }

    private void CheckRightsOnClick(object sender, RoutedEventArgs e)
    {
        var rights = FileSystemEffectiveRights.GetRights(UserName.Text, _initialDirectory);
        Result.SetCurrentValue(System.Windows.Controls.TextBox.TextProperty, rights.ToString());
    }

    private void BrowseClick(object sender, RoutedEventArgs e)
    {
        var browser = new ExplorerFolderBrowser
                      {
                          SelectedPath = _initialDirectory
                      };
        browser.ShowDialog();
        _initialDirectory = browser.SelectedPath;
        Load();
    }
}