namespace MovieRental.PaymentProviders
{
    public interface IPaymentProviderResolver
    {
        public IPaymentProvider GetPaymentProviderByName(string paymentProvider);
    }
}
