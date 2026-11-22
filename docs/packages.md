# Pakete

Stand: 22.11.2026, Unity 6000.6.9f1. Die Versionen stammen aus `Packages/manifest.json` und `Packages/packages-lock.json`. Aendert sich ein Paket, wird diese Datei im selben Pull Request angepasst.

## Direkt eingebunden

| Paket | Version | Zweck |
|---|---|---|
| `com.unity.addressables` | 2.11.2 | Laden von Content über Schlüssel |
| `com.unity.cinemachine` | 6.6.0 | Kamera (optional) |
| `com.unity.inputsystem` | 1.20.1 | Input System, einziges aktives Input Handling |
| `com.unity.localization` | 1.5.14 | Lokalisierung (Deutsch, Englisch) |
| `com.unity.mathematics` | 1.4.0 | Mathematik für Foundation und Sim-Assemblies |
| `com.unity.probuilder` | 6.1.4 | Greybox-Modellierung |
| `com.unity.render-pipelines.universal` | 17.6.0 | URP (Render Pipeline) |
| `com.unity.modules.uielements` | 1.0.0 | UI Toolkit (Engine-Modul) |

## Als Abhängigkeit mitgebracht

Nicht in `manifest.json` aufgeführt, aber im Projekt vorhanden. Wird eines davon direkt im Code gebraucht, wird es explizit in `manifest.json` eingetragen.

| Paket | Version | Zweck |
|---|---|---|
| `com.unity.burst` | 2.0.0 | Burst-Compiler, kommt als Abhängigkeit mit |
| `com.unity.collections` | 6.6.0 | Native Collections, kommt als Abhängigkeit mit |
| `com.unity.test-framework` | 1.8.0 | Unit-Tests (EditMode und PlayMode) |
| `com.unity.nuget.newtonsoft-json` | 3.2.2 | JSON, Kandidat für das Speicherformat (ADR 0007) |

## Werkzeuge

| Paket | Version |
|---|---|
| `com.unity.collab-proxy` | 2.14.0 |
| `com.unity.ide.rider` | 3.1.0 |
| `com.unity.ide.visualstudio` | 2.0.28 |
