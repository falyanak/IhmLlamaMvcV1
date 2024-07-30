using Newtonsoft.Json;

namespace IhmLlamaMvc.Mvc.Extensions;

public static class SessionExtensions
{
    public static void SetJson<T>(this ISession session, string key, T value)
    {
        //  session.SetString(key, JsonConvert.SerializeObject(value));

        session.SetString(key,
        JsonConvert.SerializeObject(value, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}