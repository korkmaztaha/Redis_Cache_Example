using StackExchange.Redis;
using System.Threading.Channels;

namespace Redis.Pub.Sub.Subscriber
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            ISubscriber subscriber = connection.GetSubscriber();

            //mychannel.* adında bir kanal pattern'ine (desenine) abone olunuyor.
            //Bu sayede mychannel.a, mychannel.b, mychannel.anything gibi kanal isimleriyle gönderilen tüm mesajlar bu abone tarafından dinlenebilir hale geliyor.
            await subscriber.SubscribeAsync("mychannel.*", (channel, message) =>
            {
                Console.WriteLine(message);
            });

            Console.Read();

            //Channel pattern yok
            //await subscriber.SubscribeAsync("mychannel", (channel, message) =>
            //{
            //    Console.WriteLine("Yeni kullanıcı kaydı: " + message);
            //});

        }
    }
}
