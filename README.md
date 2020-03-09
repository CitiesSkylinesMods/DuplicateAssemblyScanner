# DuplicateAssemblyScanner

Detects duplicate assemblies and determines where they come from. You can subscribe it on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2013398705).

![DuplicateAssemblyScanner](https://user-images.githubusercontent.com/1386719/76086424-f9032280-5fab-11ea-9318-82865af22b33.png)

`C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines\Cities_Data\DuplicateAssemblyScanner.log`:

```
DuplicateAssemblyScanner v1.2.0.24991
[SettingsUI]
Scanning app domain assemblies for duplicates...

[>] = Assembly loaded in RAM, [*] = Mod enabled

0Harmony (5 loaded, 24 mods):
---------------------------------------------------------------------------------------------------------
    Asm Version  MD5 Hash                         /Mod Folder   Mod name
---------------------------------------------------------------------------------------------------------
  * 1.0.9.1      812905134bf7bd868428280d601ac573 /1372431101   Painter
> * 1.0.9.1      812905134bf7bd868428280d601ac573 /1386697922   Garbage Bin Controller
>   1.1.0.0      9b81c76b538fc2217f8df27d4d76685f /1412844620   Realistic Walking Speed
    1.1.0.0      c0d7618949db5ae9bb0a6dda04b82769 /870291141    Random Train Trailers 2.1.2
  * 1.2.0.1      09cb0a9d9724f2965a4b73bb590e9a57 /1420955187   Real Time
  * 1.2.0.1      09cb0a9d9724f2965a4b73bb590e9a57 /1435741602   Snooper
//etc...
```

## Purpose

Duplicate assemblies cause all sorts of [weird bugs](https://github.com/CitiesSkylinesMods/TMPE/issues/747), particularly when there's multiple versions of [Harmony library](https://github.com/pardeike/Harmony).

This mod detects the presence of duplicate assemblies:

* Summary shown in mod options screen
* Detailed info in the `DuplicateAssemblyScanner.log`

## Build

It should automatically find your `Managed` folder so there's usually no need to manually add reference paths.

If you run in to problems, see the TM:PE [Build Guide](https://github.com/CitiesSkylinesMods/TMPE/blob/master/docs/BUILDING_INSTRUCTIONS.md) as we use basically the same setup.

## Credits

* **dymanoid** for god-tier knowledge of basically everything
* **krzychu124** for MD5 hash code and lots of help
* **kian.zarrin** for auto reference paths for managed DLLs, shared assembly info trick
* **keallu** for his logging class
* **boformer** for his modding tutorials
* Anyone who's ever modded C:SL and shared their code for others to learn from!

## Contributing

Always welcome :)
