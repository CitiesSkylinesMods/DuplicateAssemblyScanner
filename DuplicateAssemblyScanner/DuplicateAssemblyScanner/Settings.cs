namespace DuplicateAssemblyScanner {
    using ColossalFramework.UI;
    using DuplicateAssemblyScanner.Structs;
    using DuplicateAssemblyScanner.Util;
    using ICities;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Generates the settings screen based on cached results from the assembly scanner.
    /// </summary>
    public class Settings {
        // Markers used in checkbox labels and log entries
        private const string MARKER_DOUBLEBLANK = "  ";
        private const string MARKER_BLANK = " ";
        private const string MARKER_ENABLED = "*";
        private const string MARKER_LOADED = ">";

        // Cache of scan results
        private static Dictionary<string, int> _duplicates;
        private static Dictionary<string, List<ModAssembly>> _occurrences;
        private static bool _duplicatesFound;

        /// <summary>
        /// Generate the options screen listing all the duplicates (if found).
        /// </summary>
        /// 
        /// <param name="helper">UI helper for building settings screen.</param>
        public static void CreateUI(UIHelperBase helper) {

            UIHelperBase group;

            if (CacheScan(out var duplicates, out var occurrences)) {

                group = AddGroup("Duplicate assemblies were detected.", helper);

                AddCheckbox("= Assembly is loaded in to app domain", true, group);
                AddCheckbox("= Assembly not loaded", false, group);

                foreach (var duplicate in duplicates) {

                    if (duplicate.Value > 1) {

                        if (occurrences.TryGetValue(duplicate.Key, out var mods)) {

                            RenderDetails(helper, duplicate.Key, duplicate.Value, mods);
                        }
                    }
                }
            } else {
                AddGroup("No duplicates.", helper);
            }
        }

        /// <summary>
        /// Renders UI for a given assembly and the mods that contain it.
        /// </summary>
        /// 
        /// <param name="helper">Helper for creating UI.</param>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="loaded">How many copies of the assembly are loaded.</param>
        /// <param name="list">The list of mods containing the assembly.</param>
        private static void RenderDetails(
            UIHelperBase helper,
            string assemblyName,
            int loaded,
            List<ModAssembly> list) {

            StringBuilder log = new StringBuilder(3000);

            log.AppendFormat("\n{0} ({1} loaded, {2} mods):\n", assemblyName, loaded, list.Count);
            log.Insert(log.Length, "-", 105);
            log.Append("\n    Asm Version  MD5 Hash                         /Mod Folder   Mod name\n");
            log.Insert(log.Length, "-", 105);

            UIHelperBase group = AddGroup($"{assemblyName} ({loaded} loaded, {list.Count} mods):", helper);

            foreach (ModAssembly item in list) {
                string v = item.AsmDetails.Version.ToString();
                string n = item.ModName;
                string e = item.ModEnabled ? MARKER_ENABLED : MARKER_DOUBLEBLANK;

                log.AppendFormat(
                    "\n{0} {1} {2} {3} /{4} {5}",
                    item.AsmLoaded ? MARKER_LOADED : MARKER_BLANK,
                    item.ModEnabled ? MARKER_ENABLED : MARKER_BLANK,
                    v.PadRight(12),
                    item.AsmMD5Hash,
                    item.Folder.PadRight(12),
                    n);

                AddCheckbox($"{v} {e} {n}", item.AsmLoaded, group);
            }

            Log.Info(log.ToString());
        }

        private static UIHelperBase AddGroup(string caption, UIHelperBase parent) {
            return parent.AddGroup(caption);
        }

        private static void AddCheckbox(string caption, bool enabled, UIHelperBase group) {
            UICheckBox box = (UICheckBox)group.AddCheckbox(caption, enabled, (bool _) => { });
            box.readOnly = true;
            box.opacity = enabled ? 1f : 0.4f;
        }

        /// <summary>
        /// Retrieves (and creates if necessary) cached results of assembly scanner.
        /// </summary>
        /// 
        /// <param name="duplicates">Number of sightings per assembly name.</param>
        /// <param name="occurrences">List the mods that contain each duplicate assembly.</param>
        /// 
        /// <returns>Returns <c>true</c> if at least one duplciate was found, otherwise <c>false</c>.</returns>
        private static bool CacheScan(
            out Dictionary<string, int> duplicates,
            out Dictionary<string, List<ModAssembly>> occurrences) {
            
            if (_duplicates == null) {
                _duplicatesFound = Assemblies.Scan(out var dup, out var occ);
                _duplicates = dup;
                _occurrences = occ;
            }

            duplicates = _duplicates;
            occurrences = _occurrences;
            return _duplicatesFound;
        }
    }
}
