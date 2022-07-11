using System;

namespace GFS.Common.Attributes
{
    /// <summary>
    /// Добавление этого атрибута регистрирует элемент в контейнере как Singleton.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class SingletonRegistrationAttribute : Attribute
    {
    }
}