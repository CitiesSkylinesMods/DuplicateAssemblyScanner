namespace DuplicateAssemblyScanner.Util {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class Assemblies {
        public static Dictionary<string, List<Assembly>> Scan() {

            // assembly nane -> assemblies[]
            Dictionary<string, List<Assembly>> Duplicates = new Dictionary<string, List<Assembly>>();

            StringBuilder log = new StringBuilder(3000);

            log.Append("Scanning assemblies...");

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies) {

                try {
                    AssemblyName details = asm.GetName();

                    string name = details.Name;
                    string ver = details.Version.ToString();

                    if (Duplicates.TryGetValue(name, out List<Assembly> value)) {

                        value.Add(asm);
                        log.Append($"\n  Duplicate: '{name}' v{ver}");

                    } else {

                        Duplicates.Add(name, new List<Assembly>() {
                            { asm }
                        });

                    }

                } catch (Exception e) {
                    Log.Error(e.ToString());
                }
            }

            Log.Info(log.ToString());

            return Duplicates;
        }
    }
}
