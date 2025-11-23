using MovieRental.Configuration.Exceptions;

namespace MovieRental.PaymentProviders
{
    public class PaymentProviderResolver : IPaymentProviderResolver
    {
        private readonly IReadOnlyDictionary<PaymentProviderType, IPaymentProvider> _providers;

        public PaymentProviderResolver(IEnumerable<IPaymentProvider> providers)
        {
            _providers = providers.ToDictionary(provider => provider.Name);
        }

        public IPaymentProvider GetPaymentProviderByName(string name)
        {
            if (Enum.TryParse(name, true, out PaymentProviderType providerType))
            {
                return _providers[providerType];
            }

            throw new PaymentProviderNotFoundException("Payment provider is not supported.");
        }
    }
}
