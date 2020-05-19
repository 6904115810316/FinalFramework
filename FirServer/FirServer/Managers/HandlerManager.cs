﻿using System.Collections.Generic;
using log4net;
using FirServer.Handlers;
using FirServer.Interface;
using FirServer.Defines;
using LiteNetLib;
using System;

namespace FirServer.Managers
{
    public class HandlerManager : BaseBehaviour, IManager
    {
        static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(HandlerManager));
        Dictionary<string, IHandler> mHandlers = new Dictionary<string, IHandler>()
        {
            { Protocal.Default, new DefaultHandler() },
            { Protocal.Disconnect, new DisconnectHandler() },
        };

        /// <summary>
        /// 初始化消息处理器映射
        /// </summary>
        public void Initialize()
        {
            logger.Info("Initialize Success!!!");
        }

        /// <summary>
        /// 添加处理器
        /// </summary>
        public void AddHandler(string protocal, IHandler handler)
        {
            if (mHandlers.ContainsKey(protocal))
            {
                return;
            }
            mHandlers.Add(protocal, handler);
        }

        /// <summary>
        /// 添加处理器
        /// </summary>
        public IHandler GetHandler(string protocal)
        {
            if (!mHandlers.ContainsKey(protocal))
            {
                return null;
            }
            return mHandlers[protocal];
        }

        /// <summary>
        /// 移除处理器
        /// </summary>
        public void RemoveHandler(string protocal)
        {
            mHandlers.Remove(protocal);
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        public void OnRecvData(NetPeer peer, NetPacketReader reader)
        {
            var protoType = (ProtoType)reader.GetByte();
            var protoName = reader.GetString();
            logger.Info("OnRecvData[commandid]:" + protoType + " protoName:" + protoName);

            if (!mHandlers.ContainsKey(protoName))
            {
                protoName = Protocal.Default;
            }
            byte[] bytes = null;
            var count = reader.GetInt();
            reader.GetBytes(bytes, count);

            IHandler handler = null;
            if (mHandlers.TryGetValue(protoName, out handler))
            {
                try
                {
                    if (handler != null)
                    {
                        handler.OnMessage(peer, bytes);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

        public void OnDispose()
        {
        }
    }
}
