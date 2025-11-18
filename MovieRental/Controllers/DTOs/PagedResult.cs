namespace MovieRental.Controllers.Dtos
{
    public class PagedResult<T>
    {
        public T? Result { get; set; }

        public int PageNumber {  get; set; }

        public int TotalPages { get; set; }

        public int Total { get; set; }
    }
}
