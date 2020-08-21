# MassTransitTryOut
MassTransit 3 listeners, 1 publisher, 2 different contracts, RabbitMQ, console applications .net Core

Needs a RabbitMq Broker to work

See : https://masstransit-project.com/getting-started/

Container for broker : $ docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq

Start up the 3 listeners (ConsoleEventListener, ConsoleEventListener2, ConsoleEventListener3) and the publisher ConsoleEventPublisher

Type any string in the publisher : it is published with contract IValueEntered in two listeners : ConsoleEventListener, ConsoleEventListener2

Type any string that starts with "abc" in the publisher : : it is published with contract IOtherContract in the third listner : ConsoleEventListener3 .
