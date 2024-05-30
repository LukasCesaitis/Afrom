using MongoDB.Driver;

namespace Kvadratai.Extensions
{
    /// <summary>
    /// Extension methods for setting up MongoDB services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class MongoDbExtension
    {
        /// <summary>
        /// Adds MongoDB services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> used to retrieve connection string and database name.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var mongoDatabase = mongoClient.GetDatabase(configuration["DatabaseName"]);
            services.AddScoped(provider => mongoDatabase);
            return services;
        }
    }
}
