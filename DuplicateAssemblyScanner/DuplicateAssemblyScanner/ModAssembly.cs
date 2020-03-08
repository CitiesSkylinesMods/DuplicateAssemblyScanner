namespace DuplicateAssemblyScanner.Structs {
    using DuplicateAssemblyScanner.Util;
    using System.Reflection;

    /// <summary>
    /// Contains details of an assembly within a mod.
    /// </summary>
    public class ModAssembly {

        /// <summary>
        /// Initializes a new instance of the <see cref="ModAssembly"/> class.
        /// </summary>
        public ModAssembly() { }

        // ------------- Mod --------------

        /// <summary>
        /// Gets or sets the path of containing folder for the mod.
        /// </summary>
        public string ModPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the mod.
        /// </summary>
        public string ModName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mod is enabled or not.
        /// </summary>
        public bool ModEnabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent folder that contains the mod.
        /// </summary>
        public string Folder { get; set; }

        // ---------- Assembly ------------

        /// <summary>
        /// Gets or sets the assembly details (name, version, etc).
        /// </summary>
        public AssemblyName AsmDetails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the assembly is loaded in the app domain or not.
        /// </summary>
        public bool AsmLoaded { get; set; }

        /// <summary>
        /// Gets or sets MD5 hash for assembly file.
        /// </summary>
        public string AsmMD5Hash { get; set; }
    }
}
