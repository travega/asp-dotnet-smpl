using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AspNetCoreDemoApp.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : ControllerBase
	{
		// GET: api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
		    var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGODB_URI"));
		    var db = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_NAME"));

		    List<Post> posts = db.GetCollection<Post>("Posts").Find(FilterDefinition<Post>.Empty).ToList();

		    List<string> res = new List<string>();

		    foreach (Post post in posts)
		    {
		        res.Add(post.ToJson().ToString());
		    }

			return res.ToArray();
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}
	}
}