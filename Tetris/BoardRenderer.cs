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

        // Koordinaten außerhalb des Spielfelds werden beim Zeichnen ignoriert.
        // Die Kollisionsprüfung gehört trotzdem in Game.CanPlacePiece.
        private void SetCellColor(int row, int column, int color)
        {
            if (row < 0 || row >= Game.Rows || column < 0 || column >= Game.Columns)
            {
                return;
            }

            cells[row, column].Background = GetBrush(color);
        }

        private Brush GetBrush(int color)
        {
            switch (color)
            {
                case 1: return Brushes.Turquoise;
                case 2: return Brushes.Gold;
                case 3: return Brushes.MediumPurple;
                case 4: return Brushes.Coral;
                case 5: return Brushes.CornflowerBlue;
                case 6: return Brushes.LightGreen;
                case 7: return Brushes.HotPink;
                default: return emptyCellBrush;
            }
        }
    }
}
