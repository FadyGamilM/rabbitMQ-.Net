using System;
using System.Text;
using RabbitMQ.Client;

//! (1) : Create a connection factory and give it the host name where our instance is running
var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = factory.CreateConnection();

//! (2) : Create the channel
using var channel = connection.CreateModel();

//! (3) : Declare the exchange we are going to use
channel.ExchangeDeclare(exchange: "headersExchange", type: ExchangeType.Headers);

//! (4) the message we want to publish
var msg = "a message for the header exchange";

//! (5) encode the message 
var encodedMsg = Encoding.UTF8.GetBytes(msg);

//! (6) publish the message, this message contains only 1 argument in the header so it will be routed to the queue who said "any"
var props = channel.CreateBasicProperties();
props.Headers = new Dictionary<string, object>{
   { "Email" , "admin@email.com" }
};
channel.BasicPublish(
   exchange: "headersExchange", 
   routingKey : "", 
   basicProperties: props, 
   body: encodedMsg
);
System.Console.WriteLine($"[PUBLISHED MESSAGE] => {msg}");

// ANOTHER MESSAGE TO THE QUEUE WHO SPECIFY "all"
//! (4) the message we want to publish
var msg2 = "another message for the header exchange";

//! (5) encode the message 
var encodedMsg2 = Encoding.UTF8.GetBytes(msg2);

//! (6) publish the message, this message contains only 1 argument in the header so it will be routed to the queue who said "any"
var props2 = channel.CreateBasicProperties();
props2.Headers = new Dictionary<string, object>{
   { "Email" , "admin@email.com" },
   { "Username", "admin" }
};
channel.BasicPublish(
   exchange: "headersExchange", 
   routingKey : "", 
   basicProperties: props2, 
   body: encodedMsg2
);
System.Console.WriteLine($"[PUBLISHED MESSAGE] => {msg2}");