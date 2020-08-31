using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Core
{
    public interface IPagedCollection<T>
    {
        Task<IEnumerable<T>> GetItemsForPage(int page);
        IPagedCollection<T> FilterBy(string filterString);
        IPagedCollection<T> SortBy(string sortBy, string sortOrder);
        int TotalItems();
        int TotalPages();
    }
}
