using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//! (1) : Create a connection factory and give it the host name where our instance is running
var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = factory.CreateConnection();

//! (2) : Create the channel
using var channel = connection.CreateModel();

//! (3) : Declare the queue we are going to use
channel.QueueDeclare(
   queue: "letterbox", // queue name
   durable: false,
   exclusive: false,
   autoDelete: false,
   arguments: null
);

//! (4) define the consumer which will consume the message
var consumer = new EventingBasicConsumer(channel);

//! (5) call the callback when we recieve the message
consumer.Received += (model, encodedMsg) => {
   // get the body of the msg
   var body = encodedMsg.Body.ToArray();
   // decode it
   var message = Encoding.UTF8.GetString(body); 
   System.Console.WriteLine($"[RECIEVED MESSAGE] => {message}");
};

//! (6) Consume the message
channel.BasicConsume(
   queue: "letterbox",
   autoAck: true,
   consumer: consumer
);

//! (7) exit 
Console.ReadKey();
