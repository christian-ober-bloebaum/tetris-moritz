using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tetris
{

    public partial class MainWindow : Window
    {

        private Game game = null!;
        private BoardRenderer renderer = null!;
        private DispatcherTimer timer = null!;
        public int tickCount;
        private PreviewPieceRenderer previewRenderer = null!;
        public SwapPiece swapPieceRenderer = null!;
        public int TickCountdown = 500;

        private UserProfile currentAccount;
        private SaveData saveData = ProfileManager.LoadData();
        public MainWindow(UserProfile account)
        {
            InitializeComponent();

            currentAccount = account;
            StatusText.Text = $"Eingeloggt als: {currentAccount.Name} (Highscore: {currentAccount.Highscore})";

            InitGameComponents();
        }

        // 2. Der leere Standard-Konstruktor als Sicherheit (falls WPF ihn sucht)
        public MainWindow()
        {
            InitializeComponent();

            // Fallback: Wenn kein Account übergeben wurde, einen Gast erstellen
            currentAccount = new UserProfile { Name = "Gast_Direktstart", Highscore = 0, IsGuest = true };
            StatusText.Text = $"Eingeloggt als: {currentAccount.Name}";

            InitGameComponents();
        }

        // Hilfsmethode, damit sich der Code nicht doppelt
        private void InitGameComponents()
        {
            game = new Game();
            renderer = new BoardRenderer(BoardPanel);
            swapPieceRenderer = new SwapPiece(BlockSwitch);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(TickCountdown);
            timer.Tick += Timer_Tick;
            previewRenderer = new PreviewPieceRenderer(PreviewPanel);
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
            TickCountdown = 500;
        }

        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            AdvanceOneStep();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // PreviewKeyDown erreicht uns auch dann, wenn ein Button den Fokus hat.
            if (e.Key == Key.E || e.Key == Key.R)
            {
                e.Handled = true;
                if (e.IsRepeat)
                {
                    return;
                }

                if (e.Key == Key.E)
                {
                    ToggleTimer();
                }
                else
                {
                    ResetGame();
                }

                return;
            }

            if (e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Down && e.Key != Key.Up && e.Key != Key.Space && e.Key != Key.LeftShift && e.Key !=Key.C)
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
                case Key.Space:
                    game.DropPiece();

                    break;
                case Key.LeftShift:
                    game.ToggleRotationDirection();
                    break;
                case Key.C:
                    game.SwapPiece();
                    break;
            }   

            RefreshView();
        }
        private void RefreshView()
        {


            renderer.Draw(game);
            previewRenderer.DrawPreview(game.NextPiece, renderer);
            swapPieceRenderer.DrawPreview(game.ExtraPieace, renderer);
            TickText.Text = "Spieltakte: " + tickCount;
            LinesText.Text = "Entfernte Reihen: " + game.LinesCleared;
            ScoreTracker.Text = game.Score.ToString();
            int newInterval = Math.Max(150, 500 - ((game.PiecesSpawned - 1) * 10));
            timer.Interval = TimeSpan.FromMilliseconds(newInterval);
            if (game.IsGameOver)
            {
                timer.Stop();
                StartPauseButton.IsEnabled = false;
                StepButton.IsEnabled = false;

                // Highscore prüfen und speichern
                if (game.Score > currentAccount.Highscore)
                {
                    currentAccount.Highscore = game.Score;

                    // In der Speicherdatei aktualisieren
                    var saveData = ProfileManager.LoadData();
                    var accToUpdate = saveData.Accounts.Find(a => a.Name == currentAccount.Name);
                    if (accToUpdate != null)
                    {
                        accToUpdate.Highscore = currentAccount.Highscore;
                        ProfileManager.SaveDataToFile(saveData);
                    }

                    StatusText.Text = $"Neuer Highscore! {game.Score} Punkte!";
                }
                else
                {
                    StatusText.Text = "Game Over. Mit R oder Neu starten beginnt eine neue Runde.";
                }
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