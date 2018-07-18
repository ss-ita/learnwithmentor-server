using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public static class PagedList<T>
    {
        public static PagedListDTO<T> GetDTO (List<T> source, int pageNumber, int pageSize)
        {
            int maxPageSize = ValidationRules.MAX_PAGE_SIZE;            
            pageSize = (pageSize > maxPageSize) ? maxPageSize : (pageSize < 1) ? 1 : pageSize;
            int totalCount = source.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            pageNumber = (pageNumber > totalPages) ? totalPages : (pageNumber < 1) ? 0 : pageNumber;
            bool hasPrevious = pageNumber > 1;
            bool hasNext = pageNumber < totalPages;
            var items = source.Skip((pageNumber) * pageSize).Take(pageSize).ToList();
            return new PagedListDTO<T>(pageNumber, totalPages, totalCount, pageSize, hasPrevious, hasNext, items);
        }
    }
}
