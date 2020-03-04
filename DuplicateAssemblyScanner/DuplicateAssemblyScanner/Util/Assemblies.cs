namespace DuplicateAssemblyScanner.Util {
    using ColossalFramework;
    using ColossalFramework.Plugins;
    using ICities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using UnityEngine.SceneManagement;
    using static ColossalFramework.Plugins.PluginManager;

    public class Assemblies {

        /// <summary>
        /// Scans app domain assemblies and compiles a dictionary keyed by assembly name;
        /// each value is a list of assemblies matching the name.
        /// </summary>
        /// 
        /// <returns>Dictionary of assembly lists keyed by assembly name.</returns>
        public static Dictionary<string, List<string>> Scan(out int problemsFound) {
            Log.Info("Scanning app domain assemblies for duplicates...");
            problemsFound = 0;

            // assembly name -> list of assemblies with that name
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

            bool detailedLogging = SceneManager.GetActiveScene().name != "Game";

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies) {

                try {
                    AssemblyName details = asm.GetName();

                    string name = details.Name;
                    string ver = details.Version.ToString();

                    if (results.TryGetValue(name, out List<string> matches)) {

                        if (matches.Count == 1) {
                            ++problemsFound;
                            if (detailedLogging) {
                                FindModsWithAssembly(name);
                            }
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

            Log.Info($"{problemsFound} problem(s) found.");
            return results;
        }

        internal static void FindModsWithAssembly(string matchName) {
            Log.Info($"# '{matchName}' assembly exists in mods:");

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
                        if (name == matchName) {
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
                return "Error getting mod name!";
            }
        }
    }
}
