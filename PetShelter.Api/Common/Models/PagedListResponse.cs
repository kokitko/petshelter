using PetShelter.Application.Common.Models;

namespace PetShelter.Api.Common.Models;

public class PagedListResponse<T>
{
    public List<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    public PagedListResponse(
        List<T> items, 
        int pageNumber, 
        int totalPages, 
        int totalCount, 
        bool hasPreviousPage, 
        bool hasNextPage)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = totalPages;
        TotalCount = totalCount;
        HasPreviousPage = hasPreviousPage;
        HasNextPage = hasNextPage;
    }
}