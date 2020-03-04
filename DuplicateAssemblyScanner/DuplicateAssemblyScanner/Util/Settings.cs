namespace DuplicateAssemblyScanner.Util {
    using ColossalFramework.UI;
    using ICities;
    using System.Collections.Generic;

    public class Settings {

        /// <summary>
        /// Cache of assembly scan results.
        /// </summary>
        internal static Dictionary<string, List<string>> cacheDictionary_;

        /// <summary>
        /// Cache of whether duplicates were found by the scan.
        /// </summary>
        internal static bool cacheDuplicates_;

        /// <summary>
        /// Get (cached) results of assembly scan.
        /// </summary>
        /// 
        /// <param name="duplicatesFound">Will be <c>true</c> if duplicates found.</param>
        /// <returns>Dictionary of assembly lists keyed by assembly name.</returns>
        internal static Dictionary<string, List<string>> CacheScanResults(out bool duplicatesFound) {
            
            if (cacheDictionary_ == null) {
                cacheDictionary_ = Assemblies.Scan(out bool issues);
                cacheDuplicates_ = issues;
            }

            duplicatesFound = cacheDuplicates_;
            return cacheDictionary_;
        }

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

        internal static void NoOp(bool _) { }
    }
}
