using Manonero.MessageBus.Kafka.Settings;

namespace Kafka_Example_01_API.Settings
{
    public class AppSetting
    {
        public string BootstrapServers { get; init; }
        public ConsumerSetting[] ConsumerSettings { get; init; }
        public ProducerSetting[] ProducerSettings { get; init; }

        public static AppSetting MapValues(IConfiguration configuration)
        {
            var bootstrapServers = configuration[nameof(BootstrapServers)];

            var consumerConfigurations = configuration.GetSection(nameof(ConsumerSettings)).GetChildren();
            var consumerSettings = new List<ConsumerSetting>();

            var producerConfigurations = configuration.GetSection(nameof(ProducerSettings)).GetChildren();
            var producerSettings = new List<ProducerSetting>();

            foreach (var producerConfiguration in producerConfigurations)
            {
                var producerSetting = ProducerSetting.MapValue(producerConfiguration, bootstrapServers);
                if (!producerSettings.Contains(producerSetting)) producerSettings.Add(producerSetting);
            }

            foreach (var consumerConfiguration in consumerConfigurations)
            {
                var consumerSetting = ConsumerSetting.MapValue(consumerConfiguration, bootstrapServers);
                if (!consumerSettings.Contains(consumerSetting)) consumerSettings.Add(consumerSetting);
            }
            var setting = new AppSetting
            {
                BootstrapServers = bootstrapServers,
                ConsumerSettings = consumerSettings.ToArray(),
                ProducerSettings = producerSettings.ToArray()
            };

            return setting;
        }

        public ConsumerSetting GetConsumerSetting(string id)
            => ConsumerSettings.FirstOrDefault(consumerSetting => consumerSetting.Id.Equals(id));

        public ProducerSetting GetProducerSetting(string id)
           => ProducerSettings.FirstOrDefault(producerSetting => producerSetting.Id.Equals(id));
    }
}
