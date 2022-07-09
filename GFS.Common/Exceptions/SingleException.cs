using System;

namespace GFS.Common.Exceptions
{
    /// <summary>
    /// Thrown if an object is violates the requirement Single.
    /// </summary>
    public class SingleException : Exception
    {
        public SingleException()
        {
        }

        public SingleException(Type type)
            : base($"{type} violates the requirement Single")
        {
        }

        public SingleException(Type type, object key)
            : base($"{type} violates the requirement Single by key {key}")
        {
        }

        public SingleException(string name)
            : base($"{name} violates the requirement Single")
        {
        }
    }
}