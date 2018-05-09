using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreDemoApp.Controllers
{
    public class KafkaSsl
    {
        public static void setup()
        {
            // Cert file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("/tmp","HEROKU_KAFKA_CERT.cert"), true)) {
                outputFile.WriteLine(Environment.GetEnvironmentVariable("KAFKA_CLIENT_CERT"));
            }

            // Cert Authority file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("/tmp","HEROKU_KAFKA_CA.ca"), true)) {
                outputFile.WriteLine(Environment.GetEnvironmentVariable("KAFKA_TRUSTED_CERT"));
            }

            // Cert Key file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("/tmp","HEROKU_KAFKA_KEY.key"), true)) {
                outputFile.WriteLine(Environment.GetEnvironmentVariable("KAFKA_CLIENT_CERT_KEY"));
            }
        }
    }
}