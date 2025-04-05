🚌 GoRutas - Arquitectura Publicador/Suscriptor con RabbitMQ
Este proyecto es una prueba de concepto para la implementación de un sistema de notificación entre conductores y padres de familia, utilizando la arquitectura de publicador-suscriptor con RabbitMQ.

📌 Descripción
La funcionalidad principal permite que un conductor envíe una notificación (por ejemplo, "recogido" o "dejado") al padre de familia correspondiente. Ambos se comunican a través de RabbitMQ, donde:

El padre de familia (subscriptor) se conecta y queda a la espera del mensaje.

El conductor (publicador) envía la notificación al padre, indicando su cédula para identificar el canal de comunicación.

🛠️ Tecnologías utilizadas
Lenguaje: C#

Mensajería: RabbitMQ 4.0 (en Docker)

Librerías utilizadas:

using RabbitMQ.Client; — para conectarse con RabbitMQ

using RabbitMQ.Client.Events; — para manejar eventos de consumo

using System.Text; — para convertir mensajes en texto

⚙️ Requisitos
Tener instalada la última versión de Docker.

Ejecutar RabbitMQ 4.0 desde Docker con el siguiente comando:

bash
Copiar
Editar
docker run -d --hostname rabbit-host --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.0-management
Tener instalado .NET Core SDK compatible con tu proyecto en C#.

🚀 ¿Cómo ejecutarlo?
Iniciar el subscriptor (padre de familia):

En consola, ejecutar el programa de recepción, indicando la cédula del padre:

bash
Copiar
Editar
dotnet run --project Receive -- 123456789
Iniciar el publicador (conductor):

En otra consola, ejecutar el programa de envío, indicando la cédula del padre y el mensaje:

bash
Copiar
Editar
dotnet run --project Send -- 123456789 "Recogido"
🔁 El mensaje será entregado directamente al padre correspondiente, gracias al sistema de colas que ofrece RabbitMQ.

🧪 Ejemplo de uso
Padre de familia: 123456789

El padre inicia su aplicación y queda esperando mensajes.

El conductor envía: "Recogido" a la cédula 123456789.

El padre ve en consola la notificación recibida.

👩‍💻 Autoría
Proyecto desarrollado por:

Laura Camila Mejía Mora

Ingrid Katherine Díaz Aranda

¡Gracias por visitar nuestro proyecto! 😄
