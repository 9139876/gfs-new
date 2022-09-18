using GFS.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GFS.EF.Extensions
{
    public static class QueryableExtensions
    {
        public static  async Task<T> SingleOrFailAsync<T>(this IQueryable<T> query)
        {
            try
            {
                return await query.SingleAsync();
            }
            catch (InvalidOperationException)
            {
                throw new SingleException(typeof(T));
            }
        }

        public static async Task<T> GetOrFailAsync<T>(this IQueryable<T> query)
        {
            var entity = await query.FirstOrDefaultAsync();
            if (entity == null) throw new NotFoundException(typeof(T));
            return entity;
        }
    }
}