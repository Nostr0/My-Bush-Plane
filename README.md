# My Bush Plane

Codename im Code: `Bush`. Einzelspieler-Spiel im Stil von My Summer Car, aber mit Flugzeugen: Teile einzeln montieren, warten, reparieren, fliegen. Unity, URP, nur PC.

## Voraussetzungen

- Unity `6000.6.9f1` (siehe `ProjectSettings/ProjectVersion.txt`), installierbar über Unity Hub
- Git und Git LFS
- Visual Studio mit Workload "Game development with Unity" (siehe `.vsconfig`)

## Setup

1. Git LFS einmalig aktivieren: `git lfs install`
2. Repository klonen: `git clone https://github.com/Nostr0/My-Bush-Plane.git`
3. Im Unity Hub "Add project from disk" wählen und den **Repository-Ordner selbst** angeben. Die Unity-Projektwurzel ist das Repo-Root, es gibt keinen Unterordner `Game/`.
4. Beim ersten Öffnen den Editor 6000.6.9f1 installieren, falls der Hub ihn anbietet. Der erste Import dauert mehrere Minuten.
5. Startszene: `Assets/_Project/Scenes/Bootstrap.unity`.

Hinweise:

- Das Projekt nutzt ausschließlich das neue Input System
- Asset Serialization steht auf "Force Text", Meta-Dateien sind sichtbar.
- Binärdateien (`png`, `psd`, `tga`, `exr`, `fbx`, `blend`, `wav`, `ogg`, `mp3`, `mp4`) liegen in Git LFS. Ohne installiertes LFS erscheinen Platzhalter-Dateien statt der Assets.

## Dokumentation

| Dokument | Inhalt |
|---|---|
| [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) | Modulgrenzen, Layer, Konventionen, Git-Regeln, Definition of Done |
| [docs/adr/](docs/adr/) | Einzelne Architekturentscheidungen |
| [docs/packages.md](docs/packages.md) | Paketversionen und Zweck |
| [docs/physics.md](docs/physics.md) | Physik-Layer, Kollisionsmatrix, Physik-Grundeinstellungen |
| [docs/performance.md](docs/performance.md) | Budgets und Messprotokolle |
| [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) | Lizenzen von Fremdcode |
