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

//! (4) the message we want to publish
var msg = "A new order is created !!";

//! (5) encode the message 
var encodedMsg = Encoding.UTF8.GetBytes(msg);

//! (6) publish the message to the fanout exchange
// TODO=> to be consumed by the Dashboard service and inventory service
channel.BasicPublish(exchange: "routingExchange", routingKey: "Analysis", null, encodedMsg);

// TODO=> to be consumed by the Inventory service only
channel.BasicPublish(exchange: "routingExchange", routingKey: "ProductsWhareHouse", null, encodedMsg);


System.Console.WriteLine($"[PUBLISHED MESSAGE] => {msg}");