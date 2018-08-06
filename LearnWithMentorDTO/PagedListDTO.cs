using System.Collections.Generic;

namespace LearnWithMentorDTO
{
    public class PagedListDTO<T>
    {
        public PagedListDTO(int pageNumber, int totalPages, int totalCount, int pageSize, bool hasPrevious, bool hasNext, List<T> items)
        {
            PageNumber = pageNumber;
            TotalPages = totalPages;
            TotalCount = totalCount;
            PageSize = pageSize;
            HasPrevious = hasPrevious;
            HasNext = hasNext;
            Items = items;
        }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; } 
        public List<T> Items { get; set; }
    }
}
