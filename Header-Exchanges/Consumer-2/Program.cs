using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//! (1) : Create a connection factory and give it the host name where our instance is running
var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = factory.CreateConnection();

//! (2) : Create the channel
using var channel = connection.CreateModel();

//! (3) : Declare the exchange we are going to use
channel.ExchangeDeclare(exchange: "headersExchange", type: ExchangeType.Headers);

//! (4) : Declare the queue 
var queue = channel.QueueDeclare(queue: "appAdminQueue");

//! (5) : binding the queue to the exchange
var bindingArgs = new Dictionary<string, object>{
   {"x-match", "all"},
   { "Email" , "admin@email.com" },
   { "Username", "admin" }
};

channel.QueueBind(
   queue: queue.QueueName,
   exchange: "headersExchange",
   routingKey: "",
   arguments: bindingArgs
);

//! (6) define the consumer which will consume the message
var consumer = new EventingBasicConsumer(channel);

//! (7) call the callback when we recieve the message
consumer.Received += (model, encodedMsg) => {
   // get the body of the msg
   var body = encodedMsg.Body.ToArray();
   // decode it
   var message = Encoding.UTF8.GetString(body); 
   System.Console.WriteLine($"[RECIEVED MESSAGE] => {message}");
};

//! (8) Consume the message
channel.BasicConsume(
   queue: queue.QueueName,
   autoAck: true,
   consumer: consumer
);

//! (9) exit 
Console.ReadKey();
