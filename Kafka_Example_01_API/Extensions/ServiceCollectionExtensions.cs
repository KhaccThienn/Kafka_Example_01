namespace Kafka_Example_01_API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseOracle(configuration.GetConnectionString("OrclDB"));
            });

            return services;
        }

        public static IServiceCollection AddSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<ProductMemory>();
            services.AddSingleton<IProductPersistenceService, ProductPersistenceService>();
            services.AddSingleton<IProductService, ProductService>();

            return services;
        }

        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<InsertProductCommand>, InsertProductCommandHandler>();
            services.AddTransient<ICommandHandler<UpdatePriceCommand>, UpdateProductPriceCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateQuantityCommand>, UpdateProductQuantityCommandHandler>();

            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services, AppSetting appSetting)
        {
            services.AddKafkaProducers(producerBuilder =>
            {
                producerBuilder.AddProducer(appSetting.GetProducerSetting("1"));
            });

            services.AddKafkaConsumers(builder =>
            {
                builder.AddConsumer<ProductConsumingTask>(appSetting.GetConsumerSetting("0"));
                builder.AddConsumer<ProductPersistanceConsumingTask>(appSetting.GetConsumerSetting("1"));
            });

            return services;
        }

        public static IServiceCollection AddMiscellaneousServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            return services;
        }

    }
}
