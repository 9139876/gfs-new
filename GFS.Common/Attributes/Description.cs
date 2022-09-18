using System.Reflection;

namespace GFS.Common.Attributes
{
    public class Description : Attribute
    {
        public string Text;

        public Description(string text)
        {
            Text = text;
        }

        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description), false);

                if (attrs != null && attrs.Length > 0)
                    return ((Description) attrs[0]).Text;
            }

            return en.ToString();
        }

        public static string[] GetAllDescptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(v => GetDescription(v))
                .ToArray();
        }

        public static T GetValueFromDescription<T>(string desc) where T : Enum
        {
            foreach (T val in Enum.GetValues(typeof(T)))
                if (GetDescription(val) == desc)
                    return val;

            throw new ArgumentException($"Перечисление {typeof(T).Name} не имеет значения с описанием '{desc}'");
        }

        public static bool TryGetValueFromDescription<T>(string desc, out T value) where T : Enum
        {
            value = Enum.GetValues(typeof(T)).Cast<T>().Where(t => GetDescription(t) == desc).FirstOrDefault();
            return !Equals(value, default(T));
        }
    }
}