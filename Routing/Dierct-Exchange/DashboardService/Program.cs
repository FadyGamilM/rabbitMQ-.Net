using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//! (1) : Create a connection factory and give it the host name where our instance is running
var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = factory.CreateConnection();

//! (2) : Create the channel
using var channel = connection.CreateModel();

//! (3) explicitly create an exchange 
channel.ExchangeDeclare(
   exchange: "routingExchange",
   type: ExchangeType.Direct
);

//! (4) : Declare the temporery queue for this specific consumer service and let rabbitmq give it a random name
var queueName = channel.QueueDeclare().QueueName;

//! (5) : Bind the queue with the exchange using the binding key 
channel.QueueBind(
   // the queue we wanna bind
   queue : queueName,
   // the exchange we wanna bind to
   exchange: "routingExchange",
   // the routing key 
   routingKey: "Analysis" // any thing related to Egypt branch 
);

//! (4) define the consumer which will consume the message
var consumer = new EventingBasicConsumer(channel);

//! (5) call the callback when we recieve the message
consumer.Received += (model, encodedMsg) => {
   // get the body of the msg
   var body = encodedMsg.Body.ToArray();
   // decode it
   var message = Encoding.UTF8.GetString(body); 
   System.Console.WriteLine($"[RECIEVED MESSAGE By Dashboard Microservice] => {message}");
};

//! (6) Consume the message
channel.BasicConsume(
   queue: queueName,
   autoAck: true,
   consumer: consumer
);

System.Console.WriteLine("Dashboard microservice starts consuming");

//! (7) exit 
Console.ReadKey();
