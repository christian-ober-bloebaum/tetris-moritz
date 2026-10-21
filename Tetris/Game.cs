using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Input;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Collections.Generic;

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


            int randomIndex = random.Next(KnownPieces.Count);  
            ActivePiece = KnownPieces[randomIndex];
            ActiveRow = 0;
            ActiveColumn = (Columns - ActivePiece.GetLength(1)) / 2;

            if (!CanPlacePiece(ActivePiece, ActiveRow, ActiveColumn))
            { 
                IsGameOver = true; 
            }

            
            // TODO 6: Prüfe beim Erzeugen eines Steins, ob das Spiel zu Ende ist.
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
            
            SpawnPiece();

            // TODO 3: Falls das nicht geht: festsetzen, Reihen entfernen, neuen Stein erzeugen.
            // TODO 6: Nach Game Over darf sich der Spielzustand nicht mehr ändern.
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

                        Board[currentRow,currentCol] = ActivePiece[i, j];
                        
                            
                        
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
                    rotatedPiece[j, rows - 1 - i] = ActivePiece[i, j];
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
            // TODO 5: Entferne volle Reihen und verschiebe darüberliegende Reihen nach unten.
            // Fülle oben mit leeren Zellen auf und erhöhe LinesCleared.
            // Prüfe auch mehrere unmittelbar aufeinanderfolgende volle Reihen.
        }
    }
}
