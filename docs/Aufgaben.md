# Vom Quadrat zu Tetris

Arbeite in kleinen Schritten. Starte die Anwendung nach jeder Änderung und erkläre
kurz, was dein Code bewirken soll. Die nummerierten TODO-Kommentare in `Game.cs`
gehören zu den Aufgaben hier. Die Methoden sind zunächst absichtlich leer oder
liefern `false`; du ersetzt diese Platzhalter durch deine Lösung.

## 0 – Das Gerüst kennenlernen

Starte das Projekt. Ändere den Untertitel in `MainWindow.xaml`. Ändere dann in
`SpawnPiece` die Farbe des Quadrats, indem du die vier Einsen durch Zweien ersetzt.
Probiere Start, Pause, Einzelschritt und R aus. Der Stein bewegt sich noch nicht.

**Geschafft, wenn:** Du kannst zeigen, welche Datei die Oberfläche und welche den
Spielzustand beschreibt. Du kannst Zeile und Spalte einer Zelle benennen.

## 1 – Den Stein bewegen

Ergänze `CanPlacePiece` und `TryMove`. Zunächst prüfst du nur die Spielfeldgrenzen.
`TryMove(-1, 0)` bedeutet eine Spalte nach links; `TryMove(0, 1)` eine Zeile nach unten.

**Hinweise:** Berechne zuerst eine mögliche neue Position in lokalen Variablen.
Durchlaufe die Steinmatrix mit zwei Schleifen. `piece.GetLength(0)` ist die Anzahl
der Zeilen, `piece.GetLength(1)` die Anzahl der Spalten. Prüfe nur Zellen mit einem
Wert ungleich 0. Falls eine belegte Zelle nicht passt, liefere `false`. Wenn alle
passen, liefere `true`. Verwende diese Prüfung vor jeder Positionsänderung.

**Prüfen:** Links und rechts bewegen; beide Ränder erreichen; mehrfach gegen eine
Wand drücken. Der Stein darf weder verschwinden noch einen Fehler verursachen.
Die untere Grenze kannst du vorübergehend über `ActiveRow` in `SpawnPiece` prüfen.

## 2 – Fallen lassen

Ergänze `Tick`: Der Stein soll versuchen, eine Zeile nach unten zu rücken.

**Prüfen:** Start lässt den Stein fallen. Pause hält ihn an. Einzelschritt bewegt
ihn genau eine Zeile. Die Taste ↓ führt denselben Schritt aus. Am Boden bleibt er
zunächst stehen. Zurücksetzen bringt ihn wieder an den Anfang.

**Debugger:** Setze einen Haltepunkt in `Tick` und beobachte `ActiveRow`. Wenn das
Fenster beim Debuggen den Fokus verliert, pausiert das Gerüst automatisch.

## 3 – Steine stapeln

Ergänze `LockPiece`. Wenn ein Stein nicht weiter fallen kann, überträgst du seine
belegten Zellen in `Board` und erzeugst mit `SpawnPiece` einen neuen Stein.
Erweitere `CanPlacePiece`, sodass auch bereits belegte Spielfeldzellen einen Zug
verhindern. Rufe nach dem Festsetzen auch `ClearFullRows` auf; die Methode wird
erst in Aufgabe 5 ausgefüllt.

**Prüfen:** Zwei Quadrate stapeln sich. Seitliches Hineinschieben ist unmöglich.
Der neue Stein verändert die festgesetzten Zellen nicht. Nach R ist das Feld leer.

## 4 – Formen und Rotation

Beginne mit einer gut erkennbaren Form, zum Beispiel einem T:

```csharp
ActivePiece = new int[,]
{
    { 0, 3, 0 },
    { 3, 3, 3 }
};
```

Ergänze `RotatePiece`. Eine Matrix mit H Zeilen und W Spalten hat nach einer
Drehung W Zeilen und H Spalten. Überlege die Zuordnung zunächst auf Papier.
Verwende `CanPlacePiece`, bevor du die gedrehte Matrix übernimmst. Eine Drehung,
die nicht passt, wird einfach abgelehnt; automatische Verschiebungen sind unnötig.
Ergänze danach die sieben Formen I, O, T, S, Z, J und L und wähle beim Erzeugen
eine zufällig aus. Ein dauerhaft gespeichertes `Random`-Objekt reicht dafür aus.

**Prüfen:** Vier Drehungen ergeben die Ausgangsform. An der Wand und neben anderen
Steinen bleiben ungültige Drehungen ohne Wirkung. Auch der lange I-Stein lässt
sich drehen. Leere Zellen einer Steinmatrix blockieren nichts.

## 5 – Reihen entfernen

Ergänze `ClearFullRows`. Eine Reihe ist voll, wenn keine ihrer Zellen 0 enthält.
Verschiebe beim Entfernen alle darüberliegenden Reihen eine Position nach unten
und leere die oberste Reihe. Erhöhe `LinesCleared` für jede entfernte Reihe.

**Hinweis:** Arbeite von unten nach oben. Nach einer Verschiebung muss dieselbe
Zeile erneut geprüft werden, weil dort nun eine andere Reihe liegt.

**Prüfen:** Eine volle Reihe verschwindet. Zwei benachbarte volle Reihen verschwinden
beide. Eine Reihe mit einer Lücke bleibt stehen. Oberhalb liegende Steine rutschen
mit nach unten. Zum gezielten Testen kannst du in `Reset` vorübergehend Zellen
von `Board` belegen. Entferne diese Testbelegung danach wieder.

## 6 – Eine vollständige Spielrunde

Prüfe beim Erzeugen eines Steins, ob er an seiner Startposition Platz hat.
Wenn nicht, setze `IsGameOver` auf `true`. Verhindere dann weitere Änderungen
durch Bewegung, Rotation und Ticks. Das Fenster zeigt Game Over automatisch an
und hält den Timer an. `Reset` muss wieder eine spielbare Runde herstellen.

**Prüfen:** Staple bis zum oberen Rand. Danach verändern weitere Eingaben das
Spielfeld nicht. Mit R kannst du neu beginnen; anschließend funktioniert auch Start.

## Zum Abschluss

Lass jemanden eine Runde spielen und notiere Auffälligkeiten. Behebe einen Fehler,
erkläre eine selbst geschriebene Methode und entferne erledigte TODO-Kommentare.
Passe den Hinweis „Lernstand: Grundgerüst“ in der Oberfläche an euren Stand an.

Falls Zeit bleibt: Punkte, Vorschau des nächsten Steins, schnellerer Spieltakt oder
eigene Farben. Wähle zuerst genau eine Erweiterung.
