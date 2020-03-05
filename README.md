# DuplicateAssemblyScanner

Detects duplicate assemblies and determines where they come from. You can subscribe it on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2013398705).

![Harmony](https://user-images.githubusercontent.com/1386719/76034521-0df19e80-5f37-11ea-8a34-3b65012e83f8.png)

```
[Info]  DuplicateAssemblyScanner v1.2.0.40729
[Info]  Enabled
[Info]  SettingsUI
[Info]  Scanning app domain assemblies for duplicates... [*] = Mod enabled, [>] = Assembly loaded in RAM
[Info]  0Harmony found in:
[Info]  *   1.0.9.1 812905134bf7bd868428280d601ac573 /1372431101   Painter
[Info]  * > 1.0.9.1 812905134bf7bd868428280d601ac573 /1386697922   Garbage Bin Controller
[Info]    > 1.1.0.0 9b81c76b538fc2217f8df27d4d76685f /1412844620   Realistic Walking Speed
[Info]      1.1.0.0 c0d7618949db5ae9bb0a6dda04b82769 /870291141    Random Train Trailers 2.1.2
[Info]  *   1.2.0.1 09cb0a9d9724f2965a4b73bb590e9a57 /1869561285   Prop Painter
[Info]  * > 1.2.0.1 09cb0a9d9724f2965a4b73bb590e9a57 /1886877404   Custom Effect Loader
[Info]  * > 1.2.0.2 67ecabf6c52feeb90f33cf501f723f01 /1562650024   Rebalanced Industries
[Info]  * > 2.0.0.0 616b7f80b28b7e80050998e4473d4c31 /1758376843   Network Skins 2 Beta
[Info]  Disabled
```

## Purpose

Duplicate assemblies cause all sorts of [weird bugs](https://github.com/CitiesSkylinesMods/TMPE/issues/747), particularly when there's multiple versions of [Harmony library](https://github.com/pardeike/Harmony).

This mod detects the presence of duplicate assemblies:

* Basic details are shown in the mod options screen
* Comprehensive details are output to the `DuplicateAssemblyScanner.log`

## Log format

The lists are sorted by assembly version then mod folder:

* `*` denotes mod was enabled
* `>` denotes assembly was loaded in app domain
* Assembly version
* MD5 hash (to detect if same version assembly contains different code)
* Folder that contains the mod
* Name of the mod

## Build

It will automatically find your `Managed` folder so there's usually no need to manually add reference paths.

Thanks to **Kian Zarrin** for making that possible!

## Contributing

Always welcome :)
