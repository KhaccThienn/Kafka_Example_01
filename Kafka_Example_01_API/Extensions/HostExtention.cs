namespace Kafka_Example_01_API.Extensions
{
    public static class HostExtention
    {
        public static IHost LoadDataToMemory<TInMemoryContext, TDbContext>(this IHost host, Action<TInMemoryContext, TDbContext> seeder)
            where TDbContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services    = scope.ServiceProvider;
                var context     = services.GetService<TInMemoryContext>();
                var dbContext   = services.GetRequiredService<TDbContext>();
                seeder(context, dbContext);
            }
            return host;
        }
    }
}
