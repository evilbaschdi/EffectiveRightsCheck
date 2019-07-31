using System;
using System.IO;
using System.Windows;
using EffectiveRightsCheck.Core;
using EvilBaschdi.CoreExtended.Browsers;
using EvilBaschdi.CoreExtended.Metro;
using EvilBaschdi.CoreExtended.Mvvm;
using EvilBaschdi.CoreExtended.Mvvm.View;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using MahApps.Metro.Controls;

namespace EffectiveRightsCheck.Wpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IThemeManagerHelper _themeManagerHelper;
        private string _initialDirectory;


        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();
            _themeManagerHelper = new ThemeManagerHelper();
            IApplicationStyle applicationStyle = new ApplicationStyle(_themeManagerHelper);
            applicationStyle.Load(true);

            Load();
        }

        private void Load()
        {
            CheckRights.IsEnabled = !string.IsNullOrWhiteSpace(_initialDirectory) && Directory.Exists(_initialDirectory);

            InitialDirectory.Text = _initialDirectory;
        }

        private void AboutWindowClick(object sender, RoutedEventArgs e)
        {
            var assembly = typeof(MainWindow).Assembly;
            IAboutWindowContent aboutWindowContent = new AboutWindowContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");

            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent, _themeManagerHelper)
                              };

            aboutWindow.ShowDialog();
        }

        private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(InitialDirectory.Text))
            {
                _initialDirectory = InitialDirectory.Text;
                Load();
            }
        }

        private void CheckRightsOnClick(object sender, RoutedEventArgs e)
        {
            var rights = FileSystemEffectiveRights.GetRights(UserName.Text, _initialDirectory);
            Result.Text = rights.ToString();
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
}