using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Core
{
  public interface IPagedCollection<T>
  {
    Task<IEnumerable<T>> GetItemsForPage(int page, int pageSize);
    void FilterBy(Expression<Func<T, bool>> exp);
    void SortBy(string sortBy, string sortOrder);
    void SortBy<TKey>(Expression<Func<T, TKey>> exp, string sortOrder);

    int TotalItems();
    int TotalPages();
  }
}
