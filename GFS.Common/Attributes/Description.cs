namespace GFS.Common.Attributes;

public class Description : Attribute
{
    private readonly string _text;

    public Description(string text)
    {
        _text = text;
    }

    public static string GetDescription(Enum @enum)
    {
        var type = @enum.GetType();

        var memInfo = type.GetMember(@enum.ToString());

        if (memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(Description), false);

            if (attrs.Length > 0)
                return ((Description) attrs[0])._text;
        }

        return @enum.ToString();
    }

    public static string[] GetAllDescriptions<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(v => GetDescription(v))
            .ToArray();
    }

    public static T GetValueFromDescription<T>(string desc) where T : Enum
    {
        foreach (T? val in Enum.GetValues(typeof(T)))
            if (GetDescription(val) == desc)
                return val;

        throw new ArgumentException($"Перечисление {typeof(T).Name} не имеет значения с описанием '{desc}'");
    }

    public static bool TryGetValueFromDescription<T>(string desc, out T? value) where T : Enum
    {
        value = Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(t => GetDescription(t) == desc);
        return !Equals(value, default(T));
    }
}