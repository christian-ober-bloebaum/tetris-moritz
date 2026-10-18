# Tetris – ein C#-Lernprojekt

Ein lauffähiges WPF-Grundgerüst für die erste Praktikumswoche. Fenster, Spielfeld,
Darstellung, Tastatureingaben, Pause, Neustart und ein Spieltakt sind vorbereitet.
Die Spielregeln entwickelt der Praktikant schrittweise selbst.

**Der Ausgangsstand ist noch kein spielbares Tetris.** Ein türkisfarbenes Quadrat
steht oben im Feld. Start lässt zunächst nur den Taktzähler laufen. Pfeiltasten
werden erkannt, bewegen den Stein aber noch nicht. Das ist beabsichtigt.

## Öffnen und starten

Voraussetzungen: Windows und das .NET 10 SDK. Für Visual Studio eignet sich
Visual Studio 2026 mit der Workload „.NET-Desktopentwicklung“.
Es werden keine zusätzlichen NuGet-Pakete benötigt.

1. `Tetris.sln` in Visual Studio öffnen.
2. Das Projekt `Tetris` starten, beispielsweise mit F5.
3. Im Fenster auf **Start** klicken. Den Taktzähler und die Anzeige der letzten Eingabe beobachten.
4. `Tetris/Game.cs` öffnen und mit [Aufgabe 1](docs/Aufgaben.md) beginnen.

Alternativ im Terminal im Ordner dieser README:

```powershell
dotnet build Tetris.sln
dotnet run --project Tetris/Tetris.csproj
```

## Wo steht was?

| Datei | Zweck | Für den Einstieg |
| --- | --- | --- |
| `Tetris/Game.cs` | Spielfeld, Stein und offene Spielregeln | Hier arbeiten |
| `Tetris/MainWindow.xaml` | Fenster, Beschriftungen und Buttons | Kleine Änderungen ausprobieren |
| `Tetris/MainWindow.xaml.cs` | Leitet Tasten und Timer an das Spiel weiter | Gemeinsam lesen |
| `Tetris/BoardRenderer.cs` | Zeichnet das Spielfeld und den aktiven Stein | Ist vorbereitet |
| `Tetris/App.xaml` | Legt das Startfenster fest | Kann zunächst so bleiben |
| [docs/Aufgaben.md](docs/Aufgaben.md) | Etappen, Hinweise und Prüffälle | Aufgabenblatt |

## Die wichtigsten Vereinbarungen

- Das Spielfeld hat 20 Zeilen und 10 Spalten. Arrays beginnen bei Index 0.
- Matrizen werden immer als `[Zeile, Spalte]` gelesen, auch bei den Steinformen.
- Zeilen wachsen nach unten, Spalten nach rechts.
- `0` ist eine leere Zelle. `1` bis `7` sind belegte Zellen in verschiedenen Farben.
- `Board` enthält festgesetzte Steine. `ActivePiece` enthält den beweglichen Stein.
- `ActiveRow` und `ActiveColumn` beschreiben dessen linke obere Ecke.
- Ein Tick ist ein Spielschritt. Der Timer ruft ihn alle 500 Millisekunden auf.
- Bei Pause ändern die Pfeiltasten das Spiel nicht. **Einzelschritt** führt bewusst
  genau einen Tick aus, damit man ihn mit dem Debugger untersuchen kann.
- Beim Verlassen des Fensters wird automatisch pausiert.

Der Code verwendet C# 14 und .NET 10, bleibt aber bei klassischen Klassen,
expliziten Typen, Schleifen und verständlichen Methoden. Kommentare und Aufgaben
sind deutsch, Bezeichner englisch. Zusätzliche Architektur-Frameworks sind für
dieses Projekt nicht nötig.

Die Sprachversion ist ausdrücklich auf `14.0` gesetzt; `global.json` wählt ein
installiertes stabiles .NET-10-SDK. Hintergrund:
[C# 14](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14),
[Sprachversion konfigurieren](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version).
