using System;
using System.Text;
using RabbitMQ.Client;

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

//! (4) the message we want to publish
var msg = "my third message";

//! (5) encode the message 
var encodedMsg = Encoding.UTF8.GetBytes(msg);

//! (6) publish the message to the default exchange for this example
channel.BasicPublish("", "letterbox", null, encodedMsg);
//                             "" -> default exchange
//                             letterbox -> queue name

System.Console.WriteLine($"[PUBLISHED MESSAGE] => {msg}");