﻿using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Library.Framework.Core.Utility
{
    public class RabbitMqHelper
    {
        /// <summary>
        /// 1：1对1模式
        /// 2：1对多模式：能者多劳
        /// 3：Exchange：订阅模式
        /// 4：Exchange：路由模式
        /// 5：Exchange： 通配符模式
        /// </summary>
        private readonly IModel _ch;
        private readonly IConnection _con;

        public delegate void Process(object model, BasicDeliverEventArgs ea);

        public RabbitMqHelper(string uri, string userName, string password,string virtualHost)
        {
            IConnectionFactory connectionFactory = new ConnectionFactory
            {
                RequestedHeartbeat = 0,
                Endpoint = new AmqpTcpEndpoint(new Uri(uri)),
                UserName = userName,
                Password = password,
                VirtualHost=virtualHost
            };
            _con = connectionFactory.CreateConnection();
            _ch = _con.CreateModel();
        }

        public void SendMessage(string queue, string exchange, string routingKey, byte[] bytes, int model)
        {

            if (model == 1 || model == 2)
            {
                _ch.QueueDeclare(queue, false, false, false, null);
                _ch.BasicPublish("", queue, null, bytes);
            }
            else if (model == 3)
            {
                _ch.ExchangeDeclare(exchange, "fanout");
                _ch.BasicPublish(exchange, "", null, bytes);
            }
            else if (model == 4 || model == 5)
            {
                _ch.ExchangeDeclare(exchange, model == 4 ? "direct" : "topic");
                _ch.BasicPublish(exchange, routingKey, null, bytes);
            }
        }

        public void ReadMessage(string queue, Process t, int model)
        {
            if (model == 1 || model == 2)
                _ch.QueueDeclare(queue, false, false, false, null);
            if (model == 3)
            {
                _ch.ExchangeDeclare(queue, "fanout");
                string q = queue + "_" + new Random().Next();
                _ch.QueueDeclare(q, false, false, false, null);
                _ch.QueueBind(q, queue, "");
                _ch.BasicQos(0, 1, false);
            }

            var consumer = new EventingBasicConsumer(_ch);
            if (model == 1)
                _ch.BasicConsume(queue, true, consumer);
            else if (model == 2 || model == 3)
                _ch.BasicConsume(queue, false, consumer);
            consumer.Received += (_model, ea) =>
            {

                t(_model, ea);
                if (model == 2 || model == 3)
                {
                    _ch?.BasicAck(ea.DeliveryTag, true);
                }
            };
        }

        public void ReadMessage(string exchange, string[] routingKeys, Process t, int model)
        {
            _ch.ExchangeDeclare(exchange, model == 4 ? "direct" : "topic");
            string queue = exchange + "_" + new Random().Next();
            _ch.QueueDeclare(queue, false, false, false, null);
            foreach (var v in routingKeys)
            {
                _ch.QueueBind(queue, exchange, v);
            }

            _ch.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(_ch);
            _ch.BasicConsume(queue, false, consumer);
            consumer.Received += (_model, ea) =>
            {
                t(_model, ea);
                _ch.BasicAck(ea.DeliveryTag, true);
            };
        }

        public void Close()
        {
            _con.Close();
        }

        public void DeleteQueue(string queue) {
            _ch.QueueDelete(queue);
        }

        public void DeleteExchange(string exchange) {
            _ch.ExchangeDelete(exchange);
        }
    }
}
