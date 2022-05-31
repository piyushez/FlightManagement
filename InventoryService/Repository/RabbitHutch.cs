using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Repository
{
   
        public class RabbitHutch
        {
            private static ConnectionFactory _factory;
            private static IConnection _connection;
            private static IModel _channel;
            public static IBus CreateBus(string hostName)
            {
                _factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    DispatchConsumersAsync = true
                };
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                return new RabbitBus(_channel);
            }
            public static IBus CreateBus(
            string hostName,
            ushort hostPort,
            string virtualHost,
            string username,
            string password)
            {
                _factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    VirtualHost = "/",
                    UserName = "guest",
                    Password = "guest",
                    DispatchConsumersAsync = true
                };

                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                return new RabbitBus(_channel);
            }
        }
  
}
