using System.Text;
using RabbitMQ.Client;

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



//! => infinite loop, we keep sending msgs each x-sec to the broker
//! => give the msg an id
var msgNumber = 1;
while(true){
   //! => publishing time, faster the the consumer processing time
   var publishingTIme = random.Next(1, 4); // 1 to 3 secs

   //! (4) the message we want to publish
   var msg = $"Message Number {msgNumber}";

   //! (5) encode the message 
   var encodedMsg = Encoding.UTF8.GetBytes(msg);

   //! (6) publish the message to the default exchange for this example
   channel.BasicPublish("", "mailbox", null, encodedMsg);
   //                             "" -> default exchange
   //                             mailbox -> queue name

   Console.WriteLine($"[PUBLISHED MESSAGE] => {msg}");

   //! => delay the next publishing 
   Task.Delay(TimeSpan.FromSeconds(publishingTIme)).Wait();

   // increase the msg id
   msgNumber ++;
}
