namespace Nemezida.Rationalizator.Web.Extensions
{
    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class QueryableExtensions
    {
        public static Task<List<TDest>> ToListAsync<TSource, TDest>(this IQueryable<TSource> source, IMapper mapper)
        {
            return mapper.ProjectTo<TDest>(source).ToListAsync();
        }

        public static List<TDest> ToList<TSource, TDest>(this IQueryable<TSource> source, IMapper mapper)
        {
            return mapper.ProjectTo<TDest>(source).ToList();
        }
    }
}
