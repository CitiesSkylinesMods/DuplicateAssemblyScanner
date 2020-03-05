namespace DuplicateAssemblyScanner.Util {
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A nicer way to retrieve values of class instance members.
    /// Derived from: https://stackoverflow.com/questions/60236067/
    /// </summary>
    public class MemberValue {

        /// <summary>
        /// Default <see cref="BindingFlags"/> to use when retrieving for members.
        /// </summary>
        private const BindingFlags DefaultBindingFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase;

        /// <summary>
        /// Retrieves the value from a field/method/property member of an object.
        /// </summary>
        /// 
        /// <typeparam name="T">The value <see cref="Type"/>.</typeparam>
        /// 
        /// <param name="instance">An <see cref="object"/> instance.</param>
        /// <param name="member">The <see cref="MemberInfo"/> for the member to inspect.</param>
        /// 
        /// <returns>Returns the value of the <paramref name="member"/>, if found, otherwise the <c>default</c> value for <typeparamref name="T"/>.</returns>
        public static T GetValue<T>(object instance, MemberInfo member) {
            return member.MemberType switch
            {
                MemberTypes.Field => (T)(member as FieldInfo)?.GetValue(instance),
                MemberTypes.Method => (T)(member as MethodInfo)?.Invoke(instance, null),
                MemberTypes.Property => (T)(member as PropertyInfo)?.GetValue(instance, null),
                _ => default,
            };
        }

        /// <summary>
        /// Tries to get the value of a member of an object instance.
        /// </summary>
        /// 
        /// <typeparam name="T">The value <see cref="Type"/>.</typeparam>
        /// 
        /// <param name="type">The <see cref="Type"/> of the <paramref name="instance"/>.</param>
        /// <param name="instance">An <see cref="object"/> instance.</param>
        /// <param name="name">The name (case insensitive) of the member to inspect.</param>
        /// <param name="value">The retrieved member value, if successful.</param>
        /// <param name="flags">Optional: The <see cref="BindingFlags"/> to apply.</param>
        /// 
        /// <returns>Returns <c>true</c> if successful, otherwise <c>false</c>.</returns>
        /// 
        /// <example>
        /// Type type = asm.GetType("TrafficManager.TrafficManagerMod");
        /// object instance = Activator.CreateInstance(type);
        ///
        /// string branch;
        /// 
        /// if (TryGetMemberValue<string>(type, instance, "BRANCH", out string val)) {
        ///     branch = val;
        /// }
        ///
        /// (instance as IDisposable)?.Dispose();
        /// </example>
        public static bool TryGetMemberValue<T>(
            Type type,
            object instance,
            string name,
            out T value,
            BindingFlags flags = DefaultBindingFlags) {

            value = default;

            if (instance == null) {
                return false;
            }

            try {
                var member = type
                    .GetMember(name, flags)
                    .FirstOrDefault();

                if (member == null) {
                    return false;
                }

                value = GetValue<T>(instance, member);
            }
            catch {
                return false;
            }

            return true;
        }
    }
}
