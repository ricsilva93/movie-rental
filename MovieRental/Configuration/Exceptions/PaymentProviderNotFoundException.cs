namespace MovieRental.Configuration.Exceptions
{
    public class PaymentProviderNotFoundException : Exception
    {
        public PaymentProviderNotFoundException(string? message) : base(message)
        {
        }
    }
}
