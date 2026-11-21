# Physics Layer Collision Matrix

## Legende

- `✓` = Kollision aktiviert
- `–` = Kollision deaktiviert

## Layer

| Layer | Beschreibung |
|--------------------|------------------------------------------------------|
| Player             | Spieler-Charakter                                    |
| HandItem           | Gegenstände, die der Spieler gerade hält             |
| Item               | Aufnehmbare und physikalische Gegenstände            |
| VehiclePart        | Einzelne Fahrzeugteile, z. B. Räder, Türen und Motor |
| VehicleHull        | Fahrzeugkarosserie und Außenhülle                    |
| Terrain            | Boden, Landschaft und Umgebung                       |
| Trigger            | Unsichtbare Trigger-Zonen                            |
| InteractionRaycast | Für Interaktionsabfragen vorgesehene Layer           |

## Collision Matrix

|                  | Player       | HandItem | Item   | VehiclePart  | VehicleHull  | Terrain  | Trigger  |
|------------------|:------------:|:--------:|:------:|:------------:|:------------:|:--------:|:--------:|
| **Player**       | –            | –        | ✓      | ✓           | ✓            | ✓        | –        |
| **HandItem**     | –            | –        | –       | ✓           | ✓           | ✓        | –         |
| **Item**         | ✓            | –        | ✓      | ✓           | ✓           | ✓        | –         |
| **VehiclePart**  | ✓            | ✓        | ✓      | ✓           | ✓           | ✓        | –         |
| **VehicleHull**  | ✓            | ✓        | ✓      | ✓           | ✓           | ✓        | –         |
| **Terrain**      | ✓            | ✓        | ✓      | ✓           | ✓           | ✓        | –         |
| **Trigger**      | –            | –        | –       | –           | –            | –        | –         |
