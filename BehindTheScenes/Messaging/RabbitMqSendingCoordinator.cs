﻿using System;
using System.Linq;
using System.Threading;

using BehindTheScenes.Core.RabbitMq;
using BehindTheScenes.Extensions;

using Microsoft.Practices.ObjectBuilder2;

namespace BehindTheScenes.Messaging
{
    public interface IRabbitMqSendingCoordinator
    {
        void SendMany(int howMany);
    }

    public class RabbitMqSendingCoordinator : IRabbitMqSendingCoordinator
    {
        private readonly IRabbitMqChannelOperator _channelOperator;

        public RabbitMqSendingCoordinator(IRabbitMqChannelOperator channelOperator)
        {
            _channelOperator = channelOperator;
        }

        public void SendMany(int howMany)
            => Enumerable.Range(1, howMany)
                         .Select(i => $"Hello World! Message {i} at {DateTime.UtcNow.SignificantTicks()}")
                         .ForEach(msg =>
                         {
                             _channelOperator.PublishMessage(msg);
                             msg.PrintNiceMessage("SENT");
                             Thread.Sleep(1000);
                         });
    }
}