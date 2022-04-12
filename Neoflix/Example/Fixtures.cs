using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Neoflix.Example
{
    public static class Fixtures
    {
        static Fixtures()
        {
            var converter = new RecursiveListDictionaryConverter();
            var dictionaryConverter = new RecursiveDictionaryConverter();

            Favorites = JsonConvert.DeserializeObject<object[]>(File.ReadAllText("./example/favorites.json"),
                converter).OfType<Dictionary<string, object>>().ToArray();
            Genres = JsonConvert.DeserializeObject<object[]>(File.ReadAllText("./example/genre.json"),
                converter).OfType<Dictionary<string, object>>().ToArray();
            Goodfellas = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                File.ReadAllText("./example/goodfellas.json"), dictionaryConverter);
            Pacino = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                File.ReadAllText("./example/pacino.json"), dictionaryConverter);
            People = JsonConvert.DeserializeObject<object[]>(File.ReadAllText("./example/people.json"),
                converter).OfType<Dictionary<string, object>>().ToArray();
            Popular = JsonConvert.DeserializeObject<object[]>(File.ReadAllText("./example/popular.json"),
                converter).OfType<Dictionary<string, object>>().ToArray();
            Ratings = JsonConvert.DeserializeObject<object[]>(File.ReadAllText("./example/ratings.json"),
                converter).OfType<Dictionary<string, object>>().ToArray();
            Roles = JsonConvert.DeserializeObject<object[]>(File.ReadAllText("./example/roles.json"),
                converter).OfType<Dictionary<string, object>>().ToArray();
            Similar = JsonConvert.DeserializeObject<object[]>(File.ReadAllText("./example/similar.json"),
                converter).OfType<Dictionary<string, object>>().ToArray();
            User = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText("./example/user.json"),
                dictionaryConverter);
        }

        public static Dictionary<string, object>[]? Favorites { get; }
        public static Dictionary<string, object>[] Genres { get; }
        public static Dictionary<string, object> Goodfellas { get; }
        public static Dictionary<string, object> Pacino { get; }
        public static Dictionary<string, object>[]? People { get; }
        public static Dictionary<string, object>[]? Popular { get; }
        public static Dictionary<string, object>[]? Ratings { get; }
        public static Dictionary<string, object>[]? Roles { get; }
        public static Dictionary<string, object>[]? Similar { get; }
        public static Dictionary<string, object> User { get; }
    }

    #region loading code for json that is inline with driver

    internal class RecursiveListDictionaryConverter : JsonConverter<object[]>
    {
        public override void WriteJson(JsonWriter writer, object[]? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object[]? ReadJson(JsonReader reader, Type objectType, object[]? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var result = new List<object>();
            var dictionaryConverter = new RecursiveDictionaryConverter();

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    result.Add(dictionaryConverter.ReadJson(reader, typeof(Dictionary<string, object>), null, false,
                        serializer));
                    continue;
                }

                if (reader.TokenType == JsonToken.EndArray)
                    break;

                result.Add(reader.Value);
            }

            return result.ToArray();
        }
    }

    internal class RecursiveDictionaryConverter : JsonConverter<Dictionary<string, object>>
    {
        public override void WriteJson(JsonWriter writer, Dictionary<string, object>? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object>? ReadJson(JsonReader reader, Type objectType,
            Dictionary<string, object>? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            var name = string.Empty;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    name = reader.Value.ToString();
                    reader.Read();
                }

                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    result.Add(name, ReadJson(reader, typeof(Dictionary<string, object>), null, false, serializer));
                    continue;
                }

                if (reader.TokenType == JsonToken.StartArray)
                {
                    result.Add(name,
                        new RecursiveListDictionaryConverter().ReadJson(reader, typeof(object[]), null, false,
                            serializer));
                    continue;
                }

                result.Add(name, reader.Value);
            }

            return result;
        }
    }

    #endregion
}