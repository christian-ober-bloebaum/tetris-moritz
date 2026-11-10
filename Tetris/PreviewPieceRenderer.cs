using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Tetris
{
    public class PreviewPieceRenderer
    {
        private readonly Border[,] cells = new Border[Game.PrewiewRows, Game.PreviewCols];
        private readonly Brush emptyCellBrush = new SolidColorBrush(Color.FromRgb(27, 38, 56));

        public PreviewPieceRenderer(UniformGrid previewPanel)
        {
            previewPanel.Rows = Game.PrewiewRows;
            previewPanel.Columns = Game.PreviewCols;
            previewPanel.Width = Game.PreviewCols * 28;
            previewPanel.Height = Game.PrewiewRows * 28;

            previewPanel.Children.Clear();
            for (int row = 0; row < Game.PrewiewRows; row++)
            {
                for (int column = 0; column < Game.PreviewCols; column++)
                {
                    Border cell = new Border();
                    cell.Margin = new Thickness(1);
                    cell.CornerRadius = new CornerRadius(2);
                    cell.Background = emptyCellBrush;
                    cells[row, column] = cell;
                    previewPanel.Children.Add(cell);
                }
            }
        }

        public void DrawPreview(int[,] nextPiece, BoardRenderer boardRenderer)
        {

            for (int row = 0; row < Game.PrewiewRows; row++)
            {
                for (int column = 0; column < Game.PreviewCols; column++)
                {
                    cells[row, column].Background = emptyCellBrush;
                }
            }

            if (nextPiece == null) return;

            int pieceRows = nextPiece.GetLength(0);
            int pieceCols = nextPiece.GetLength(1);

            // Zentrierungsberechnung für die 4x4 Box
            int startRow = (Game.PrewiewRows - pieceRows) / 2;
            int startCol = (Game.PreviewCols - pieceCols) / 2;

            for (int r = 0; r < pieceRows; r++)
            {
                for (int c = 0; c < pieceCols; c++)
                {
                    int color = nextPiece[r, c];
                    if (color != 0)
                    {
                        int targetR = startRow + r;
                        int targetC = startCol + c;

  
                        if (targetR >= 0 && targetR < Game.PrewiewRows && targetC >= 0 && targetC < Game.PreviewCols)
                        {
                            cells[targetR, targetC].Background = boardRenderer.GetBrush(color, 1.0);
                        }
                    }
                }
            }
        }
    }
}