namespace DuplicateAssemblyScanner.Util {
    using ColossalFramework;
    using ColossalFramework.Plugins;
    using DuplicateAssemblyScanner.Structs;
    using ICities;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using static ColossalFramework.Plugins.PluginManager;

    /// <summary>
    /// Utility functions for mods.
    /// </summary>
    public class Mods {
        // Markers used in log file entries
        private const string MARKER_BLANK = " ";
        private const string MARKER_ENABLED = "*";
        private const string MARKER_LOADED = ">";

        /// <summary>
        /// Given an assembly name, this scans to find out which mods contain the assembly.
        /// </summary>
        /// 
        /// <param name="assemblyName">The name of the assembly to search for.</param>
        /// 
        /// <returns>Returns a list of the mods containing the assembly.</returns>
        internal static List<ModAssembly> FindAnyContainingAssembly(string assemblyName) {

            Log.Info($"{assemblyName} found in:");

            List<ModAssembly> results = new List<ModAssembly>();

            PluginManager manager = Singleton<PluginManager>.instance;
            List<PluginInfo> mods = new List<PluginInfo>(manager.modCount);

            mods.AddRange(manager.GetPluginsInfo()); // normal mods
            mods.AddRange(manager.GetCameraPluginInfos()); // camera scripts

            // iterate mods
            foreach (PluginInfo mod in mods) {
                if (TryGetLoadedAssembly(mod, assemblyName, out ModAssembly loaded)) {
                    results.Add(loaded);
                } else if (TryGetUnloadedAssembly(mod, assemblyName, out ModAssembly unloaded)) {
                    results.Add(unloaded);
                }
            }

            results = results.OrderBy(x => x.AsmDetails.Version).ThenBy(y => y.Folder).ToList();

            foreach (ModAssembly item in results) {

                string v = item.AsmDetails.Version.ToString();
                string n = item.ModName;
                string l = item.AsmLoaded ? MARKER_LOADED : MARKER_BLANK;
                string e = item.ModEnabled ? MARKER_ENABLED : MARKER_BLANK;
                string p = item.Folder.PadRight(12);
                string h = item.AsmMD5Hash.PadRight(32);

                Log.Info($"{e} {l} {v} {h} /{p} {n}");
            }

            return results;
        }

        /// <summary>
        /// Scans the mod to see if it has loaded the assembly in to app domain.
        /// </summary>
        /// 
        /// <param name="mod">The <see cref="PluginInfo"/> to scan.</param>
        /// <param name="assemblyName">The name of the assembly to search for.</param>
        /// <param name="details">If found, the <see cref="ModAssembly"/> struct containing details.</param>
        /// 
        /// <returns>Returns <c>true</c> if successful, otherwise <c>false</c>.</returns>
        private static bool TryGetLoadedAssembly(PluginInfo mod, string assemblyName, out ModAssembly details) {

            List<Assembly> assemblies = mod.GetAssemblies();

            foreach (Assembly asm in assemblies) {

                if (asm.GetName().Name == assemblyName) {

                    string hash = string.Empty;

                    if (TryGetAssemblyPath(mod, assemblyName, out string fullPath)) {
                        if (Md5.TryGetHash(fullPath, out string md5Hash, true)) {
                            hash = md5Hash;
                        }
                    }

                    details = new ModAssembly {
                        ModPath = mod.modPath,
                        ModName = GetName(mod),
                        ModEnabled = mod.isEnabled,

                        Folder = Path.GetFileName(mod.modPath),

                        AsmDetails = asm.GetName(),
                        AsmLoaded = true,
                        AsmMD5Hash = hash,
                    };

                    return true;
                }
            }

            details = default;
            return false;
        }

        /// <summary>
        /// Scans the mod folder to see if contains an unloaded copy of the assembly.
        /// </summary>
        /// 
        /// <param name="mod">The <see cref="PluginInfo"/> to scan.</param>
        /// <param name="assemblyName">The name of the assembly to search for.</param>
        /// <param name="details">If found, the <see cref="ModAssembly"/> struct containing details.</param>
        /// 
        /// <returns>Returns <c>true</c> if successful, otherwise <c>false</c>.</returns>
        private static bool TryGetUnloadedAssembly(PluginInfo mod, string assemblyName, out ModAssembly details) {

            details = default;

            // we're only interested in Harmony for now...
            if (assemblyName != "0Harmony") {
                return false;
            }

            if (TryGetAssemblyPath(mod, assemblyName, out string fullPath)) {

                details = new ModAssembly {
                    ModPath = mod.modPath,
                    ModName = GetName(mod),
                    ModEnabled = mod.isEnabled,

                    Folder = Path.GetFileName(mod.modPath),

                    AsmLoaded = false,
                };

                if (Md5.TryGetHash(fullPath, out string md5Hash, true)) {
                    details.AsmMD5Hash = md5Hash;
                }

                if (Assemblies.TryGetAssemblyDetailsWithoutLoading(fullPath, out AssemblyName asmDetails)) {
                    details.AsmDetails = asmDetails;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to get the assembly path based on a <see cref="PluginInfo"/> and assembly name.
        /// </summary>
        /// 
        /// <param name="mod">The <see cref="PluginInfo"/> associated with a mod.</param>
        /// <param name="assemblyName">The name of the assembly we're looking for.</param>
        /// <param name="fullPath">If successful, the full path to the assembly on disk.</param>
        /// 
        /// <returns>Returns <c>true</c> if successful, otherwise <c>false</c>.</returns>
        private static bool TryGetAssemblyPath(PluginInfo mod, string assemblyName, out string fullPath) {
            fullPath = Path.Combine(mod.modPath, $"{assemblyName}.dll");
            try {
                if (File.Exists(fullPath)) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Returns the most human readable identifier that's available for the mod.
        ///
        /// Note: For sake of simplcity it doesn't auto-instantiate non-instantiated mods.
        /// </summary>
        /// 
        /// <param name="mod">The <see cref="PluginInfo"/> associated with the mod.</param>
        /// 
        /// <returns>The name of the mod, or it's dll name, or the name of parent folder.</returns>
        private static string GetName(PluginInfo mod) {
            try {
                return mod?.userModInstance != null
                    ? ((IUserMod)mod.userModInstance).Name
                    : mod.name ?? Path.GetFileName(mod.modPath);
            }
            catch {
                return "** error **";
            }
        }
    }
}
