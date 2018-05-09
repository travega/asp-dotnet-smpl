using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemoApp.Controllers
{
	[Route("api/[controller]")]
	public class KafkaController : ControllerBase
	{
		// GET: api/kafka
		[HttpGet]
		public IEnumerable<string> Get()
		{
		    KafkaSsl.setup();
			return new[] { "value1", "value2" };
		}

	    [Route("consumer/start")]
	    [HttpPost]
	    public string Post()
	    {
	        KafkaSsl.setup();
	        KafkaConsumer.start();
	        return "Kafka consumer started";
	    }

		// GET api/kafka/<some message>
		[HttpGet("{message}")]
		public string Get(string message)
		{
		    KafkaSsl.setup();
		    string topic = Environment.GetEnvironmentVariable("KAFKA_PREFIX") + Environment.GetEnvironmentVariable("TOPIC");
		    KafkaProducer kafkaProducer = new KafkaProducer(topic, message);
		    kafkaProducer.publish();

			return "The message: '" + message + "', has been published to the 'chatter' topic";
		}
	}
}