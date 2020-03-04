namespace DuplicateAssemblyScanner.Util {
    using ColossalFramework.UI;
    using ICities;
    using System.Collections.Generic;
    using System.Reflection;

    public class Settings {

        /// <summary>
        /// Generate the options screen listing all the duplicates (if found).
        /// </summary>
        /// 
        /// <param name="helper">UI helper for building settings screen.</param>
        public static void CreateUI(UIHelperBase helper) {

            // assembly name -> list of assemblies with that name
            Dictionary<string, List<string>> results = Assemblies.Scan(out int problemsFound);

            helper.AddGroup($"{problemsFound} problems found.");

            if (problemsFound > 0) {

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
