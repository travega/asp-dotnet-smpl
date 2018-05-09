using System.Threading.Tasks;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace AspNetCoreDemoApp.Controllers
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Confluent.Kafka;
    using Confluent.Kafka.Serialization;

    public class KafkaConsumer
    {
        private void startConsumer()
        {
            Console.WriteLine("GROUP ID: " + Environment.GetEnvironmentVariable("KAFKA_PREFIX") + Environment.GetEnvironmentVariable("TOPIC") + "-consumer-group");
            var conf = new Dictionary<string, object>
            {
                { "group.id", Environment.GetEnvironmentVariable("KAFKA_PREFIX") + Environment.GetEnvironmentVariable("TOPIC") + "-consumer-group" },
                { "bootstrap.servers", Environment.GetEnvironmentVariable("KAFKA_URL").ToString().Replace("kafka+ssl://", "") },
                { "auto.commit.interval.ms", 1000 },
                { "auto.offset.reset", "earliest" },
                { "enable.auto.commit", true},
                { "security.protocol", "ssl" },
                { "ssl.key.location", "/tmp/HEROKU_KAFKA_KEY.key" },
                { "ssl.certificate.location", "/tmp/HEROKU_KAFKA_CERT.cert" },
                { "ssl.ca.location", "/tmp/HEROKU_KAFKA_CA.ca" }
            };

            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.OnMessage += (_, msg)
                    =>
                {
                    Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");

                    var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGODB_URI"));
                    var db = mongoClient.GetDatabase("heroku_zrm8x9h7");

                    Post post = JsonConvert.DeserializeObject<Post>(msg.Value);

                    Console.WriteLine("DB connection: " + Environment.GetEnvironmentVariable("MONGODB_URI"));
                    db.GetCollection<Post>("Posts").InsertOne(post);
                };

                consumer.OnError += (_, error)
                    => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                    => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe(Environment.GetEnvironmentVariable("KAFKA_PREFIX") + Environment.GetEnvironmentVariable("TOPIC"));

                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                }
            }
        }

        public static Task<string> start() // No async because the method does not need await
        {
            KafkaConsumer kafkaConsumer = new KafkaConsumer();
            return Task.Run(() =>
            {
                kafkaConsumer.startConsumer();
                return "Kafka Consumer Started!";
            });
        }
    }
}