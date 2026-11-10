using System;
using System.Collections.Generic;
using System.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Automation;

namespace Tetris
{
    // Hier entsteht das Spiel. Diese Klasse kennt weder Fenster noch Farben oder Tasten.
    public class Game
    {
        public const int Columns = 10;
        public const int Rows = 20;
        public const int PrewiewRows = 4;
        public const int PreviewCols = 4;
        public const int ExtraRows = 4;
        public const int ExtraCols = 4;

        public int[,] NextPiece { get; private set; } = new int[0, 0];
        public int[,] Board { get; private set; } = new int[Rows, Columns];
        public int[,] ActivePiece { get; private set; } = new int[0, 0];
        public int PiecesSpawned { get; private set; } = 0;
        private int lastClearedLines = -1;
        public int MaxCombo { get; private set; } = 5;
        public int ActiveRow { get; private set; }
        public int ActiveColumn { get; private set; }
        public int Combo { get; private set; } = 0;

        public int Score { get; private set; } = 0;
        public int LinesCleared { get; private set; } = 0;
        public bool IsGameOver { get; private set; } = false;
        public int[,] ExtraPieace { get; private set; } = new int[0, 0];
        public int timereduce { get; private set; }

        private const int BaseScore1 = 100;
        private const int BaseScore2 = 300;
        private const int BaseScore3 = 500;
        private const int BaseScore4 = 800;

        private List<int[,]> KnownPieces = new List<int[,]>
        {
            new int[,]
            {
                { 0, 3, 0},
                { 3, 3, 3},
            },
            new int[,]
            {
                { 2, 2},
                { 2, 2},
            },
            new int[,]
            {
                { 6, 6, 0},
                { 0, 6, 6},
            },
            new int[,]
            {
                { 0, 8, 8},
                { 8, 8, 0},
            },
            new int[,]
            {
                { 12, 0, 0},
                { 12, 12, 12},
            },
            new int[,]
            {
                { 0, 0, 9},
                { 9, 9, 9},
            },
            new int[,]
            {
                { 5, 5, 5, 5},
            },
        };

        private Random random = new Random();

        public Game()
        {
            Reset();
        }

        private bool rotateLeft = false;

        public void ToggleRotationDirection()
        {
            rotateLeft = !rotateLeft;
        }

        private bool hasSwapped = false;

        public void SwapPiece()
        {
            if (IsGameOver || hasSwapped) return;

            if (ExtraPieace == null || ExtraPieace.GetLength(0) == 0)
            {
                ExtraPieace = ActivePiece;
                SpawnPiece();
            }
            else
            {
                int[,] temp = ActivePiece;
                ActivePiece = ExtraPieace;
                ExtraPieace = temp;

                ActiveRow = 0;
                ActiveColumn = (Columns - ActivePiece.GetLength(1)) / 2;

                if (!CanPlacePiece(ActivePiece, ActiveRow, ActiveColumn))
                {
                    IsGameOver = true;
                }
            }

            hasSwapped = true;
        }

        public void Reset()
        {
            Board = new int[Rows, Columns];
            LinesCleared = 0;
            Score = 0;
            Combo = 0;
            lastClearedLines = -1;
            IsGameOver = false;
            hasSwapped = false;
            PiecesSpawned = 1;
            int randomSwap = random.Next(KnownPieces.Count);
            ExtraPieace = KnownPieces[randomSwap];

            int randomIndex = random.Next(KnownPieces.Count);
            ActivePiece = KnownPieces[randomIndex];

            int nextIndex = random.Next(KnownPieces.Count);
            NextPiece = KnownPieces[nextIndex];

            ActiveRow = 0;
            ActiveColumn = (Columns - ActivePiece.GetLength(1)) / 2;
        }

        public void SpawnPiece()
        {
            ActivePiece = NextPiece;

            int nextIndex = random.Next(KnownPieces.Count);
            NextPiece = KnownPieces[nextIndex];

            ActiveRow = 0;
            ActiveColumn = (Columns - ActivePiece.GetLength(1)) / 2;
            hasSwapped = false;
            PiecesSpawned++;

            if (!CanPlacePiece(ActivePiece, ActiveRow, ActiveColumn))
            {
                IsGameOver = true;
            }
        }

