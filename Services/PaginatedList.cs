using Microsoft.EntityFrameworkCore;

namespace NutriFitWeb.Services
***REMOVED***
    public class PaginatedList<T> : List<T>
    ***REMOVED***
        public int PageIndex ***REMOVED*** get; private set; ***REMOVED***
        public int TotalPages ***REMOVED*** get; private set; ***REMOVED***

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        ***REMOVED***
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
    ***REMOVED***

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        ***REMOVED***
            int count = await source.CountAsync();
            List<T>? items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
    ***REMOVED***
***REMOVED***
***REMOVED***