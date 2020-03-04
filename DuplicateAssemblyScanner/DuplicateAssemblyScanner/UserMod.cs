namespace DuplicateAssemblyScanner {
    using ColossalFramework.UI;
    using DuplicateAssemblyScanner.Util;
    using ICities;
    using JetBrains.Annotations;
    using UnityEngine.SceneManagement;

    public class UserMod : IUserMod {

        [UsedImplicitly]
        public string Name => "DAS";

        [UsedImplicitly]
        public string Description => "Scans for duplicate assemblies in the app domain, which can cause bugs.";

        [UsedImplicitly]
        public void OnEnabled() {
            Log.Info("Enabled");
        }

        [UsedImplicitly]
        public void OnSettingsUI(UIHelperBase helper) {
            if (SceneManager.GetActiveScene().name == "Game") {
                return;
            }
            Log.Info("SettingsUI");

            //todo: actually display stuff in the options screen
            _ = Assemblies.Scan();
        }

        [UsedImplicitly]
        public void OnDisabled() {
            Log.Info("Disabled");
        }
    }
}