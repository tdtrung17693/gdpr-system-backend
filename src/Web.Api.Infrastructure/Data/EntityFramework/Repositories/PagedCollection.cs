using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Web.Api.Core;
using Web.Api.Core.Interfaces;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
  class PagedCollection<T> : IPagedCollection<T>
  {
    private IQueryable<T> _query;
    private readonly IMapper _mapper;
    private int _pageSize;
    private int _totalItems = -1;
    private int _totalPages = -1;
    public PagedCollection(IQueryable<T> query)
    {
      _query = query;
    }

    public void FilterBy(Expression<Func<T, bool>> exp)
    {
      _query = _query.Where(exp);
    }

    /*
     * Page is base 1
    */
    public async Task<IEnumerable<T>> GetItemsForPage(int page, int pageSize = 10)
    {
      if (_pageSize != pageSize)
      {
        _pageSize = pageSize;
        _totalPages = -1;
      }

      var items = await _query
          .Skip((page - 1) * _pageSize)
          .Take(pageSize)
          .ToListAsync();

      return items;
    }

    public void SortBy(string sortBy, string sortOrder = "asc")
    {
      var t = typeof(T);
      //var nullParams = new object[] { };
      var sortableFields = (List<string>)t.GetMethod("GetSortableFields").Invoke(null, null);
      if (!sortableFields.Contains(sortBy))
      {
        sortBy = (string)t.GetMethod("GetDefaultSortingField").Invoke(null, null);
      }
      if (string.IsNullOrWhiteSpace(sortBy))
      {
        return;
      }

      var param = Expression.Parameter(typeof(T), "x");

      Expression body = param;
      foreach (var member in sortBy.Split('.'))
      {
        body = Expression.PropertyOrField(body, member);
      }

      var lambda = (dynamic)Expression.Lambda(body, param);

      if (sortOrder == "asc") _query = Queryable.OrderBy(_query, lambda);
      else _query = Queryable.OrderByDescending(_query, lambda);
    }

    public void SortBy<TKey>(Expression<Func<T, TKey>> exp, string sortOrder = "asc")
    {
      _query = sortOrder == "asc" ? _query.OrderBy(exp) : _query.OrderByDescending(exp);
    }

    public int TotalItems()
    {
      if (_totalItems == -1)
      {
        _totalItems = _query.Count();
      }

      return _totalItems;
    }

    public int TotalPages()
    {
      if (_totalPages == -1)
      {
        _totalPages = (int)Math.Ceiling(TotalItems() * 1.0 / _pageSize);
      }

      return _totalPages;
    }
  }
}
