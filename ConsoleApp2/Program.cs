using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var projectId = "376558168506";

            AuthImplicit(projectId);

            SubscriberServiceApiClient subsClient = SubscriberServiceApiClient.Create();
            SubscriptionName subsName = new SubscriptionName(projectId, "westworldSubscription");
            SubscriberClient einstein =await SubscriberClient.CreateAsync(subsName);
            bool acknowledge = false;
            await einstein.StartAsync(
                async (PubsubMessage pubSubMessage, CancellationToken cancel) =>
                {
                    string msg = Encoding.UTF8.GetString(pubSubMessage.Data.ToArray());

                 
                    await Console.Out.WriteLineAsync($"{pubSubMessage.MessageId}: {msg}");

                    if (msg.Length > 1) acknowledge = true;
                    return acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack;
                });
            Thread.Sleep(5000);
            einstein.StopAsync(CancellationToken.None).Wait();
        }

        public static void AuthImplicit(string projectId)
        {
    
            var credential = GoogleCredential.GetApplicationDefault();
            var storage = StorageClient.Create(credential);
           
         
        }

      
    }
}
