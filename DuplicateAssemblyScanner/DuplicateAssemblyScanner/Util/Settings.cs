namespace DuplicateAssemblyScanner.Util {
    using ColossalFramework.UI;
    using ICities;
    using System.Collections.Generic;

    /// <summary>
    /// Generates the settings screen based on cached results from the assembly scanner.
    /// </summary>
    public class Settings {

        /// <summary>
        /// Cache of assembly scan results.
        /// </summary>
        private static Dictionary<string, List<string>> cacheDictionary_;

        /// <summary>
        /// Cache of whether duplicates were found by the scan.
        /// </summary>
        private static bool cacheDuplicates_;

        /// <summary>
        /// Generate the options screen listing all the duplicates (if found).
        /// </summary>
        /// 
        /// <param name="helper">UI helper for building settings screen.</param>
        public static void CreateUI(UIHelperBase helper) {

            // assembly name -> list of assemblies with that name
            Dictionary<string, List<string>> results = CacheScanResults(out bool duplicatesFound);

            helper.AddGroup(duplicatesFound
                ? "Duplicate assemblies were detected."
                : "No duplicates.");

            if (duplicatesFound) {

                UIHelperBase group;
                UICheckBox checkbox;
                int matches;

                foreach (KeyValuePair<string, List<string>> entry in results) {

                    matches = entry.Value.Count;

                    if (matches > 1) {
                        group = helper.AddGroup($"{matches} × '{entry.Key}' assemblies");

                        foreach (string ver in entry.Value) {
                            checkbox = (UICheckBox)group.AddCheckbox(ver, true, NoOp);
                            checkbox.isEnabled = false;
                        }
                    }
                }
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
        /// Get (cached) results of assembly scan.
        /// </summary>
        /// 
        /// <param name="duplicatesFound">Will be <c>true</c> if duplicates found.</param>
        /// <returns>Dictionary of assembly lists keyed by assembly name.</returns>
        private static Dictionary<string, List<string>> CacheScanResults(out bool duplicatesFound) {
            
            if (cacheDictionary_ == null) {
                cacheDictionary_ = Assemblies.Scan(out bool issues);
                cacheDuplicates_ = issues;
            }

            duplicatesFound = cacheDuplicates_;
            return cacheDictionary_;
        }
    }
}