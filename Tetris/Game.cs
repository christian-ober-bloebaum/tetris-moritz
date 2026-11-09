using System;
using System.Collections.Generic;
using System.Printing;
using System.Security.Cryptography.X509Certificates;

namespace Tetris
{
    // Hier entsteht das Spiel. Diese Klasse kennt weder Fenster noch Farben oder Tasten.
    public class Game
    {
        public const int Columns = 10;
        public const int Rows = 20;
        public const int PrewiewRows = 4;
        public const int PreviewCols = 4;

        public int[,] NextPiece { get; private set; } = new int[0, 0];
        // Zugriff immer mit [Zeile, Spalte]. Zeile 0 ist oben, Spalte 0 ist links.
        // 0 bedeutet leer. Zahlen von 1 bis 7 stehen für verschiedene Steinfarben.
        // Board enthält nur festgesetzte Steine. Der aktive Stein wird getrennt gehalten.
        public int[,] Board { get; private set; } = new int[Rows, Columns];
        public int[,] ActivePiece { get; private set; } = new int[0, 0];

        // Position der linken oberen Ecke der Steinmatrix auf dem Spielfeld.
        public int ActiveRow { get; private set; }
        public int ActiveColumn { get; private set; }

        public int LinesCleared { get; private set; }
        public bool IsGameOver { get; private set; }
        public int timereduce {  get; private set; }
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
        public void Reset()
        {
            Board = new int[Rows, Columns];
            LinesCleared = 0;
            IsGameOver = false;

            // Initialisiere den ersten und den nächsten Stein
            int randomIndex = random.Next(KnownPieces.Count);
            ActivePiece = KnownPieces[randomIndex];

            int nextIndex = random.Next(KnownPieces.Count);
            NextPiece = KnownPieces[nextIndex];

            ActiveRow = 0;
            ActiveColumn = (Columns - ActivePiece.GetLength(1)) / 2;
        }

        public void SpawnPiece()
        {
            // Der nächste Stein wird zum aktiven Stein
            ActivePiece = NextPiece;

            // Ein neuer "NextPiece" wird gewürfelt
            int nextIndex = random.Next(KnownPieces.Count);
            NextPiece = KnownPieces[nextIndex];

            ActiveRow = 0;
            ActiveColumn = (Columns - ActivePiece.GetLength(1)) / 2;

            if (!CanPlacePiece(ActivePiece, ActiveRow, ActiveColumn))
            {
                IsGameOver = true;
            }
        }

        public int GetDroppedPieceRow()
        {
            // ermittle, in welcher Zeile das aktuelle Piece landen würde - ähnlich wie DropPiece
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
            // TODO 1: Berechne die neue Position und prüfe sie mit CanPlacePiece.
            // Ändere die Position nur bei einem erlaubten Zug und liefere dann true.
            // Bei einem ungültigen Zug bleiben Position und Spielfeld unverändert.

            return false;
        }

        public bool CanPlacePiece(int[,] piece, int targetRow, int targetColumn)
        {
            for (int i = 0; i < piece.GetLength(0); i++)
            {
                for (int j = 0; j < piece.GetLength(1); j++)
                {
                    // TODO 1: Prüfe jede belegte Zelle der Steinmatrix.    
                    //leere Matrixzellen zählen nicht.
                    if (piece[i, j] == 0)
                    {
                        continue;
                    }
                    int currentBoardRow = targetRow + i;
                    int currentBoardCol = targetColumn + j;
                    // Die Zielzelle muss im Spielfeld liegen; 
                    if (currentBoardRow < 0 || currentBoardRow >= Rows || currentBoardCol < 0 || currentBoardCol >= Columns)
                    {
                          return false;
                    }
                    // TODO 3: Prüfe zusätzlich, ob Board an der Zielzelle bereits belegt ist.
                    // Prüfe die Grenzen, BEVOR du auf Board zugreifst.
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
            // Ein Tick entspricht genau einem Schritt des Spiels.
            // TODO 2: Versuche, den Stein um eine Zeile nach unten zu bewegen.
            if (TryMove(0, 1))
                return;
            LockPiece();
            ClearFullRows();
            SpawnPiece();
        }
        // TODO 3: Falls das nicht geht: festsetzen, Reihen entfernen, neuen Stein erzeugen      // TODO 6: Nach Game Over darf sich der Spielzustand nicht mehr ändern.
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
            // TODO 3: Übertrage belegte Zellen des aktiven Steins nach Board.
        }
        public void RotatePiece()
        {
            int rows = ActivePiece.GetLength(0);
            int cols = ActivePiece.GetLength(1);
            // Neue Matrix für den gedrehten Stein erstellen
            int[,] rotatedPiece = new int[cols, rows];
            // Matrix drehen
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (rotateLeft == false)
                    {
                        // rotate right
                        rotatedPiece[j, rows - 1 - i] = ActivePiece[i, j];
                    }
                    else
                    {
                        // rotate left
                        rotatedPiece[cols - 1 - j, i] = ActivePiece[i, j];
                    }
                }
            }
            // Erst jetzt die vollständige Matrix überprüfen
            if (CanPlacePiece(rotatedPiece, ActiveRow, ActiveColumn))
            {
                ActivePiece = rotatedPiece;
            }
        }
        // TODO 4: Erzeuge eine um 90 Grad gedrehte Matrix.
        // Übernimm sie nur, wenn CanPlacePiece die gedrehte Form erlaubt.
        // Ein Quadrat darf bei dieser einfachen Variante unverändert bleiben.
        public void ClearFullRows()
        {
            for (int row = 0; row < Rows; row++)
            {
                // Ist die Zeile voll?
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
                // Volle Zeile leeren
                for (int col = 0; col < Columns; col++)
                {
                    Board[row, col] = 0;
                }
                LinesCleared++;
                for (int r = row; r > 0; r--)
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        Board[r, col] = Board[r - 1, col];
                    }
                }
            }
        }
    }
}
