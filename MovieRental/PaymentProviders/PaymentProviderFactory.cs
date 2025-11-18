using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MovieRental.PaymentProviders
{
    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly IReadOnlyDictionary<PaymentProviderType, IPaymentProvider> _providers;

        public PaymentProviderFactory(IEnumerable<IPaymentProvider> providers)
        {
            _providers = providers.ToDictionary(provider => provider.Name);
        }

        public IPaymentProvider GetPaymentProviderByType(string name)
        {
            if(Enum.TryParse(name, out PaymentProviderType providerType))
            {
                return _providers[providerType];
            }

            return default; // TODO: /me Provider does not exist.
        }
    }
}
