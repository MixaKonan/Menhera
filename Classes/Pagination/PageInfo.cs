using System;

namespace Menhera.Classes.Pagination
{
    public class PageInfo
    {
        public int PageNumber { get; } // номер текущей страницы
        public int PageSize { get; } // кол-во объектов на странице
        public int TotalItems { get; } // всего объектов
        
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }

        public PageInfo(int pageNumber, int pageSize, int totalItems)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }
    }
}