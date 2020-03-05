namespace DuplicateAssemblyScanner {
    using ColossalFramework.UI;
    using DuplicateAssemblyScanner.Structs;
    using DuplicateAssemblyScanner.Util;
    using ICities;
    using System.Collections.Generic;

    /// <summary>
    /// Generates the settings screen based on cached results from the assembly scanner.
    /// </summary>
    public class Settings {
        // Markers used in checkbox labels
        private const string MARKER_BLANK = " ";
        private const string MARKER_ENABLED = "*";

        // Cache of scan results
        private static Dictionary<string, int> duplicates_;
        private static Dictionary<string, List<ModAssembly>> occurrences_;
        private static bool duplicatesFound_;

        /// <summary>
        /// Generate the options screen listing all the duplicates (if found).
        /// </summary>
        /// 
        /// <param name="helper">UI helper for building settings screen.</param>
        public static void CreateUI(UIHelperBase helper) {

            UIHelperBase group;

            if (CacheScan(
                out Dictionary<string, int> duplicates,
                out Dictionary<string, List<ModAssembly>> occurrences)) {

                group = helper.AddGroup("Duplicate assemblies were detected.");

                Disable((UICheckBox)group.AddCheckbox("= Assembly is loaded in to app domain", true, NoOp));
                Disable((UICheckBox)group.AddCheckbox("= Assembly not loaded", false, NoOp));

                foreach (KeyValuePair<string, int> duplicate in duplicates) {

                    if (duplicate.Value > 1) {

                        if (occurrences.TryGetValue(duplicate.Key, out List<ModAssembly> mods)) {

                            RenderDetails(helper, duplicate.Key, duplicate.Value, mods);
                        }
                    }
                }
            } else {
                helper.AddGroup("No duplicates.");
            }
        }

        /// <summary>
        /// A dummy click handler for checkboxes. Does nothing.
        /// </summary>
        /// 
        /// <param name="_">Ignored paramter.</param>
        internal static void NoOp(bool _) {
            // do nothing
        }

        /// <summary>
        /// Renders UI for a given assembly and the mods that contain it.
        /// </summary>
        /// 
        /// <param name="helper">Helper for creating UI.</param>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="loaded">How many copies of the assembly are loaded.</param>
        /// <param name="list">The list of mods containing the assembly.</param>
        internal static void RenderDetails(
            UIHelperBase helper,
            string assemblyName,
            int loaded,
            List<ModAssembly> list) {

            UIHelperBase group = helper.AddGroup($"{list.Count} × {assemblyName} ({loaded} loaded):");

            foreach (ModAssembly item in list) {
                string v = item.AsmDetails.Version.ToString();
                string n = item.ModName;
                string e = item.ModEnabled ? MARKER_ENABLED : MARKER_BLANK;

                Disable((UICheckBox)group.AddCheckbox($"{v} {e} {n}", item.AsmLoaded, NoOp));
            }
        }

        /// <summary>
        /// Disables a checkbox and sets its opacity based on state.
        /// </summary>
        /// 
        /// <param name="box">The checkbox to disable.</param>
        private static void Disable(UICheckBox box) {
            box.readOnly = true;
            box.opacity = box.isChecked ? 1f : 0.5f;
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
            
            if (duplicates_ == null) {
                duplicatesFound_ = Assemblies.Scan(
                    out Dictionary<string, int> dup,
                    out Dictionary<string, List<ModAssembly>> occ);

                duplicates_ = dup;
                occurrences_ = occ;
            }

            duplicates = duplicates_;
            occurrences = occurrences_;
            return duplicatesFound_;
        }
    }
}
