using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;

namespace AspNetCoreDemoApp.Controllers
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Confluent.Kafka;
    using Confluent.Kafka.Serialization;

    public class KafkaProducer
    {

        private string topic;
        private string message;

        public KafkaProducer(string topic, string message)
        {
            this.topic = topic;
            this.message = message;
        }

        public void publish()
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", Environment.GetEnvironmentVariable("KAFKA_URL").ToString().Replace("kafka+ssl://", "") },
                { "security.protocol", "ssl" },
                { "ssl.key.location", "/tmp/HEROKU_KAFKA_KEY.key" },
                { "ssl.certificate.location", "/tmp/HEROKU_KAFKA_CERT.cert" },
                { "ssl.ca.location", "/tmp/HEROKU_KAFKA_CA.ca" }
            };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                var dr = producer.ProduceAsync(this.topic, null, this.message).Result;
                Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
                return;
            }

            return;
        }
    }
}