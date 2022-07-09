using System;

namespace GFS.Common.Extensions
{
    public static class ExceptionExtension
    {
        public static void ThrowIf<T, TException>(this T value, Predicate<T> emitter, TException e)
            where TException : Exception
        {
            if (emitter(value))
                throw e;
        }

        public static void ThrowIf<T, TException>(this T value, Predicate<T> emitter)
            where TException : Exception, new()
        {
            if (emitter(value))
                throw new TException();
        }

        public static void ThrowIf<T, TException>(this T value, Predicate<T> emitter, Func<TException> exceptionFactory)
            where TException : Exception
        {
            if (emitter(value))
                throw exceptionFactory();
        }

        /// <exception cref="ArgumentNullException"></exception>  
        public static void ThrowIfNull(this object value)
        {
            value.ThrowIfNull(() => new ArgumentNullException(nameof(value)));
        }

        public static void ThrowIfNull<TException>(this object value)
            where TException : Exception, new()
        {
            value.ThrowIfNull(new TException());
        }

        public static void ThrowIfNull<TException>(this object value, TException e)
            where TException : Exception
        {
            value.ThrowIf(v => v == null, e);
        }

        public static void ThrowIfNull<TException>(this object value, Func<TException> exceptionFactory)
            where TException : Exception
        {
            value.ThrowIfNull(exceptionFactory());
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfFalse(this bool value)
        {
            value.ThrowIfFalse(() => new ArgumentOutOfRangeException(nameof(value)));
        }

        public static void ThrowIfFalse<TException>(this bool value)
            where TException : Exception, new()
        {
            value.ThrowIfFalse(new TException());
        }

        public static void ThrowIfFalse<TException>(this bool value, TException e)
            where TException : Exception
        {
            value.ThrowIf(v => !v, e);
        }

        public static void ThrowIfFalse<TException>(this bool value, Func<TException> exceptionFactory)
            where TException : Exception
        {
            value.ThrowIfFalse(exceptionFactory());
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>    
        public static void ThrowIfTrue(this bool value)
        {
            value.ThrowIfTrue(() => new ArgumentOutOfRangeException(nameof(value)));
        }

        public static void ThrowIfTrue<TException>(this bool value)
            where TException : Exception, new()
        {
            value.ThrowIfTrue(new TException());
        }

        public static void ThrowIfTrue<TException>(this bool value, TException e)
            where TException : Exception
        {
            value.ThrowIf(v => v, e);
        }

        public static void ThrowIfTrue<TException>(this bool value, Func<TException> exceptionFactory)
            where TException : Exception
        {
            value.ThrowIfTrue(exceptionFactory());
        }
    }
}