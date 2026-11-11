using System.Windows;

namespace Tetris
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Startfenster als erstes anzeigen
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
        }
    }
}