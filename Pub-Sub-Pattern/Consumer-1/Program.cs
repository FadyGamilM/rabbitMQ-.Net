using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//! (1) : Create a connection factory and give it the host name where our instance is running
var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = factory.CreateConnection();

//! (2) : Create the channel
using var channel1 = connection.CreateModel();

//! (3) explicitly create an exchange 
channel1.ExchangeDeclare(
   exchange: "PubSub",
   type: ExchangeType.Fanout
);


//! (4) : Declare the temporery queue for this specific consumer service and let rabbitmq give it a random name
var queueName = channel1.QueueDeclare().QueueName;

//! (5) define the consumer which will consume the message
var consumer = new EventingBasicConsumer(channel1);

//! (6) now we need to bind our queue to the declared exchange
channel1.QueueBind(queue: queueName, exchange: "PubSub", routingKey: "");

//! (7) call the callback when we recieve the message
consumer.Received += (model, encodedMsg) => {
   // get the body of the msg
   var body = encodedMsg.Body.ToArray();
   // decode it
   var message = Encoding.UTF8.GetString(body); 
   System.Console.WriteLine($"[RECIEVED MESSAGE By First Consumer] => {message}");
};

//! (8) Consume the message
channel1.BasicConsume(
   queue: queueName,
   autoAck: true,
   consumer: consumer
);

System.Console.WriteLine("First Consumer starts consuming");

//! (9) exit only if recieved a key from user 
Console.ReadKey();
