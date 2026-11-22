# ADR 0001: Assembly- und Namespace-Struktur

Status: Vorschlag
Datum: 22.11.2026

## Kontext

Das Projekt wird neu aufgesetzt. Spiellogik soll ohne Unity testbar sein, Module sollen sich nicht gegenseitig verwickeln, und die Struktur muss über mehr als zehn Meilensteine tragen.

## Entscheidung

Es gilt `docs/ARCHITECTURE.md`, Abschnitte 3 bis 6:

- Root-Namespace `MaintGames.Bush`, Assembly-Name gleich Namespace des Ordners.
- Layer L0 bis L6 mit Abhängigkeiten nur nach unten.
- Sim-Assemblies ohne Engine-Referenzen (`noEngineReferences: true`).
- Alle eigenen asmdefs mit `autoReferenced: false`.
- Eine Assembly entsteht, wenn der erste Code hineingehört (Tabelle in Abschnitt 5.4).

## Alternativen

- Eine einzige Assembly: einfacher, aber keine erzwungenen Grenzen und alles kompiliert bei jeder Änderung neu.
- Eine Assembly pro Feature ohne Layer: Zyklen entstehen schnell, weil Features einander brauchen.
- DI-Framework von Beginn an: verschoben, Entscheidung per eigenem ADR, falls die manuelle Verdrahtung unübersichtlich wird.

## Folgen

- Neue Module brauchen eine asmdef und einen Eintrag in `docs/ARCHITECTURE.md`.
- Layer-Verstöße schlagen beim Kompilieren fehl, nicht erst im Review.
- Leere asmdefs erzeugen in Unity eine Warnung. Deshalb wird jede asmdef zusammen mit ihrem ersten Code angelegt.
