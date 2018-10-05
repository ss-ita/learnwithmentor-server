using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public static class PagedList<T, TDto>
    {
        public async static Task<PagedListDto<TDto>> GetDTO(IQueryable<T> source, int pageNumber, int pageSize, Func<T, Task<TDto>> convertToDTO)
        {
            var maxPageSize = Infrastructure.ValidationRules.MAX_PAGE_SIZE;
            pageSize = (pageSize > maxPageSize) ? maxPageSize : (pageSize < 1) ? 1 : pageSize;
            var totalCount = source.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            pageNumber = (pageNumber > totalPages) ? totalPages : (pageNumber < 1) ? 0 : pageNumber;
            var hasPrevious = pageNumber > 1;
            var hasNext = pageNumber < totalPages;
            source = source.Skip((pageNumber) * pageSize).Take(pageSize);
            var listDTO = new List<TDto>();
            foreach (var user in source)
            {
                listDTO.Add(await convertToDTO(user));
            }
            return new PagedListDto<TDto>(pageNumber, totalPages, totalCount, pageSize, hasPrevious, hasNext, listDTO);
        }
    }
}
