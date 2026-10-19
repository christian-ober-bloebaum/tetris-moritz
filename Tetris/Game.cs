using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Windows.Input;

namespace Tetris
{
    // Hier entsteht das Spiel. Diese Klasse kennt weder Fenster noch Farben oder Tasten.
    public class Game
    {
        public const int Columns = 10;
        public const int Rows = 20;

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

        public Game()
        {
            Reset();
        }

        public void Reset()
        {
            Board = new int[Rows, Columns];
            LinesCleared = 0;
            IsGameOver = false;
            SpawnPiece();
        }

        public void SpawnPiece()
        {
            // Ein Quadrat ist der erste Übungsstein. Weitere Formen folgen später.
            ActivePiece = new int[,]
            {
                { 3, 3 },
                { 3, 3 }
            };

            ActiveRow = 0;
            ActiveColumn = (Columns - ActivePiece.GetLength(1)) / 2;

            // TODO 6: Prüfe beim Erzeugen eines Steins, ob das Spiel zu Ende ist.
        }

        public bool TryMove(int columnChange, int rowChange)

        {
            
          
            // TODO 1: Berechne die neue Position und prüfe sie mit CanPlacePiece.
            // Ändere die Position nur bei einem erlaubten Zug und liefere dann true.
            // Bei einem ungültigen Zug bleiben Position und Spielfeld unverändert.

            return false;
        }

        public bool CanPlacePiece(int[,] piece, int targetRow, int targetColumn)
        {
            for(targetRow = 0 ; targetRow < piece.GetLength(0);)
            { if (targetRow != 0)
                    return false;
                else
                    return true;
            }            

            for (targetColumn = 0; targetColumn < piece.GetLength(1);) ;
            {  if (targetColumn != 0)
                    return false;
                else
                    return true;
                        }


            // TODO 1: Prüfe jede belegte Zelle der Steinmatrix.
            // Die Zielzelle muss im Spielfeld liegen; leere Matrixzellen zählen nicht.
            // TODO 3: Prüfe zusätzlich, ob Board an der Zielzelle bereits belegt ist.
            // Prüfe die Grenzen, BEVOR du auf Board zugreifst.
            return false;
        }

        public void Tick()
        {
            // Ein Tick entspricht genau einem Schritt des Spiels.
            // TODO 2: Versuche, den Stein um eine Zeile nach unten zu bewegen.
            // TODO 3: Falls das nicht geht: festsetzen, Reihen entfernen, neuen Stein erzeugen.
            // TODO 6: Nach Game Over darf sich der Spielzustand nicht mehr ändern.
        }

        public void LockPiece()
        {
            // TODO 3: Übertrage belegte Zellen des aktiven Steins nach Board.
        }

        public void RotatePiece()
        {
            // TODO 4: Erzeuge eine um 90 Grad gedrehte Matrix.
            // Übernimm sie nur, wenn CanPlacePiece die gedrehte Form erlaubt.
            // Ein Quadrat darf bei dieser einfachen Variante unverändert bleiben.
        }

        public void ClearFullRows()
        {
            // TODO 5: Entferne volle Reihen und verschiebe darüberliegende Reihen nach unten.
            // Fülle oben mit leeren Zellen auf und erhöhe LinesCleared.
            // Prüfe auch mehrere unmittelbar aufeinanderfolgende volle Reihen.
        }
    }
}
