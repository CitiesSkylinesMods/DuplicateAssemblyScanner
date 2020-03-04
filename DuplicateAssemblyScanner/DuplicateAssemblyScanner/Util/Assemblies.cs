namespace DuplicateAssemblyScanner.Util {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class Assemblies {

        /// <summary>
        /// Scans app domain assemblies and compiles a dictionary keyed by assembly name;
        /// each value is a list of assemblies matching the name.
        /// </summary>
        /// 
        /// <returns>Dictionary of assembly lists keyed by assembly name.</returns>
        public static Dictionary<string, List<string>> Scan(out int problemsFound) {

            problemsFound = 0;

            // assembly name -> list of assemblies with that name
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

            StringBuilder log = new StringBuilder(1500);

            log.Append("Scanning assemblies...");

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies) {

                try {
                    AssemblyName details = asm.GetName();

                    string name = details.Name;
                    string ver = details.Version.ToString();

                    if (results.TryGetValue(name, out List<string> matches)) {

                        if (matches.Count == 1) {
                            ++problemsFound;
                            log.Append($"\n  Duplicate: '{name}' v{matches.First<string>()}");
                        }

                        matches.Add(ver);
                        log.Append($"\n  Duplicate: '{name}' v{ver}");

                    } else {
                        results.Add(name, new List<string>() {
                            { ver }
                        });
                    }
                } catch (Exception e) {
                    Log.Error(e.ToString());
                }
            }

            Log.Info(log.ToString());

            return results;
        }
    }
}
