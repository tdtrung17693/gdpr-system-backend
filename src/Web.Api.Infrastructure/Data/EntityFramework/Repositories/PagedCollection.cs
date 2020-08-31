using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Web.Api.Core;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    class PagedCollection<T, R> : IPagedCollection<T>
    {
        private readonly IQueryable<R> _query;
        private readonly IMapper _mapper;
        private readonly int _pageSize;
        private int _totalItems = -1;
        private int _totalPages = -1;
        public PagedCollection(IQueryable<R> query, IMapper mapper, int pageSize)
        {
            _query = query;
            _mapper = mapper;
            _pageSize = pageSize;
        }

        public IPagedCollection<T> FilterBy(string filterString)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetItemsForPage(int page)
        {
            var items = await _query
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<R>, IEnumerable<T>>(items);
        }

        public IPagedCollection<T> SortBy(string sortBy, string sortOrder)
        {
            throw new NotImplementedException();
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
