# Physik

Gehört zu Issue #3. Dieses Dokument beschreibt den **Soll-Zustand**. Maßgeblich im Projekt sind die Dateien `ProjectSettings/TagManager.asset` (Layer) und `ProjectSettings/DynamicsManager.asset` (Matrix, Physik-Werte). Weicht eine davon ab, wird entweder das Projekt oder dieses Dokument korrigiert.

## Layer

| Index | Layer | Beschreibung |
|---:|---|---|
| 6 | Player | Spieler-Charakter |
| 7 | HandItem | Gegenstand, den der Spieler gerade hält |
| 8 | Item | Aufnehmbare, physikalische Gegenstände |
| 9 | VehiclePart | Einzelne Fahrzeugteile (Räder, Türen, Motor) |
| 10 | VehicleHull | Fahrzeugkarosserie und Außenhülle |
| 11 | Terrain | Boden, Landschaft, feste Umgebung |
| 12 | Trigger | Unsichtbare Trigger-Zonen |
| 13 | InteractionRaycast | Nur für Interaktions-Raycasts, kollidiert mit nichts |

Die eingebauten Layer (Default, TransparentFX, Ignore Raycast, Water, UI) bleiben auf Unity-Standard. Spielobjekte gehören nicht auf `Default`, weil Default mit allem kollidiert.

## Kollisionsmatrix

`✓` = Kollision bzw. Trigger-Ereignis aktiv, `–` = deaktiviert.

Die Matrix gilt in Unity auch für Trigger-Ereignisse (`OnTriggerEnter` und Co.). Ein Collider auf dem Layer `Trigger`, der mit nichts kollidiert, meldet deshalb nie etwas. Die Trigger-Zeile ist darum nicht leer.

|                    | Player | HandItem | Item | VehiclePart | VehicleHull | Terrain | Trigger | InteractionRaycast |
|--------------------|:------:|:--------:|:----:|:-----------:|:-----------:|:-------:|:-------:|:------------------:|
| **Player**             | – | – | ✓ | ✓ | ✓ | ✓ | ✓ | – |
| **HandItem**           | – | – | – | ✓ | ✓ | ✓ | ✓ | – |
| **Item**               | ✓ | – | ✓ | ✓ | ✓ | ✓ | ✓ | – |
| **VehiclePart**        | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | – |
| **VehicleHull**        | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | – |
| **Terrain**            | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | – | – |
| **Trigger**            | ✓ | ✓ | ✓ | ✓ | ✓ | – | – | – |
| **InteractionRaycast** | – | – | – | – | – | – | – | – |

Begründungen:

- `HandItem` kollidiert nicht mit `Player` und nicht mit `Item`: Das gehaltene Objekt soll den Spieler nicht wegschieben und keine losen Teile aus dem Weg räumen.
- `Trigger` ist mit allem aktiv außer `Terrain`, `Trigger` und `InteractionRaycast`. Das ist ein Vorschlag und wird eingeschränkt, sobald konkrete Zonen existieren.
- `InteractionRaycast` wird nur über `LayerMask` in Raycasts getroffen und nimmt an keiner Kollision teil.

## Physik-Grundeinstellungen

Aktuelle Werte (Unity-Standard, noch nicht evaluiert):

| Einstellung | Wert | Datei |
|---|---|---|
| Fixed Timestep | 0,02 s | `TimeManager.asset` |
| Default Solver Iterations | 6 | `DynamicsManager.asset` |
| Default Solver Velocity Iterations | 1 | `DynamicsManager.asset` |
| Default Contact Offset | 0,01 m | `DynamicsManager.asset` |
| Sleep Threshold | 0,005 | `DynamicsManager.asset` |
| Default Max Depenetration Velocity | 10 m/s | `DynamicsManager.asset` |

## Evaluation (offen)

Akzeptanzkriterium aus Issue #3: Ein 2-cm-Testobjekt tunnelt nicht durch eine Tischplatte.

Versuchsaufbau, sobald die Playground-Szene (Issue #5) steht:

1. Tischplatte als Box-Collider, 3 cm dick. Testobjekt: Würfel mit 2 cm Kantenlänge, 20 g.
2. Wurf senkrecht und schräg auf die Platte mit 1, 5, 10 und 20 m/s, je 100 Durchläufe.
3. Varianten: Fixed Timestep 0,02 / 0,01667 / 0,01 s, Collision Detection Discrete / Continuous / Continuous Dynamic, Solver Iterations 6 / 10.
4. Gemessen wird die Zahl der Tunnel-Fälle je Variante und die Physik-Zeit pro Frame.
5. Ergebnis und gewählte Werte werden hier eingetragen.
