namespace DuplicateAssemblyScanner.Util {
    using ColossalFramework;
    using ColossalFramework.Plugins;
    using ICities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using static ColossalFramework.Plugins.PluginManager;

    public class Assemblies {

        /// <summary>
        /// Scans app domain assemblies and compiles a dictionary keyed by assembly name;
        /// each value is a list of assemblies matching the name.
        /// </summary>
        ///
        /// <param name="duplicatesFound">Will be <c>true</c> if duplicates found.</param>
        /// <returns>Dictionary of assembly lists keyed by assembly name.</returns>
        public static Dictionary<string, List<string>> Scan(out bool duplicatesFound) {
            Log.Info("Scanning app domain assemblies for duplicates...");

            duplicatesFound = false;

            // assembly name -> list of assemblies with that name
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies) {

                try {
                    AssemblyName details = asm.GetName();

                    string name = details.Name;
                    string ver = details.Version.ToString();

                    if (results.TryGetValue(name, out List<string> matches)) {

                        if (matches.Count == 1) {
                            duplicatesFound = true;
                            LogModsContainingAssembly(name);
                        }

                        matches.Add(ver);

                    } else {

                        results.Add(name, new List<string>() {
                            { ver }
                        });

                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                }
            }

            return results;
        }

        /// <summary>
        /// Given an assembly name, this attempts to find which mods contain that assembly.
        /// Results are listed in the log file.
        /// </summary>
        ///
        /// <param name="targetName">The `GetName().Name` of the assembly to scan for.</param>
        internal static void LogModsContainingAssembly(string targetName) {
            Log.Info($"# '{targetName}' assembly exists in mods:");

            PluginManager manager = Singleton<PluginManager>.instance;

            List<PluginInfo> mods = new List<PluginInfo>(manager.modCount);

            mods.AddRange(manager.GetPluginsInfo()); // normal mods
            mods.AddRange(manager.GetCameraPluginInfos()); // camera scripts

            List<Assembly> contents;
            string name;
            string ver;
            string path;

            // iterate plugins
            foreach (PluginInfo mod in mods) {
                try {
                    contents = mod.GetAssemblies();
                    foreach (Assembly asm in contents) {
                        name = asm.GetName().Name;
                        if (name == targetName) {
                            ver = asm.GetName().Version.ToString();
                            path = Path.GetFileName(mod.modPath);
                            name = GetNameOfMod(mod);

                            Log.Info($"  - v{ver} /{path} '{name}'");
                        }
                    }
                } catch (Exception e) {
                    Log.Error(e.ToString());
                }
            }
        }

        internal static string GetNameOfMod(PluginInfo mod) {
            try {
                return mod?.userModInstance != null ? ((IUserMod)mod.userModInstance).Name : string.Empty;
            } catch {
                return "** error **";
            }
        }
    }
}
