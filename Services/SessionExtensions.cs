using System.Text.Json;

namespace NutriFitWeb.Services
{
    /// <summary>
    /// SessionExtensions class
    /// </summary>
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T? value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        /// <summary>
        /// Gets the value of a session item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key">The key to get the value from</param>
        /// <returns></returns>
        public static T? Get<T>(this ISession session, string key)
        {
            string? value = session.GetString(key);
            return value is null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
