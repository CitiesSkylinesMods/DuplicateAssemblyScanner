namespace DuplicateAssemblyScanner.Structs {
    using DuplicateAssemblyScanner.Util;
    using System.Reflection;
    using static ColossalFramework.Plugins.PluginManager;

    /// <summary>
    /// Contains details of an assembly within a mod.
    /// </summary>
    public struct ModAssembly {

        // ------------- Mod --------------

        /// <summary>
        /// Path of containing folder for the mod (<see cref="PluginInfo.modPath"/>).
        /// </summary>
        public string ModPath;

        /// <summary>
        /// Name of the mod (<see cref="Mods.GetNameOfMod()"/>).
        /// </summary>
        public string ModName;

        /// <summary>
        /// Enabled state of the mod (<see cref="PluginInfo.isEnabled"/>).
        /// </summary>
        public bool ModEnabled;

        /// <summary>
        /// The name of the parent folder that contains the mod.
        /// </summary>
        public string Folder;

        // ---------- Assembly ------------

        /// <summary>
        /// Assembly details (name, version, etc).
        /// </summary>
        public AssemblyName AsmDetails;

        /// <summary>
        /// Loaded state of assembly.
        /// </summary>
        public bool AsmLoaded;

        /// <summary>
        /// MD5 hash for assembly file.
        /// </summary>
        public string AsmMD5Hash;
    }
}
