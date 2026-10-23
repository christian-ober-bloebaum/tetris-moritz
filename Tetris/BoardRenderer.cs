using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Tetris
{
    // Vorbereitete Zeichenhilfe: Die Spiellogik muss keine WPF-Elemente verändern.
    public class BoardRenderer
    {
        private readonly Border[,] cells = new Border[Game.Rows, Game.Columns];
        private readonly Brush emptyCellBrush = new SolidColorBrush(Color.FromRgb(27, 38, 56));

        public BoardRenderer(UniformGrid boardPanel)
        {
            boardPanel.Rows = Game.Rows;
            boardPanel.Columns = Game.Columns;
            boardPanel.Width = Game.Columns * 28;
            boardPanel.Height = Game.Rows * 28;

            for (int row = 0; row < Game.Rows; row++)
            {
                for (int column = 0; column < Game.Columns; column++)
                {
                    Border cell = new Border();
                    cell.Margin = new Thickness(1);
                    cell.CornerRadius = new CornerRadius(2);
                    cells[row, column] = cell;
                    boardPanel.Children.Add(cell);
                }
            }
        }

        public void Draw(Game game)
        {
            for (int row = 0; row < Game.Rows; row++)
            {
                for (int column = 0; column < Game.Columns; column++)
                {
                    SetCellColor(row, column, game.Board[row, column]);
                }
            }
            if (!game.IsGameOver)
            {
                int ghostRow = game.GetDroppedPieceRow();
                for (int row = 0; row < game.ActivePiece.GetLength(0); row++)
                {
                    for (int column = 0; column < game.ActivePiece.GetLength(1); column++)
                    {
                        int color = game.ActivePiece[row, column];
                        if (color != 0)
                        {
                            SetCellColor(ghostRow + row, game.ActiveColumn + column, color, 0.35);
                        }
                    }
                }
            }
            for (int row = 0; row < game.ActivePiece.GetLength(0); row++)
            {
                for (int column = 0; column < game.ActivePiece.GetLength(1); column++)
                {
                    int color = game.ActivePiece[row, column];
                    if (color != 0)
                    {
                        SetCellColor(game.ActiveRow + row, game.ActiveColumn + column, color);
                    }
                }
            }
        }

        private void SetCellColor(int row, int column, int color, double opacity = 1.0)
        {
            if (row < 0 || row >= Game.Rows || column < 0 || column >= Game.Columns)
            {
                return;
            }

            var brush = GetBrush(color, opacity);
            cells[row, column].Background = brush;
        }

        private Brush GetBrush(int color, double opacity)
        {
            Brush b = emptyCellBrush;
            switch (color)
            {
                case 1: b = Brushes.Turquoise; break;
                case 2: b = Brushes.Gold; break;
                case 3: b = Brushes.MediumPurple; break;
                case 4: b = Brushes.Coral; break;
                case 5: b = Brushes.CornflowerBlue; break;
                case 6: b = Brushes.LightGreen; break;
                case 7: b = Brushes.HotPink; break;
                case 8: b = Brushes.Crimson; break;       // Ein kräftiges Dunkelrot
                case 9: b = Brushes.DarkOrange; break;    // Ein sattes Orange
                case 10: b = Brushes.Teal; break;        // Ein dunkles Blaugrün
                case 11: b = Brushes.Orchid; break;      // Ein schöner Lilaton
                case 12: b = Brushes.DeepSkyBlue; break; // Ein leuchtendes Hellblau
                case 13: b = Brushes.LimeGreen; break;   // Ein knalliges Apfelgrün               
            }
            if(opacity != b.Opacity)
            {
                b = ((SolidColorBrush)b).Clone();
                b.Opacity = opacity;
            }
            return b;
        }
    }
}
