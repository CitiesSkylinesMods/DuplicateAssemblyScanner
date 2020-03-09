namespace DuplicateAssemblyScanner {
    using ColossalFramework;
    using ColossalFramework.Plugins;
    using ICities;
    using JetBrains.Annotations;
    using UnityEngine.SceneManagement;

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
        public string Description => "Scans for duplicate assemblies in the app domain.";

        /// <summary>
        /// Called when mod is enabled.
        /// </summary>
        [UsedImplicitly]
        public void OnEnabled() {
            Log.Debug("Enabled");
            PluginManager plugins = Singleton<PluginManager>.instance;
            plugins.eventPluginsChanged += OnModsChanged;
            plugins.eventPluginsStateChanged += OnModsChanged;
        }

        /// <summary>
        /// Called when settings UI is required.
        /// </summary>
        ///
        /// <param name="helper">Helper for creating UI.</param>
        [UsedImplicitly]
        public void OnSettingsUI(UIHelperBase helper) {
            Log.Info($"[SettingsUI] {SceneManager.GetActiveScene().name}");
            Settings.CreateUI(helper);
        }

        /// <summary>
        /// Logs out when mods change, as they cause settings to be recreated.
        /// </summary>
        public void OnModsChanged() {
            Log.Info($"[OnModsChanged] {SceneManager.GetActiveScene().name}");

            // force rescan:
            Settings._duplicates = null;
        }

        /// <summary>
        /// Called when mod is disabled.
        /// </summary>
        [UsedImplicitly]
        public void OnDisabled() {
            Log.Debug("Disabled");
            PluginManager plugins = Singleton<PluginManager>.instance;
            plugins.eventPluginsChanged -= OnModsChanged;
            plugins.eventPluginsStateChanged -= OnModsChanged;
        }
    }
}
