using MovieRental.Movie.Repositories;
using MovieRental.PaymentProviders;
using MovieRental.Rental.Repositories;

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

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            return services;
        }
    }
}
