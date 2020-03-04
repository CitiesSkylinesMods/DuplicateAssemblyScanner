# DuplicateAssemblyScanner

Detects duplicate assemblies and determines where they come from. You can subscribe it on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2013398705).

![Harmony](https://user-images.githubusercontent.com/1386719/75841597-0a8ad580-5dc6-11ea-8041-64054ed53dc2.png)

```
[Info]   # Duplicate '0Harmony' assembly exists in mods:
[Info]     - v1.2.0.2 /1562650024 'Rebalanced Industries'
[Info]     - v1.1.0.0 /1412844620 'Realistic Walking Speed'
[Info]     - v1.0.9.1 /1386697922 'Garbage Bin Controller'
[Info]     - v1.2.0.1 /1886877404 'Custom Effect Loader'
[Info]     - v2.0.0.0 /1758376843 'Network Skins 2 Beta'
```
> Log entry formt: `- vAssembly /folder 'mod name'`

## Purpose

Duplicate assemblies cause all sorts of [weird bugs](https://github.com/CitiesSkylinesMods/TMPE/issues/747), particularly when there's multiple versions of [Harmony library](https://github.com/pardeike/Harmony).

This mod detects the presence of duplicate assemblies:

* Basic details are shown in the mod options screen
* Comprehensive details are output to the `DuplicateAssemblyScanner.log`

## Build

It will automatically find your `Managed` folder so there's usually no need to manually add reference paths.

Thanks to **Kian Zarrin** for making that possible!

## Contributing

Always welcome :)
