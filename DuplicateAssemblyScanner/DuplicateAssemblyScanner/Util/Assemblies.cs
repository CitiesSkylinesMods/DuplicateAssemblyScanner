namespace DuplicateAssemblyScanner.Util {
    using DuplicateAssemblyScanner.Structs;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Utility functions for assemblies.
    /// </summary>
    public class Assemblies {

        /// <summary>
        /// Scans the app domain to find duplicate assemblies.
        ///
        /// When duplicates are detected, a list of mods containing the assembly is compiled.
        /// </summary>
        /// 
        /// <param name="duplicates">Number of sightings per assembly name.</param>
        /// <param name="occurrences">List the mods that contain each duplicate assembly.</param>
        /// 
        /// <returns>Returns <c>true</c> if at least one duplciate was found, otherwise <c>false</c>.</returns>
        internal static bool Scan(
            out Dictionary<string, int> duplicates,
            out Dictionary<string, List<ModAssembly>> occurrences) {

            Log.Info("Scanning app domain assemblies for duplicates...\n\n[>] = Assembly loaded in RAM, [*] = Mod enabled");

            bool duplicatesFound = false;

            // assembly -> num duplicates
            duplicates = new Dictionary<string, int>();

            // assembly -> mods that contain it
            occurrences = new Dictionary<string, List<ModAssembly>>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies) {

                try {
                    string assemblyName = asm.GetName().Name;

                    if (duplicates.TryGetValue(assemblyName, out int count)) {

                        if (count == 1) {
                            duplicatesFound = true;

                            List<ModAssembly> mods = Mods.FindAnyContainingAssembly(assemblyName);

                            occurrences.Add(assemblyName, mods);
                        }

                        duplicates[assemblyName] += 1;

                    } else {

                        duplicates.Add(assemblyName, 1);

                    }

                } catch (Exception e) {
                    Log.Error(e.ToString());
                }
            }

            return duplicatesFound;
        }

        /// <summary>
        /// Attempts to get info of an assembly on disk, _without loading it in to app domain_.
        /// </summary>
        /// 
        /// <param name="fullPath">Full path to assembly, including file name.</param>
        /// <param name="asmDetails">If successful, the <see cref="AssemblyName"/> for the assembly.</param>
        /// 
        /// <returns>If successful, returns <c>true</c>; otherwise <c>false</c>.</returns>
        internal static bool TryGetAssemblyDetailsWithoutLoading(
            string fullPath,
            out AssemblyName asmDetails) {

            try {
                asmDetails = AssemblyName.GetAssemblyName(fullPath);
                return true;
            } catch {
                asmDetails = default;
                return false;
            }
        }
    }
}
