namespace MovieRental.PaymentProviders
{
    public interface IPaymentProvider
    {
        PaymentProviderType Name { get; }

        Task<bool> Pay(double price);
    }
}
