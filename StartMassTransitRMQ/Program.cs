namespace EventContracts
{
    public interface IValueEntered
    {
        string Value { get; }
    }

    public interface IOtherContract
    {
        int Number { get; }
        string OtherValue { get; }
    }
}

namespace ConsoleEventPublisher
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventContracts;
    using MassTransit;

    public class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                do
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    if (value.StartsWith("abc"))
                    {
                        await busControl.Publish<IOtherContract>(new
                        {
                            OtherValue = "abc",
                            Number = 23
                        });

                    }
                    else
                    {
                        await busControl.Publish<IValueEntered>(new
                        {
                            Value = value
                        });
                    }



                }
                while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}