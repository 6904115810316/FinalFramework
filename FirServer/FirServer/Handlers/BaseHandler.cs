﻿using FirServer.Interface;
using LiteNetLib;
using ProtoBuf;
using System.IO;

namespace FirServer.Handlers
{
    public abstract class BaseHandler : BaseBehaviour, IHandler
    {
        public abstract void OnMessage(NetPeer peer, byte[] bytes);

        public T DeSerialize<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }
    }
}
