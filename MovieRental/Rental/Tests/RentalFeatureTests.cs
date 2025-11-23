using Moq;
using MovieRental.Configuration.Exceptions;
using MovieRental.Controllers.DTOs;
using MovieRental.PaymentProviders;
using MovieRental.Rental.Repositories;

namespace MovieRental.Rental.Tests
{
    [TestClass]
    public class RentalFeatureTests
    {
        private Mock<IRentalRepository> _rentalRepositoryMock;
        private Mock<IPaymentProviderResolver> _paymentProviderResolverMock;
        private Mock<IPaymentProvider> _paymentProviderMock;
        private RentalFeatures _feature;

        [TestInitialize]
        public void Initialize()
        {
            _rentalRepositoryMock = new Mock<IRentalRepository>();
            _paymentProviderResolverMock = new Mock<IPaymentProviderResolver>();
            _paymentProviderMock = new Mock<IPaymentProvider>();

            _paymentProviderResolverMock
                .Setup(r => r.GetPaymentProviderByName(It.IsAny<string>()))
                .Returns(_paymentProviderMock.Object);

            _feature = new RentalFeatures(
                _rentalRepositoryMock.Object,
                _paymentProviderResolverMock.Object
            );
        }


        [TestMethod]
        public async Task SaveAsync_ValidData_ShouldSaveAndReturn()
        {
            // Arrange
            var rental = new Rental
            {
                PaymentMethod = "paypal",
                DaysRented = 3
            };

            _paymentProviderMock
                .Setup(p => p.Pay(It.IsAny<double>()))
                .ReturnsAsync(true);

            var savedRental = new Rental { Id = 1, PaymentMethod = "paypal", CustomerId = 1, MovieId = 1 };

            _rentalRepositoryMock
                .Setup(r => r.SaveAsync(rental, It.IsAny<CancellationToken>()))
                .ReturnsAsync(savedRental);

            // Act
            var result = await _feature.SaveAsync(rental);

            // Assert
            Assert.AreEqual(savedRental, result);

            _paymentProviderResolverMock.Verify(r =>
                r.GetPaymentProviderByName("paypal"), Times.Once);

            _paymentProviderMock.Verify(p =>
                p.Pay(It.IsAny<double>()), Times.Once);

            _rentalRepositoryMock.Verify(r =>
                r.SaveAsync(rental, It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        public async Task SaveAsync_PaymentFails_ShouldThrowPaymentFailedException()
        {
            // Arrange
            var rental = new Rental
            {
                PaymentMethod = "mastercard",
                DaysRented = 5
            };

            _paymentProviderMock
                .Setup(p => p.Pay(It.IsAny<double>()))
                .ReturnsAsync(false);

            // Act + Assert
            await Assert.ThrowsAsync<PaymentFailedException>(() =>
                _feature.SaveAsync(rental));

            _rentalRepositoryMock.Verify(r =>
                r.SaveAsync(It.IsAny<Rental>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }


        [TestMethod]
        public async Task GetRentalsByCustomerNameAsync_InvalidInput_ShouldThrowException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _feature.GetRentalsByCustomerNameAsync("   "));
        }

        [TestMethod]
        public async Task GetRentalsByCustomerNameAsync_ValidRequest_ShouldReturn()
        {
            // Arrange
            string name = "tiago";
            int page = 1;
            int pageSize = 10;

            var rentals = new List<RentalResponseDto>
        {
            new RentalResponseDto(1, 1, 1, 2, "paypal", name, "The Godfather")
        };

            var expected = new PagedResult<RentalResponseDto>(rentals, 1, 10, 1);

            _rentalRepositoryMock
                .Setup(r => r.GetRentalsByCustomerNameAsync(name, page, pageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _feature.GetRentalsByCustomerNameAsync(name, page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual(name, result.Result.First().CustomerName);

            _rentalRepositoryMock.Verify(r =>
                r.GetRentalsByCustomerNameAsync(name, page, pageSize, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
