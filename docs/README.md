# SokoGrump Documentation

This directory is the implementation guide for SokoGrump. It complements the concise project overview in [README.md](../README.md) and the repository-level architecture record in [ARCHITECTURE.md](../ARCHITECTURE.md).

## Start Here

- [Architecture](architecture.md): boundaries, ownership, dependencies, and design constraints.
- [Runtime Flows](runtime-flows.md): startup, screen navigation, gameplay update, completion, and shutdown.
- [Gameplay Rules](gameplay.md): board state, movement, pushing, undo, timing, and completion semantics.
- [Data And Content](data-and-content.md): level files, mappings, settings, localisation, assets, and deployment inputs.
- [Development And Verification](development.md): project structure, commands, tests, and change-impact guidance.

## Repository Scope

SokoGrump is a single-process MonoGame DesktopGL application. It has no server, network API, shared database, or background service. The principal runtime inputs are packaged levels, compiled MonoGame content, and localisation files. Mutable progress and preferences are stored locally in `Settings.xml`.

The source is split into two projects in [SokoGrump.slnx](../SokoGrump.slnx):

- [SokoGrump](../SokoGrump/): executable game.
- [SokoGrump.UnitTests](../SokoGrump.UnitTests/): NUnit and Moq tests for models, mapping, and game logic.

## Documentation Rules

This documentation describes current code, including limitations and awkward boundaries. It is not a proposed redesign. When implementation changes, update the relevant topic and its links, then run the verification commands in [Development And Verification](development.md).