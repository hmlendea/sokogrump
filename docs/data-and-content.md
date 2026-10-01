# Data And Content

## Level Files

The packaged levels live in [SokoGrump/Levels](../SokoGrump/Levels/). The current repository contains 101 files, `0.lvl` through `100.lvl`. Each file is expected to contain 14 rows of 16 numeric characters. The numeric values correspond to [TileId.cs](../SokoGrump/Models/TileId.cs).

`BoardRepository.Get` reads rows with `File.ReadAllLines`, converts each character through `char.GetNumericValue`, and indexes a shared tile-entity dictionary. `PlayerOnFloor` records the player coordinate and stores a floor entity. `PlayerOnTarget` records the coordinate and stores an empty-target entity. Other values are copied as their matching tile entity.

The repository then maps data objects into domain models through [BoardMappingExtensions.cs](../SokoGrump/GameLogic/Mapping/BoardMappingExtensions.cs). Mapping preserves board id, start position, and tiles. It builds `Board.Targets` from both `EmptyTarget` and `CrateOnTarget` positions.

The repository exposes update/remove methods inherited from the data-access abstraction, but normal game loading is read-only and uses `GetAll`. Level source files are not modified during play.

## Board And Tile Caching

[BoardManager.cs](../SokoGrump/GameLogic/GameManagers/BoardManager.cs) loads all files into a dictionary keyed by filename stem. It also creates six runtime tile prototypes: floor, wall, crate, empty target, completed target, and void. Player marker tiles are source-format values and are not runtime prototypes.

- `GetBoard(id)` returns a deep clone of the cached board.
- `GetTile(id)` returns a clone of the requested tile prototype.
- `GetTiles()` exposes the prototype collection for rendering.
- `UnloadContent()` clears both dictionaries.

Clone isolation is an invariant. A `GameManager` may alter tiles, crate variation, and player state without changing cached level definitions.

## Settings And Progress

[ApplicationPaths.cs](../SokoGrump/Settings/ApplicationPaths.cs) derives packaged paths from the executable directory and user state from the operating system local application-data directory:

| Value | Location |
| --- | --- |
| User data directory | `<LocalApplicationData>/SokoGrump` |
| Settings | `<LocalApplicationData>/SokoGrump/Settings.xml` |
| Levels | `<application root>/Levels` |
| Data | `<application root>/Data` |
| Localisation | `<application root>/Data/Localisation` |

[SettingsManager.cs](../SokoGrump/Settings/SettingsManager.cs) is a singleton containing `AudioSettings`, `GraphicsSettings`, `UserData`, and `DebugMode`. It serialises itself as XML. A missing settings file is replaced by defaults and saved immediately. Read/write errors are not translated into a recovery flow.

`GraphicsSettings` defaults to 1280 by 720 windowed mode. Each host update applies fullscreen and resolution changes to MonoGame when they differ from the current graphics device settings. The settings screen currently exposes fullscreen; audio settings exist in the serialised model but have no corresponding gameplay flow.

`UserData.LastLevel` controls the Continue Game menu item. `UserData.Language` records the selected language and is saved with the other settings.

## Localisation

The JSON files are [en.json](../SokoGrump/Data/Localisation/en.json) and [ro.json](../SokoGrump/Data/Localisation/ro.json). [LocalisationManager.cs](../SokoGrump/Localisation/LocalisationManager.cs) chooses candidates as follows:

1. If a language is saved, try that language, then English.
2. Otherwise try `CurrentUICulture.Name`, its two-letter ISO name, then English.
3. On the first successful file, load it and persist the selected language.
4. If no candidate exists, retain English as the current language and persist English on first run.

Screens access translated labels through manager properties rather than reading JSON directly.

## MonoGame Content

[Content.mgcb](../SokoGrump/Content/Content.mgcb) defines the compiled content pipeline. The content tree contains sprites, tile sheets, buttons, cursors, fonts, and audio. Runtime string names are established in [BoardManager.cs](../SokoGrump/GameLogic/GameManagers/BoardManager.cs) and [GuiGameBoard.cs](../SokoGrump/Gui/Controls/GuiGameBoard.cs), for example `SpriteSheets/brick`, `SpriteSheets/wall`, `SpriteSheets/crate`, `SpriteSheets/player`, and `SpriteSheets/target`.

The project file [SokoGrump.csproj](../SokoGrump/SokoGrump.csproj) copies level files, data files, the bitmap font, and platform-specific native libraries to output. Compiled content is loaded by `NuciContentManager` during [GameWindow.LoadContent](../SokoGrump/GameWindow.cs).

## Format Change Impact

Changing a level format requires coordinated updates to [BoardRepository.cs](../SokoGrump/DataAccess/Repositories/BoardRepository.cs), mapping, level fixtures, and tests. Changing dimensions requires updates to [GameDefines.cs](../SokoGrump/Settings/GameDefines.cs), parsing loops, rendering layout, and test helpers. Renaming a content asset requires updating both the `.mgcb` entry and every source `ContentFile` or `SpriteSheet` reference.