using Microsoft.EntityFrameworkCore;

namespace NutriFitWeb.Services
{
    /// <summary>
    /// PaginatedList<T> class, derives from List<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// Gets and Sets the page index.
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// Gets and Sets the total pages to be displayed in the list.
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items">List of items to be displayed</param>
        /// <param name="count">Count of items</param>
        /// <param name="pageIndex">Index of the page</param>
        /// <param name="pageSize">Size of items that can be present in the list</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }
        /// <summary>
        /// Flag to know if the current page has any previous pages.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;
        /// <summary>
        /// Flag to know if the current page is the last one.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;
        /// <summary>
        /// Creates a Paginated list
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int count = await source.CountAsync();
            List<T>? items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}