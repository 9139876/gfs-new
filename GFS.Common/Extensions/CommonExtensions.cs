﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GFS.Common.Extensions
{
    public static class CommonExtensions
    {
        public static void RequiredNotNull(this object value, string? paramName = null, object? @object = null)
        {
            paramName ??= nameof(value);
            
            if (value == null)
                throw new InvalidOperationException(Compose(paramName, @object));
        }

        public static void RequiredNotNull(this string? value, string paramName, object? @object = null)
        {
            if (string.IsNullOrEmpty(value?.Trim()))
                throw new InvalidOperationException(Compose(paramName, @object));
        }

        public static string Serialize(this object value, Action<JsonSerializerSettings>? configureSettings = null)
        {
            var jsonSerializerSettings = JsonSerializerDefaultSettings;
            if (IfContainAbstractMembers(value.GetType()))
            {
                jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;
            }

            configureSettings?.Invoke(jsonSerializerSettings);

            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

        public static T? Deserialize<T>(this string value)
            where T : class
        {
            var jsonSerializerSettings = JsonSerializerDefaultSettings;

            if (IfContainAbstractMembers(value.GetType()))
                jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;

            return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings);
        }

        public static List<KeyValuePair<string, string?>>? ToPropertiesCollection(this object? obj)
        {
            return obj
                ?.GetType()
                .GetProperties()
                .Select(x => new KeyValuePair<string, string?>(x.Name, x.GetValue(obj)?.ToString()))
                .ToList();
        }

        public static T ToModel<T>(this List<KeyValuePair<string, string>> propertiesCollection)
            where T : class, new()
        {
            var instance = new T();

            var instanceProperties = instance.GetType().GetProperties();

            foreach (var (key, value) in propertiesCollection)
            {
                var currentProperty = instanceProperties.SingleOrDefault(x => x.Name == key);

                if (currentProperty != null)
                    currentProperty.SetValue(instance, value);
            }

            return instance;
        }

        private static string Compose(string paramName, object? @object)
        {
            return @object == null ? paramName : $"{paramName} for object:{JsonConvert.SerializeObject(@object)}";
        }

        private static bool IfContainAbstractMembers(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                var propertyType = property.PropertyType;

                if (propertyType.IsAbstract || propertyType.IsInterface)
                {
                    return true;
                }

                if (propertyType.Name != "DateTime" && IfContainAbstractMembers(propertyType)
                   ) // DateTime содержит в себе свойство DateTime => бесконечная рекурсия
                {
                    return true;
                }
            }

            return false;
        }

        private static readonly JsonSerializerSettings JsonSerializerDefaultSettings = new()
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() },
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };
    }
}