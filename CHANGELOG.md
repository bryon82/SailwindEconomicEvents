# Changelog

All notable changes to this project will be documented in this file.

## [v1.1.1] - 2026-06-21

### Added
- 14 new events.

### Updated
- Guid, removed the 82.
- Guid for ModSaveBackups.

## [v1.1.1] - 2026-06-04

### Fixed
- Removed testing code.

## [v1.1.0] - 2026-06-04

### Updated
- Changed the release process so hopefully some linux users will no longer have issues when unzipping the release file.

### Performance Improvements
- Removed many LINQ operations and replaced with for-loops and cached lookups.
- Cache port/event lookups instead of repeating queries.
- Cache list of all ports at start instead of rebuilding with each call.
- Cache built strings for events once when scheduled instead of building each time looked at in log.

## [v1.0.3] - 2026-04-24

### Fixed
- UI character size and line spacing bug in event description.

## [v1.0.2] - 2026-02-23

### Fixed
- UI bugs caused by changes made to reputation UI in 0.35 update.

### Added
- Events are now on their own dedicated logbook tab.

## [v1.0.1] - 2025-09-15

### Added
- New ports from 0.33 update.
- Explosive Event event.

## [v1.0.0] - 2025-07-24

### Added
- Economic events which a affect a port in each region.
- Port dude will tell you news of active events.
