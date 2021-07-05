using Newtonsoft.Json;
using System;
using System.Text;

namespace WSChat.Shared.Helpers
{
    public static class JsonHelper
    {
        public static T Desserialize<T>(Span<byte> buffer)
        {
            string json = Encoding.UTF8.GetString(buffer);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Span<byte> Serialize<T>(T data)
        {
            string json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
