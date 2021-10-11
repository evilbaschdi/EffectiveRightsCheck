using System;
using System.IO;
using System.Windows;
using EffectiveRightsCheck.Core;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Browsers;
using EvilBaschdi.CoreExtended.Controls.About;
using MahApps.Metro.Controls;

namespace EffectiveRightsCheck.Wpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private string _initialDirectory;
        private readonly IRoundCorners _roundCorners;


        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();

            _roundCorners = new RoundCorners();
            IApplicationStyle style = new ApplicationStyle(_roundCorners, true);
            style.Run();

            Load();
        }

        private void Load()
        {
            CheckRights.IsEnabled = !string.IsNullOrWhiteSpace(_initialDirectory) && Directory.Exists(_initialDirectory);

            InitialDirectory.Text = _initialDirectory ?? string.Empty;
        }

        private void AboutWindowClick(object sender, RoutedEventArgs e)
        {
            var assembly = typeof(MainWindow).Assembly;
            IAboutContent aboutWindowContent = new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");

            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent, _roundCorners)
                              };

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