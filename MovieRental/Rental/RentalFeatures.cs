using MovieRental.Configuration.Exceptions;
using MovieRental.Configuration.Validation;
using MovieRental.Controllers.DTOs;
using MovieRental.PaymentProviders;
using MovieRental.Rental.Repositories;

namespace MovieRental.Rental
{
    public class RentalFeatures : IRentalFeatures
    {
        private readonly IRentalRepository _rentalRepository;

        private readonly IPaymentProviderResolver _paymentProviderFactory;
        public RentalFeatures(
            IRentalRepository rentalRepository,
            IPaymentProviderResolver paymentProviderFactory)
        {
            _rentalRepository = rentalRepository;
            _paymentProviderFactory = paymentProviderFactory;
        }

        //TODO: make me async :(
        public async Task<Rental> SaveAsync(Rental rental, CancellationToken cancellationToken = default)
        {
            var paymentProvider = _paymentProviderFactory.GetPaymentProviderByName(rental.PaymentMethod);

            var price = new Random().NextDouble() * rental.DaysRented;
            var paymentSucceeded = await paymentProvider.Pay(price);

            if (!paymentSucceeded)
            {
                throw new PaymentFailedException("Payment failed.");
            }

            return await _rentalRepository.SaveAsync(rental, cancellationToken);
        }

        //TODO: finish this method and create an endpoint for it
        public async Task<PagedResult<RentalResponseDto>> GetRentalsByCustomerNameAsync(
            string customerName,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new ArgumentException("Customer Name cannot be empty.");
            }

            (page, pageSize) = PaginationValidator.Normalize(page, pageSize);

            return await _rentalRepository.GetRentalsByCustomerNameAsync(customerName, page, pageSize, cancellationToken);
        }
    }
}
