namespace DuplicateAssemblyScanner.Util {
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Utility functions for files.
    /// </summary>
    public class Md5 {

        /// <summary>
        /// Tries to get the MD5 hash for <paramref name="fullPath"/>.
        /// </summary>
        /// 
        /// <param name="fullPath">Full path of file, including file name.</param>
        /// <param name="md5Hash">If successful, the calculated MD5 hasn for <paramref name="fullPath"/>.</param>
        /// <param name="removeHyphens">If <c>true</c>, hyphens will be removed form the returned string.</param>
        /// 
        /// <returns>Returns <c>true</c> if successful, otherwise <c>false</c>.</returns>
        internal static bool TryGetHash(string fullPath, out string md5Hash, bool removeHyphens = false) {
            try {
                md5Hash = CaclulateHash(fullPath, removeHyphens);
                return true;
            } catch {
                md5Hash = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Calculates MD5 hash for <paramref name="fullPath"/>. Throws exception if file doesn't exist.
        ///
        /// Robbed from FPS_Booster mod by krzychu124.
        /// </summary>
        /// 
        /// <param name="fullPath">Full path of file, including file name.</param>
        /// <param name="removeHyphens">If <c>true</c>, hyphens will be removed form the returned string.</param>
        ///
        /// <exception cref="UnauthorizedAccessException"><paramref name="fullPath"/> access denied or not a file.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="fullPath"/> not found.</exception>
        /// <exception cref="IOException"><paramref name="fullPath"/> was already open and cannot be read.</exception>
        /// 
        /// <returns>Returns the MD5 hash of the file.</returns>
        private static string CaclulateHash(
            string fullPath,
            bool removeHyphens = false) {

            using MD5 md5 = MD5.Create();
            using FileStream stream = File.OpenRead(fullPath);
            byte[] hash = md5.ComputeHash(stream);
            return removeHyphens
                ? BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant()
                : BitConverter.ToString(hash).ToLowerInvariant();
        }
    }
}
