namespace MovieRental.Controllers.DTOs
{
    public record CustomerDTO
    {
        public int Id { get; init; }

        public required string Title { get; init; }
    }
}
