// the producer doesn't have to declare a queue anymore, because each consumer will have its own queue
using System;
using System.Text;
using RabbitMQ.Client;

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

channel.ExchangeDeclare(
   exchange: "TopicRoutingExchange",
   type: ExchangeType.Topic
);

//! (4) the message we want to publish
var msg1 = "A new order is created !!";
//! (5) encode the message 
var encodedMsg1 = Encoding.UTF8.GetBytes(msg1);
//! (6) publish the message to both the dierct and topic exchange
channel.BasicPublish(exchange: "TopicRoutingExchange", routingKey: "admin.Analysis.egypt", null, encodedMsg1);
System.Console.WriteLine($"[PUBLISHED MESSAGE] => {msg1}");

//! (4) the message we want to publish
var msg2 = "A new shipment is coming to egypt's store today";
//! (5) encode the message 
var encodedMsg2 = Encoding.UTF8.GetBytes(msg2);
channel.BasicPublish(exchange: "routingExchange", routingKey: "ProductsWhareHouse", null, encodedMsg2);
System.Console.WriteLine($"[PUBLISHED MESSAGE] => {msg2}");


//! (4) the message we want to publish
var msg3 = "fady gamil create a new complaint today";
//! (5) encode the message 
var encodedMsg3 = Encoding.UTF8.GetBytes(msg3);
//! (6) publish the message to both the dierct and topic exchange
channel.BasicPublish(exchange: "TopicRoutingExchange", routingKey: "user.egypt.complaint", null, encodedMsg3);
System.Console.WriteLine($"[PUBLISHED MESSAGE] => {msg3}");
