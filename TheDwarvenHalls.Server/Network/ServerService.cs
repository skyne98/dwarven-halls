using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using Fleck;
using Leopotam.Ecs;
using Serilog;
using TheDwarvenHalls.Server.Events.Network;
using TheDwarvenHalls.Server.Services;
using TheDwarvenHalls.Shared.Messages;
using TheDwarvenHalls.Shared.Messages.Network;

namespace TheDwarvenHalls.Server.Network
{
    public class ServerService: IEcsSystem, IEcsInitSystem, IEcsRunSystem
    {
        private NetworkConfiguration _configuration;
        private WebSocketServer _server;
        private List<IWebSocketConnection> _connections;
        private Dictionary<string, IWebSocketConnection> _players;
        private ChannelWriter<NetworkEvent> _eventsWriter;
        private ChannelReader<NetworkRequest> _requestsReader;

        public Channel<NetworkEvent> Events { get; private set; }
        public Channel<NetworkRequest> Requests { get; private set; }
        
        public ServerService(ConfigurationService configurationService)
        {
            _configuration = configurationService.Configuration.Network;
        }

        public void Initialize()
        {
            _connections = new List<IWebSocketConnection>();
            _players = new Dictionary<string, IWebSocketConnection>();

            // Create the channels
            Events = Channel.CreateUnbounded<NetworkEvent>(new UnboundedChannelOptions()
            {
                SingleReader = false,
                SingleWriter = true
            });
            _eventsWriter = Events.Writer;
            Requests = Channel.CreateUnbounded<NetworkRequest>(new UnboundedChannelOptions()
            {
                SingleReader = true,
                SingleWriter = false
            });
            _requestsReader = Requests.Reader;
            
            // Set the server up
            var location = _configuration.Port.HasValue
                ? $"ws://0.0.0.0:{_configuration.Port.Value}"
                : $"ws://0.0.0.0:8181";
            _server = new WebSocketServer(location);
            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    _connections.Add(socket);
                    Log.Information($"Opened connection to {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort}");
                };
                socket.OnClose = () =>
                {
                    _connections.Remove(socket);
                    if (_players.ContainsValue(socket))
                    {
                        var name = _players.First(player => player.Value == socket).Key;
                        _players.Remove(name);
                        Log.Information($"Closed connection to {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} for the player {name}");

                        _eventsWriter.TryWrite(new PlayerDisconnectedEvent()
                        {
                            Name = name
                        });
                    }
                    else
                    {
                        Log.Information($"Closed connection to {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort}");   
                    }
                };
                socket.OnBinary = binaryMessage =>
                {
                    var message = Message.Deserialize(binaryMessage);
                    switch (message)
                    {
                        case AuthenticationMessage authenticationMessage:
                            if (_configuration.Password == authenticationMessage.Password.Trim())
                            {
                                if (_players.ContainsKey(authenticationMessage.Name))
                                {
                                    // Player is already connected, replace him
                                    var oldSocket = _players[authenticationMessage.Name];
                                    oldSocket.Close();
                                    _players[authenticationMessage.Name] = socket;
                                    Log.Information($"Player {authenticationMessage.Name} has reconnected, old connection closed");
                                }
                                else
                                {
                                    // Player is not yet connected
                                    _players.Add(authenticationMessage.Name, socket);
                                    Log.Information($"Player {authenticationMessage.Name} has connected");
                                    
                                    _eventsWriter.TryWrite(new PlayerConnectedEvent()
                                    {
                                        Name = authenticationMessage.Name
                                    });
                                }
                            }
                            break;
                    }
                };
            });
        }

        public void Destroy()
        {
            _server.Dispose();
        }

        public void Run()
        {
            // Fast loop around available requests
            while (_requestsReader.TryRead(out var request))
            {
                // TODO: Handle requests
            }
        }
    }
}