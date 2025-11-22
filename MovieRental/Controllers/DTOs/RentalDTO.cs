using MovieRental.Controllers.Dtos;

namespace MovieRental.Controllers.DTOs
{
    public record RentalDTO
    {
        public RentalDTO(int id, int daysRented, string paymentMethod)
        {
            this.Id = id;
            this.DaysRented = daysRented;
            this.PaymentMethod = paymentMethod;
        }

        public int Id { get; init; }

        //public MovieDTO Movie { get; init; }

        //public CustomerDTO Customer { get; init; }

        public int DaysRented { get; init; }

        public string PaymentMethod { get; init; }
    }
}
