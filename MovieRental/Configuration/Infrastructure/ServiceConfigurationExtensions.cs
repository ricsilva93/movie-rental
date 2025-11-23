using MovieRental.PaymentProviders;

namespace MovieRental.Configuration.Infrastructure
{
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection ConfigurePaymentProviders(this IServiceCollection services)
        {
            services.AddTransient<IPaymentProvider, PayPalProvider>();
            services.AddTransient<IPaymentProvider, MbWayProvider>();
            services.AddTransient<IPaymentProviderResolver, PaymentProviderResolver>();

            return services;
        }
    }
}
