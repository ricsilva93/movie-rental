namespace MovieRental.Configuration.Validation
{
    public static class PaginationValidator
    {
        private const int maxPageSize = 100;

        public static (int Page, int PageSize) Normalize(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > maxPageSize) pageSize = maxPageSize;
            return (page, pageSize);
        }
    }
}
