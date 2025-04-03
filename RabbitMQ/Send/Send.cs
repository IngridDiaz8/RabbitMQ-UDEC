using RabbitMQ.Client;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // Declaramos el exchange de tipo direct para poder filtrar por routing key
        await channel.ExchangeDeclareAsync(exchange: "gorutas_notificaciones", type: ExchangeType.Direct);

        Console.WriteLine("Sistema de Notificación de Conductor - Gorutas");
        Console.WriteLine("----------------------------------------------");
        
        while (true)
        {
            Console.WriteLine("\nIngrese el ID del padre de familia:");
            string padreId = Console.ReadLine();
            
            Console.WriteLine("\nSeleccione el estado del niño:");
            Console.WriteLine("1. Niño recogido");
            Console.WriteLine("2. Niño dejado en destino");
            Console.Write("Opción: ");
            string opcion = Console.ReadLine();
            
            string estado = opcion == "1" ? "RECOGIDO" : "DEJADO";
            
            var mensaje = $"Conductor notifica: Niño {estado} | PadreID:{padreId} | Fecha:{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            var body = Encoding.UTF8.GetBytes(mensaje);
            
            // Publicamos con routing key igual al ID del padre
            await channel.BasicPublishAsync(
                exchange: "gorutas_notificaciones",
                routingKey: padreId,
                body: body);
            
            Console.WriteLine($" [x] Notificación enviada: {mensaje}");
            
            Console.WriteLine("\n¿Desea enviar otra notificación? (s/n)");
            if (Console.ReadLine().ToLower() != "s") break;
        }
    }
}
