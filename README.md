
# EasyRedisMQ
A Simple .Net Message Queue that Uses Redis for the back end.

Introduction
------------

EasyRedisMQ is designed to be a light weight message queue system that is built on top of redis.  It is completely written in C#.

It is not trying to compete with full featured message queue systems such as RabbitMQ. However, it also has more features built in than the basic redis pub/sub implementation. If you don't need a full blown message queue system such as RabbitMQ but the built in pub/sub features of redis are not enough, then EasyRedisMQ may be the right solution for you.

EasyRedisMQ Conventions
----------------
EasyRedisMQ relies on class types to handle publishing and subscribing to messages. If you publish a message of type T then any programs subscribing to messages of type T will receive that message.

**Asynchronous**
EasyRedisMQ is fully built to take advantage of asynchronous processing within .Net. All methods implement the async/await keywords.  Since the inherit nature of communicating with a redis database is an asynchronous process, there are no synchronous method implementations within EasyRedisMQ.

**Exchanges**
Exchanges are like post offices. They handle the routing of messages from publishers to subscribers. Publishers push messages to an exchange and subscribers subscribe to an exchange. Publishers do not know about the subscribers and subscribers do not know about the publishers. An exchange name is established using the following convention:
```cs
var exchangeName = string.Format("Exchange.{0}", typeof(T).FullName);
```

**Subscribers**
Subscribers subscribe to a specific message type T. When subscribing to a message type T the subscriber must provider a subscriberId. Each *unique* subscriberId for each exchange is stored in a redis set. The convention for storing the subscribers to an exchange is:
```cs
var exchangeSubscribersKey = string.Format("{0}.Subscribers", exchangeName);
```

**Queues**
Queues are stored as a list in redis.  The list will only show up in redis if there are objects in the queue, otherwise the redis key is deleted and then recreated once items are added. Objects on the queue are serialized to JSON using the ultra-fast Jil JSON (De)Serializer. Each unique subscriberId will get its own queue. If multiple applications use the same subscriberId then only one queue will be created. Queues names are established using the following convention:
```cs
var subscriberQueueName = string.Format("{0}.Queue.{1}", exchangeName, subscriberId);
```

**Publishers**
Publishers publishes messages to an exchange. The exchange is determined by the type T of the message. When the message is published, the EasyRedisMQ exchange will push a copy of the message onto the appropriate queue for each subscriberId. Then the exchange will broadcast to all subscribers that a new message has arrived using redis's built in pub/sub functionality.



Getting Started
---------------
Install the nuget package in your project
>PM> Install-Package EasyRedisMQ

Add the following section to your app.config and replace the appropriate values

```xml
    <configSections>
        <section name="redisCacheClient" type="StackExchange.Redis.Extensions.Core.Configuration.RedisCachingSectionHandler, StackExchange.Redis.Extensions.Core"/>
    </configSections>
    <redisCacheClient allowAdmin="true" ssl="false" connectTimeout="5000" database="0">
        <hosts>
            <add host="127.0.0.1" cachePort="6379"/>
        </hosts>
    </redisCacheClient>
```

Configure your dependency injection framework to provide Singletons for the following Interaces and Classes:

 1. ISerializer -- JilSerializer -- Singleton
 2. ICacheClient -- StackExchangeRedisCacheClient -- Singleton
 3. IMessageBroker -- MessageBroker -- Singleton

Here is how to do it using StructureMap
```cs
For<ISerializer>().Singleton().Use(c => new JilSerializer());
For<ICacheClientExtended>().Singleton().Use(c => new StackExchangeRedisCacheClient(c.GetInstance<ISerializer>(), null));
For<IMessageBroker>().Singleton().Use<MessageBroker>();
```

Publishing a Message
--------------------

EasyRedisMQ makes publishing messages easy.  
```cs
var messageBroker = container.GetInstance<IMessageBroker>(); //StructureMap Implementation
messageBroker.PublishAsync(myObjectToBePublished);
```
*****NOTE: If there are no subscribers to an exchange, the message will not be published anywhere and will simply disappear.**

Subscribing to an Exchange
--------------------------
Subscribing to an exchange is just as easy with EasyRedisMQ. In order to subscribe to an exchange, you need 3 things:

 1. The message type you want to subscribe to
 2. A subscriberId
		 a. Each unique subscriberId will get its own queue and its own copy of every message published.
		 b. If multiple applications subscribe using the same subscriberId then they will each be notified via redis's pub/sub functionality and the message will be delivered on a first come first serve basis. 
 3. An asynchronous message handler with the following signature: Func< T, Task> onMessageAsync
```cs
var messageBroker = container.GetInstance<IMessageBroker>();
var subscriber = messageBroker.SubscribeAsync<ConsoleMessage>("EasyRedisMQ.Consumer", async x => { await WriteConsoleMessageAsync(x); });
```

Sample Applications
-------------------
That's it for the basic introductions. The EasyRedisMQ GitHub repo has some very basic sample applications that you can download and play around with to become more familiar with EasyRedisMQ. 

Go Play!

Dependencies
------------
EasyRedisMQ uses the following dependencies:

> 1. [StackExchange.Redis.Extensions.Jil](https://github.com/imperugo/StackExchange.Redis.Extensions)

> 2. [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)

> 3. [Jil](https://github.com/kevin-montrose/Jil)


Inspiration
-----------
A major source of inspiration for this project is [EasyNetQ](https://github.com/EasyNetQ/EasyNetQ)



> Written with [StackEdit](https://stackedit.io/).
