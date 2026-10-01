# Development And Verification

## Projects And Dependencies

The solution in [SokoGrump.slnx](../SokoGrump.slnx) contains the executable [SokoGrump.csproj](../SokoGrump/SokoGrump.csproj) and test project [SokoGrump.UnitTests.csproj](../SokoGrump.UnitTests/SokoGrump.UnitTests.csproj). Both target .NET 10. The game uses MonoGame DesktopGL and NuciXNA packages; the test project uses NUnit and Moq.

Required local tools are the .NET 10 SDK, MonoGame content build tools for asset changes, and a TrueType core font package on Linux. NuGet dependencies restore through the normal .NET workflow.

## Commands

Restore dependencies:

```bash
dotnet restore SokoGrump.slnx
```

Build the executable and content:

```bash
dotnet build SokoGrump
```

Run the game:

```bash
dotnet run --project SokoGrump
```

Run all automated tests:

```bash
dotnet test SokoGrump.slnx
```

The current baseline is 153 discovered NUnit cases, all passing at the time this documentation was generated. Rendering, input, content loading, and screen transitions have no automated end-to-end coverage and require manual verification.

## Test Organisation

- [GameManagerTests.cs](../SokoGrump.UnitTests/GameLogic/GameManagers/GameManagerTests.cs) tests new game, retry, movement legality, pushing, completion, timing, and undo.
- [BoardMappingExtensionsTests.cs](../SokoGrump.UnitTests/GameLogic/Mapping/BoardMappingExtensionsTests.cs) tests entity/domain conversion and target discovery.
- [TileMappingExtensionsTests.cs](../SokoGrump.UnitTests/GameLogic/Mapping/TileMappingExtensionsTests.cs) tests tile conversion.
- [BoardTests.cs](../SokoGrump.UnitTests/Models/BoardTests.cs) tests board cloning and target/tile isolation.
- [TileTests.cs](../SokoGrump.UnitTests/Models/TileTests.cs) and [ModelBaseTests.cs](../SokoGrump.UnitTests/Models/ModelBaseTests.cs) test model behaviour.
- [BoardTestHelper.cs](../SokoGrump.UnitTests/Helpers/BoardTestHelper.cs) supplies deterministic boards and tile entities for unit tests.

The production project exposes internals to the test assembly through `InternalsVisibleTo` in [SokoGrump.csproj](../SokoGrump/SokoGrump.csproj). Game-manager tests inject a mocked [IBoardManager.cs](../SokoGrump/GameLogic/GameManagers/IBoardManager.cs); production construction uses the concrete board manager.

## Manual Smoke Test

After a build, verify:

1. The process opens the splash screen and reaches the title screen.
2. New Game opens level `0`; movement, crate pushing, retry, and undo work.
3. The information bar updates time and move count.
4. Completing a level opens Victory and advances to the next existing numeric level.
5. Completing level `100` opens Game Finished and clears Continue Game progress.
6. Settings can toggle fullscreen and persist after restart.
7. English and Romanian localisation files load, and an unsupported saved language falls back to English.
8. The board, font, cursor, buttons, and audio load from build output.

## Change Impact Guide

| Change | Minimum verification |
| --- | --- |
| Movement, pushing, undo, completion, or timing | Update [GameManagerTests.cs](../SokoGrump.UnitTests/GameLogic/GameManagers/GameManagerTests.cs); run all tests and manually play a representative level |
| Level dimensions or tile identifiers | Update [GameDefines.cs](../SokoGrump/Settings/GameDefines.cs), parser, mapping, test helpers, and level fixtures; run tests and build content |
| Level file format | Update [BoardRepository.cs](../SokoGrump/DataAccess/Repositories/BoardRepository.cs), mapping tests, and packaged files |
| Screen transition or progress persistence | Update the affected screen and run the manual smoke test from a clean user-data directory |
| Settings XML shape | Update [SettingsManager.cs](../SokoGrump/Settings/SettingsManager.cs) and consider migration for existing users |
| Localisation fields or language selection | Update [LocalisationData.cs](../SokoGrump/DataAccess/DataObjects/LocalisationData.cs), both JSON files, manager properties, and screen usage |
| Content asset names or pipeline entries | Update [Content.mgcb](../SokoGrump/Content/Content.mgcb), source `ContentFile`/`SpriteSheet` strings, build, and launch |
| Release packaging | Inspect [release.sh](../release.sh) and verify native libraries and copied data in the target package |

## Release Boundary

[release.sh](../release.sh) delegates packaging to an external maintainer script. It is not the game runtime and should be treated as a release-environment dependency. Review the downloaded script before execution, and verify the resulting package contains the executable, compiled `Content`, all level files, localisation data, font, and platform-native libraries.