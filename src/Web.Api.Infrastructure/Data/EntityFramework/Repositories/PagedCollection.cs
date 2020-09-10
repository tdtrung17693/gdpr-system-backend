using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
    private List<string> _sortableFields = null;
    private List<string> _filterableFields = null;
    private string _defaultSortingField = "";
    private Type _entityType;
    public PagedCollection(IQueryable<T> query)
    {
      _query = query;
      _entityType = typeof(T);
      var GetSortableFields = _entityType.GetMethod("GetSortableFields");
      var GetFilterableFields = _entityType.GetMethod("GetFilterableFields");
      var GetDefaultSortingField = _entityType.GetMethod("GetDefaultSortingField");
      _sortableFields = (List<string>)GetSortableFields.Invoke(null, null);
      _filterableFields = (List<string>)GetFilterableFields.Invoke(null, null);
      _defaultSortingField = GetDefaultSortingField != null ? (string)GetDefaultSortingField.Invoke(null, null) : "CreatedAt";
    }

    public void FilterBy(Expression<Func<T, bool>> exp)
    {
      _query = _query.Where(exp);
    }

    // Filter exactly value
    public void FilterBy<TFieldValue>(string fieldName, TFieldValue fieldValue)
    {
      fieldName = this._normalizeFieldName(fieldName);
      if (!_filterableFields.Contains(fieldName)) return;

      var fieldType = _entityType.GetProperty(fieldName).PropertyType;
      var fieldTypeCode = Type.GetTypeCode(fieldType);

      if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        fieldTypeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(fieldType));
      }

      switch (fieldTypeCode)
      {
        case TypeCode.Byte:
        case TypeCode.SByte:
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
        case TypeCode.Decimal:
        case TypeCode.Double:
        case TypeCode.Single:
        case TypeCode.Boolean:
          _query = _query.Where($"{fieldName} = @0", fieldValue);
          break;

        case TypeCode.String:
          _query = _query.Where($"{fieldName} like '%' + @0 + '%'", fieldValue);
          break;
        default:
          if (fieldType == typeof(Guid))
            _query = _query.Where($"{fieldName} = @0", fieldValue);
          else
          {
            Console.Error.WriteLine($"Filtering field {fieldName} have unsupported field type {fieldType}");
          }
          return;
      };
    }

    private string _normalizeFieldName(string fieldName)
    {
      return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
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
      //var nullParams = new object[] { };
      if (!_sortableFields.Contains(sortBy))
      {
        sortBy = _defaultSortingField;
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
