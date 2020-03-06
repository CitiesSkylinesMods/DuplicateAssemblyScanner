# DuplicateAssemblyScanner

Detects duplicate assemblies and determines where they come from. You can subscribe it on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2013398705).

![DuplicateAssemblyScanner](https://user-images.githubusercontent.com/1386719/76086424-f9032280-5fab-11ea-9318-82865af22b33.png)

```
DuplicateAssemblyScanner v1.2.0.21987
[SettingsUI]
Scanning app domain assemblies for duplicates...

[*] = Mod enabled, [>] = Assembly loaded in RAM

0Harmony (5 loaded, 24 mods):
---------------------------------------------------------------------------------------------------------
    Asm Version  MD5 Hash                         /Mod Folder   Mod name
---------------------------------------------------------------------------------------------------------
*   1.0.9.1      812905134bf7bd868428280d601ac573 /1372431101   Painter
* > 1.0.9.1      812905134bf7bd868428280d601ac573 /1386697922   Garbage Bin Controller
  > 1.1.0.0      9b81c76b538fc2217f8df27d4d76685f /1412844620   Realistic Walking Speed
    1.1.0.0      c0d7618949db5ae9bb0a6dda04b82769 /870291141    Random Train Trailers 2.1.2
*   1.2.0.1      09cb0a9d9724f2965a4b73bb590e9a57 /1420955187   Real Time
*   1.2.0.1      09cb0a9d9724f2965a4b73bb590e9a57 /1435741602   Snooper
//etc...
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

## Credits

* **dymanoid** for god-tier knowledge of basically everything
* **krzychu124** for MD5 hash code and lots of help
* **kian.zarrin** for auto reference paths for managed DLLs, shared assembly info trick
* **keallu** for his logging class
* **boformer** for his modding tutorials
* Anyone who's ever modded C:SL and shared their code for others to learn from!

## Contributing

Always welcome :)
