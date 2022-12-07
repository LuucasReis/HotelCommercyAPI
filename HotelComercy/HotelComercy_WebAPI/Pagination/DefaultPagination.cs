using System.Runtime.CompilerServices;

namespace HotelComercy_WebAPI.Pagination
{
    public class DefaultPagination
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 0;
        public int TotalPages { get; set; }
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
    }
}
