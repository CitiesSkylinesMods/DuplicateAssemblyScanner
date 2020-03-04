namespace DuplicateAssemblyScanner {
    using DuplicateAssemblyScanner.Util;
    using ICities;
    using JetBrains.Annotations;

    /// <summary>
    /// The main mod class which the game instantiates when the mod is enabled.
    /// </summary>
    public class UserMod : IUserMod {

        /// <summary>
        /// Gets mod name shown in content manager and options screens.
        /// Version defined in Solution Items > VersionInfo.cs file.
        /// </summary>
        [UsedImplicitly]
        public string Name => $"DAS v{typeof(UserMod).Assembly.GetName().Version.ToString(3)}";

        /// <summary>
        /// Gets mod description shown in content manager.
        /// </summary>
        [UsedImplicitly]
        public string Description => "Scans for duplicate assemblies in the app domain, which can cause bugs.";

        /// <summary>
        /// Called when mod is enabled.
        /// </summary>
        [UsedImplicitly]
        public void OnEnabled() {
            Log.Info("Enabled");
        }

        /// <summary>
        /// Called when settings UI is required.
        /// </summary>
        ///
        /// <param name="helper">Helper for creating UI.</param>
        [UsedImplicitly]
        public void OnSettingsUI(UIHelperBase helper) {
            Log.Info("SettingsUI");
            Settings.CreateUI(helper);
        }

        /// <summary>
        /// Called when mod is disabled.
        /// </summary>
        [UsedImplicitly]
        public void OnDisabled() {
            Log.Info("Disabled");
        }
    }
}
