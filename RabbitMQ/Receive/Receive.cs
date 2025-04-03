using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Sistema de Notificación para Padres - Gorutas");
        Console.WriteLine("--------------------------------------------");
        
        Console.WriteLine("Ingrese su ID de padre de familia:");
        string padreId = Console.ReadLine();
        
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // Declaramos el exchange
        await channel.ExchangeDeclareAsync(exchange: "gorutas_notificaciones", type: ExchangeType.Direct);
        
        // Declaramos una cola temporal exclusiva para este padre
        var queueDeclareResult = await channel.QueueDeclareAsync();
        string queueName = queueDeclareResult.QueueName;
        
        // Enlazamos la cola al exchange usando el ID del padre como routing key
        await channel.QueueBindAsync(
            queue: queueName,
            exchange: "gorutas_notificaciones",
            routingKey: padreId);

        Console.WriteLine($" [*] Padre {padreId} esperando notificaciones...");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n [x] Nueva notificación recibida: {message}");
            Console.ResetColor();
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

        Console.WriteLine(" Presione [enter] para salir.");
        Console.ReadLine();
    }
}