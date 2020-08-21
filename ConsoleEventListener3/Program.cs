namespace ConsoleEventListener3
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Automatonymous.Activities;
    using EventContracts;
    using MassTransit;

    public class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("event-listener3", e =>
                {
                    e.Consumer<EventConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        class EventConsumer :
            IConsumer<IOtherContract>
        {
            public async Task Consume(ConsumeContext<IOtherContract> context)
            {
                Console.WriteLine("Value: {0}, Number {1}",context.Message.OtherValue, context.Message.Number);
            }
        }
    }
}