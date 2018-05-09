using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AspNetCoreDemoApp.Controllers
{

    public partial class Post
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("event")]
        public Event Event { get; set; }

        [JsonProperty("sobject")]
        public Sobject Sobject { get; set; }
    }

    public partial class Event
    {
        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Sobject
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

    public partial class Post
    {
        public static Post FromJson(string json) => JsonConvert.DeserializeObject<Post>(json, AspNetCoreDemoApp.Controllers.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Post self) => JsonConvert.SerializeObject(self, AspNetCoreDemoApp.Controllers.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}