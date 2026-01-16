using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AlfredNexus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Nếu muốn mặc định Dark mode: SetDarkMode(true);
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                SetDarkMode(toggle.IsChecked == true);
            }
        }

        private void SetDarkMode(bool isDark)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }
    }
}