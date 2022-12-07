namespace HotelComercy_WebAPI.Pagination
{
    public class PagedInfo
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedInfo(int count,int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling((double)count / pageSize);
        }

    }
}
