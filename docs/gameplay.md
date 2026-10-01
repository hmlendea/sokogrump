# Gameplay Rules

## Board Model

A board is a 16 by 14 `Tile[,]` array, a player start coordinate, and a list of target coordinates. The dimensions and crate variation count are defined in [GameDefines.cs](../SokoGrump/Settings/GameDefines.cs). `Player` stores location, facing direction, and successful move count.

Tile identifiers are defined in [TileId.cs](../SokoGrump/Models/TileId.cs):

| Id | Meaning | Runtime role |
| ---: | --- | --- |
| 0 | `Floor` | Walkable terrain |
| 1 | `Wall` | Solid boundary |
| 2 | `CrateOnFloor` | Pushable crate |
| 3 | `EmptyTarget` | Walkable target in source data |
| 4 | `PlayerOnFloor` | Player start marker in source data |
| 5 | `CrateOnTarget` | Crate occupying a target in source data |
| 6 | `PlayerOnTarget` | Player start marker on a target in source data |
| 7 | `Void` | Solid outside/void tile |

`TileType` classifies tiles as `Walkable`, `Solid`, or `Moveable`. At runtime the player is separate from the tile array. The player-start marker is converted to `Floor` or `EmptyTarget` while parsing; `NewGame` then removes target encoding from the mutable gameplay array while retaining target coordinates.

## Starting A Level

[GameManager.NewGame](../SokoGrump/GameLogic/GameManagers/GameManager.cs) performs these steps:

1. Set `Level` and obtain a clone from `IBoardManager.GetBoard`.
2. Reset elapsed time and create a player at `Board.PlayerStartLocation` facing South with zero moves.
3. Replace every `EmptyTarget` with a cloned `Floor` tile.
4. Replace every `CrateOnTarget` with a cloned `CrateOnFloor` tile.
5. Assign each crate a random variation from `0` through `CrateVariationCount - 1`.
6. Clear undo history.

`Retry` calls `NewGame(Level)`, so it resets player state, timer, crate variations, and history.

## Movement

[GameManager.CanMove](../SokoGrump/GameLogic/GameManagers/GameManager.cs) calculates one destination and, for a push, a second destination in the requested cardinal direction.

- A destination with `TileType.Walkable` is traversable.
- A destination with `TileType.Solid` is blocked.
- A destination with `TileType.Moveable` is traversable only when it is a `CrateOnFloor` and the tile beyond it is an in-bounds `Floor`.
- A direction outside the four values in [MovementDirection.cs](../SokoGrump/Models/MovementDirection.cs) is rejected.
- A board edge blocks movement when the destination or push destination would be outside the fixed grid.

`MovePlayer` is a no-op when the direction or move is invalid. A valid move increments `Player.MovesCount`, updates location and direction, and pushes a snapshot onto the undo stack. A push moves the crate to the second destination and preserves its visual variation.

The model does not permit pushing onto a target tile during normal play because targets have been normalised to `Floor`. Target presentation is reconstructed from `Board.Targets`.

## Undo

Undo is a stack of `MoveSnapshot` values held by `GameManager`. Each snapshot stores the prior player location, direction, move count, both affected tile copies, and whether a crate was pushed.

- `CanUndo` is true when history is non-empty.
- `PeekUndo` exposes only animation information and requires history to exist.
- `Undo` does nothing with an empty history.
- Undo restores both crate tiles when the move pushed a crate, then restores player location, direction, and move count.
- New game and retry clear history.

[GuiGameBoard.cs](../SokoGrump/Gui/Controls/GuiGameBoard.cs) prevents overlapping movement animations. It animates first, then calls `Undo` or `MovePlayer` when the player animation ends.

## Completion And Timing

`GameManager.Update` evaluates completion before advancing the timer. Completion is true when every coordinate in `Board.Targets` contains `TileId.CrateOnFloor`. The targets list includes both originally empty targets and originally occupied targets, so all target positions must be covered.

Elapsed time advances by the supplied milliseconds only while the level is incomplete. The game manager still forwards the update to `IBoardManager.Update`, whose current implementation is empty.

[GameplayScreen.cs](../SokoGrump/Gui/Screens/GameplayScreen.cs) handles the resulting transition:

- If the next `.lvl` file exists, it opens `VictoryScreen` with the next level and stores that next level in `UserData.LastLevel`.
- Otherwise it opens `GameFinishedScreen` and stores `LastLevel = 0`.

## Input And Presentation

Keyboard movement is handled by [GuiGameBoard.cs](../SokoGrump/Gui/Controls/GuiGameBoard.cs); menus use NuciXNA controls and mouse input. Retry and undo are separate GUI buttons. Rendering uses target coordinates independently from tile identity: a crate on a target is tinted red, and target sprites are hidden where a crate is already drawn.

The public controls are documented in [README.md](../README.md): WASD or arrow keys move, `R` retries, `U` undoes, and mouse input operates menu and gameplay controls.

## Verified Behaviour

The rule contract is covered by [GameManagerTests.cs](../SokoGrump.UnitTests/GameLogic/GameManagers/GameManagerTests.cs), including valid walk, solid tiles, board edges, crate pushes, blocked pushes, retry, undo, move count, timing, and completion. Mapping and target discovery are covered by [BoardMappingExtensionsTests.cs](../SokoGrump.UnitTests/GameLogic/Mapping/BoardMappingExtensionsTests.cs).