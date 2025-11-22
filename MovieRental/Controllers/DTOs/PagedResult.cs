namespace MovieRental.Controllers.Dtos
{
    public record PagedResult<T> //todo /me change
    {
        public PagedResult(IEnumerable<T> result, int page, int pageSize, int total)
        {
            this.Result = result;
            this.PageNumber = page;
            this.Total = total;
            this.TotalPages = pageSize > 0 ? (int)Math.Ceiling((double)total / pageSize) : 1;
        }

        public IEnumerable<T> Result { get; init; } = Enumerable.Empty<T>();

        public int PageNumber { get; init; } = 1;

        public int TotalPages { get; init; } = 1;

        public int Total { get; init; } = 0;
    }
}
