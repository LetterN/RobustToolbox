﻿using System;
using System.Net;
using Lidgren.Network;
using Robust.Shared.Interfaces.Network;

namespace Robust.Shared.Network
{
    /// <summary>
    ///     A network connection from this local peer to a remote peer.
    /// </summary>
    internal class NetChannel : INetChannel
    {
        private readonly NetManager _manager;
        private readonly NetConnection _connection;

        /// <inheritdoc />
        public long ConnectionId => _connection.RemoteUniqueIdentifier;

        /// <inheritdoc />
        public INetManager NetPeer => _manager;

        public string UserName { get; }
        public LoginType AuthType { get; }

        /// <inheritdoc />
        public short Ping => (short) Math.Round(_connection.AverageRoundtripTime * 1000);

        /// <inheritdoc />
        public bool IsConnected => _connection.Status == NetConnectionStatus.Connected;

        /// <inheritdoc />
        public IPEndPoint RemoteEndPoint => _connection.RemoteEndPoint;

        /// <summary>
        ///     Exposes the lidgren connection.
        /// </summary>
        public NetConnection Connection => _connection;

        public NetUserId UserId { get; }

        // Only used on server, contains the encryption to use for this channel.
        public NetEncryption? Encryption { get; set; }

        /// <summary>
        ///     Creates a new instance of a NetChannel.
        /// </summary>
        /// <param name="manager">The server this channel belongs to.</param>
        /// <param name="connection">The raw NetConnection to the remote peer.</param>
        internal NetChannel(NetManager manager, NetConnection connection, NetUserId userId, string userName, LoginType loginType)
        {
            _manager = manager;
            _connection = connection;
            UserId = userId;
            UserName = userName;
            AuthType = loginType;
        }

        /// <inheritdoc />
        public T CreateNetMessage<T>()
            where T : NetMessage
        {
            return _manager.CreateNetMessage<T>();
        }

        /// <inheritdoc />
        public void SendMessage(NetMessage message)
        {
            if (_manager.IsClient)
            {
                _manager.ClientSendMessage(message);
                return;
            }

            _manager.ServerSendMessage(message, this);
        }

        /// <inheritdoc />
        public void Disconnect(string reason)
        {
            if (_connection.Status == NetConnectionStatus.Connected)
                _connection.Disconnect(reason);
        }
    }
}
