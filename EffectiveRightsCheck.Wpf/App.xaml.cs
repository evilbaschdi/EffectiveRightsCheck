using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EvilBaschdi.CoreExtended.Metro;

namespace EffectiveRightsCheck.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            var themeManagerHelper = new ThemeManagerHelper();
            themeManagerHelper.RegisterSystemColorTheme();

            base.OnStartup(e);
        }
    }
}
