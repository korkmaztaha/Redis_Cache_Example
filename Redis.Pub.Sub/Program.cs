using StackExchange.Redis;
namespace Redis.Pub.Sub
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
           ConnectionMultiplexer connection = await  ConnectionMultiplexer.ConnectAsync("localhost:6379");
            ISubscriber subscriber = connection.GetSubscriber();

            while (true)
            {
                Console.Write("Mesaj : ");
                string message = Console.ReadLine();
                await subscriber.PublishAsync("mychannel", message);
            }
        }
    }
}
