using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public static class PagedList<T, Tdto>
    {
        public static PagedListDTO<Tdto> GetDTO(IQueryable<T> source, int pageNumber, int pageSize, Func<T, Tdto> convertToDTO)
        {
            int maxPageSize = Infrastructure.ValidationRules.MAX_PAGE_SIZE;
            pageSize = (pageSize > maxPageSize) ? maxPageSize : (pageSize < 1) ? 1 : pageSize;
            int totalCount = source.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            pageNumber = (pageNumber > totalPages) ? totalPages : (pageNumber < 1) ? 0 : pageNumber;
            bool hasPrevious = pageNumber > 1;
            bool hasNext = pageNumber < totalPages;
            source = source.Skip((pageNumber) * pageSize).Take(pageSize);
            var listDTO = new List<Tdto>();
            foreach (var user in source)
            {
                listDTO.Add(convertToDTO(user));
            }
            return new PagedListDTO<Tdto>(pageNumber, totalPages, totalCount, pageSize, hasPrevious, hasNext, listDTO);
        }
    }
}
