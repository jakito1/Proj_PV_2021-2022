using System.Text.Json;

namespace NutriFitWeb.Services
***REMOVED***
    public static class SessionExtensions
    ***REMOVED***
        public static void Set<T>(this ISession session, string key, T? value)
        ***REMOVED***
            session.SetString(key, JsonSerializer.Serialize(value));
    ***REMOVED***

        public static T? Get<T>(this ISession session, string key)
        ***REMOVED***
            string? value = session.GetString(key);
            return value is null ? default : JsonSerializer.Deserialize<T>(value);
    ***REMOVED***
***REMOVED***
***REMOVED***
