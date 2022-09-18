namespace GFS.Common.Exceptions
{
    /// <summary>
    /// Thrown if an object could not be found.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(Type type)
            : base($"{type} not found")
        {
        }

        public NotFoundException(Type type, object key)
            : base($"{type} not found by key {key}")
        {
        }

        public NotFoundException(string name)
            : base($"{name} not found")
        {
        }
    }
}