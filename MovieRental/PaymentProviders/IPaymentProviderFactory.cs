namespace MovieRental.PaymentProviders
{
    public interface IPaymentProviderFactory
    {
        public IPaymentProvider GetPaymentProviderByType(string paymentProvider);
    }
}
