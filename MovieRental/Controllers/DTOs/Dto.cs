namespace MovieRental.Controllers.DTOs
{
    public record MovieResponseDto(int Id, string Title);

    public record RentalResponseDto(int Id, int CustomerId, int MovieId, int DaysRented, string PaymentMethod, string CustomerName, string MovieTitle);
}
