using Newtonsoft.Json;

namespace M_BMilkStoreClient.Service
{
    public static class SessionService
    {
        public static void SetSessionObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetSessionObjectAsJson<T>( this ISession session, string key )
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
