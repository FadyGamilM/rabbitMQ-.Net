# What we are trying to learn from this project ?
#### We will select some random processing time and make our consumer process the recieved message through this time to see the effect of the competing consumers pattern.

```CSharp
channel.BasicConsume(
   queue: "mailbox",
   // -> this property is true which means each message will be
   //    achknowledged once its recieved, but we will make it 
   //    false so each msg has to be achkowledged manually
   autoAck: true, 
   consumer: consumer
);
```