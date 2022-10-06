using System;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

var random = new Random();

//! (1) : Create a connection factory and give it the host name where our instance is running
var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = factory.CreateConnection();

//! (2) : Create the channel
using var channel = connection.CreateModel();

//! (3) : Declare the queue we are going to use
channel.QueueDeclare(
   queue: "mailbox", // queue name
   durable: false,
   exclusive: false,
   autoDelete: false,
   arguments: null
);

//! ==> set the prefetch count to 1 
// Qos = Quality Of Service
channel.BasicQos(
   prefetchSize: 0, 
   prefetchCount: 1, 
   global: false
);

//! (4) define the consumer which will consume the message
var consumer = new EventingBasicConsumer(channel);
//! (5) call the callback when we recieve the message
consumer.Received += (model, encodedMsg) => {
   // add ranodm processing time between 1 and 5 secs
   var consumerProcessingTime = random.Next(1, 6);
    
   // get the body of the msg
   var body = encodedMsg.Body.ToArray();

   // decode the message
   var message = Encoding.UTF8.GetString(body); 

   // just for logging info
   Console.WriteLine($"[RECIEVED MESSAGE] => {message} takes {consumerProcessingTime} secs of processing");

   // delay the processing 
   Task.Delay(TimeSpan.FromSeconds(consumerProcessingTime)).Wait();

   // achknowledge the message
   channel.BasicAck(
      deliveryTag: encodedMsg.DeliveryTag, // indicates which msg we are processing and achkownledge
      multiple: false // send single ack
   ); 
};

//! (6) Consume the message
channel.BasicConsume(
   queue: "mailbox",
   autoAck: false, // each msg will be manualy achnowledged
   consumer: consumer
);

//! (7) exit 
Console.ReadKey();