        public int GetDroppedPieceRow()
        {
            int row = ActiveRow;
            while (CanPlacePiece(ActivePiece, row + 1, ActiveColumn))
            {
                row++;
            }
            return row;
        }

        public void DropPiece()
        {
            ActiveRow = GetDroppedPieceRow();
            Tick();
        }

        public bool TryMove(int columnChange, int rowChange)
        {
            int targetrow = ActiveRow + rowChange;
            int targetcol = ActiveColumn + columnChange;

            if (CanPlacePiece(ActivePiece, targetrow, targetcol))
            {
                ActiveRow = targetrow;
                ActiveColumn = targetcol;
                return true;
            }

            return false;
        }

        public bool CanPlacePiece(int[,] piece, int targetRow, int targetColumn)
        {
            for (int i = 0; i < piece.GetLength(0); i++)
            {
                for (int j = 0; j < piece.GetLength(1); j++)
                {
                    if (piece[i, j] == 0)
                    {
                        continue;
                    }
                    int currentBoardRow = targetRow + i;
                    int currentBoardCol = targetColumn + j;

                    if (currentBoardRow < 0 || currentBoardRow >= Rows || currentBoardCol < 0 || currentBoardCol >= Columns)
                    {
                        return false;
                    }

                    if (Board[currentBoardRow, currentBoardCol] > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Tick()
        {
            if (TryMove(0, 1))
                return;
            LockPiece();
            ClearFullRows();
            SpawnPiece();
        }

        public void LockPiece()
        {
            for (int i = 0; i < ActivePiece.GetLength(0); i++)
            {
                for (int j = 0; j < ActivePiece.GetLength(1); j++)
                {
                    if (ActivePiece[i, j] != 0)
                    {
                        int currentRow = ActiveRow + i;
                        int currentCol = ActiveColumn + j;

                        Board[currentRow, currentCol] = ActivePiece[i, j];
                    }
                }
            }
        }

        public void RotatePiece()
        {
            int rows = ActivePiece.GetLength(0);
            int cols = ActivePiece.GetLength(1);
            int[,] rotatedPiece = new int[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (rotateLeft == false)
                    {
                        rotatedPiece[j, rows - 1 - i] = ActivePiece[i, j];
                    }
                    else
                    {
                        rotatedPiece[cols - 1 - j, i] = ActivePiece[i, j];
                    }
                }
            }

            if (CanPlacePiece(rotatedPiece, ActiveRow, ActiveColumn))
            {
                ActivePiece = rotatedPiece;
            }
        }

        public void ClearFullRows()
        {
            int linesclearedThisTurn = 0;
            for (int row = 0; row < Rows; row++)
            {
                bool isFull = true;
                for (int col = 0; col < Columns; col++)
                {
                    if (Board[row, col] == 0)
                    {
                        isFull = false;
                        break;
                    }
                }
                if (!isFull)
                {
                    continue;
                }

                for (int col = 0; col < Columns; col++)
                {
                    Board[row, col] = 0;
                }
                linesclearedThisTurn++;
                LinesCleared++;

                for (int r = row; r > 0; r--)
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        Board[r, col] = Board[r - 1, col];
                    }
                }
            }

            if (linesclearedThisTurn > 0)
            {
                if (lastClearedLines == linesclearedThisTurn)
                {
                    Combo++;
                }
                else
                {
                    Combo = 1;
                }

                if (Combo > MaxCombo)
                {
                    Combo = MaxCombo;
                }

                int addedScore = 0;
                switch (linesclearedThisTurn)
                {
                    case 1: addedScore = BaseScore1 * Combo; break;
                    case 2: addedScore = BaseScore2 * Combo; break;
                    case 3: addedScore = BaseScore3 * Combo; break;
                    case 4: addedScore = BaseScore4 * Combo; break;
                    default: addedScore = linesclearedThisTurn * 200 * Combo; break;
                }

                Score += addedScore;
                lastClearedLines = linesclearedThisTurn;
            }
            else
            {
                Combo = 0;
                lastClearedLines = -1;
            }
        }
    }
}