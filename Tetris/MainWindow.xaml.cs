using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tetris
{
    // Verbindet Tasten, Timer und Spiel. Die Regeln werden in Game.cs implementiert.
    public partial class MainWindow : Window
    {
        private readonly Game game;
        private readonly BoardRenderer renderer;
        private readonly DispatcherTimer timer;
        private int tickCount;

        public MainWindow()
        {
            InitializeComponent();

            game = new Game();
            renderer = new BoardRenderer(BoardPanel);

            // Der WPF-Timer läuft auf dem UI-Thread. Hier sind keine Hintergrundthreads nötig.
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;

            RefreshView();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            AdvanceOneStep();
        }

        private void AdvanceOneStep()
        {
            if (game.IsGameOver)
            {
                RefreshView();
                return;
            }

            game.Tick();
            tickCount++;
            RefreshView();
        }

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleTimer();
        }

        private void ToggleTimer()
        {
            if (game.IsGameOver)
            {
                StatusText.Text = "Game Over. Starte mit R eine neue Runde.";
                return;
            }

            if (timer.IsEnabled)
            {
                PauseGame();
            }
            else
            {
                timer.Start();
                StartPauseButton.Content = "Pause";
                StepButton.IsEnabled = false;
                StatusText.Text = "Spieltakt läuft. Die Spielregeln ergänzt du in Game.cs.";
            }
        }

        private void PauseGame()
        {
            timer.Stop();
            StartPauseButton.Content = "Weiter";
            StepButton.IsEnabled = true;
            StatusText.Text = "Pausiert. Mit Einzelschritt kannst du einen Spieltakt untersuchen.";
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void ResetGame()
        {
            PauseGame();
            game.Reset();
            tickCount = 0;
            StartPauseButton.Content = "Start";
            InputText.Text = "Letzte Eingabe: –";
            StatusText.Text = "Zurückgesetzt. Bereit für die nächste Runde.";
            RefreshView();
        }

        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            AdvanceOneStep();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // PreviewKeyDown erreicht uns auch dann, wenn ein Button den Fokus hat.
            if (e.Key == Key.Space || e.Key == Key.R)
            {
                e.Handled = true;
                if (e.IsRepeat)
                {
                    return;
                }

                if (e.Key == Key.Space)
                {
                    ToggleTimer();
                }
                else
                {
                    ResetGame();
                }

                return;
            }

            if (e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Down && e.Key != Key.Up)
            {
                return;
            }

            e.Handled = true;
            InputText.Text = "Letzte Eingabe: " + e.Key;

            if (!timer.IsEnabled || game.IsGameOver)
            {
                StatusText.Text = "Für die Steuerung zuerst Start / Weiter drücken.";
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    game.TryMove(-1, 0);
                    break;
                case Key.Right:
                    game.TryMove(1, 0);
                    break;
                case Key.Down:
                    AdvanceOneStep();
                    break;
                case Key.Up:
                    game.RotatePiece();
                    break;
            }

            RefreshView();
        }

        private void RefreshView()
        {
            renderer.Draw(game);
            TickText.Text = "Spieltakte: " + tickCount;
            LinesText.Text = "Entfernte Reihen: " + game.LinesCleared;

            if (game.IsGameOver)
            {
                timer.Stop();
                StartPauseButton.IsEnabled = false;
                StepButton.IsEnabled = false;
                StatusText.Text = "Game Over. Mit R oder Neu starten beginnt eine neue Runde.";
            }
            else
            {
                StartPauseButton.IsEnabled = true;
            }
        }

        private void Window_Deactivated(object? sender, EventArgs e)
        {
            // Bei einem Wechsel zu Visual Studio läuft das Spiel nicht unbemerkt weiter.
            if (timer.IsEnabled)
            {
                PauseGame();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            timer.Stop();
            base.OnClosed(e);
        }
    }
}
