﻿using System;
using System.Threading;
using CommandLine;
using Fleck;
using Leopotam.Ecs;
using ProtoBuf;
using Serilog;
using TheDwarvenHalls.Server.Database;
using TheDwarvenHalls.Server.Mechanics;
using TheDwarvenHalls.Shared.Messages;

namespace TheDwarvenHalls.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Arguments>(args)
                .WithParsed<Arguments>(o =>
                {
                    // Set up the logger
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .WriteTo.LiteDB(Context.DatabasePath)
                        .CreateLogger();
                    Log.Information("Logger is set up");
            
                    // WebSocket server logger
                    FleckLog.LogAction = (level, message, ex) => {
                        switch(level) {
                            case LogLevel.Debug:
                                Log.Debug(message, ex);
                                break;
                            case LogLevel.Error:
                                Log.Error(message, ex);
                                break;
                            case LogLevel.Warn:
                                Log.Warning(message, ex);
                                break;
                            default:
                                Log.Information(message, ex);
                                break;
                        }
                    };

                    // Starting the world
                    var context = new Context();
                    using (var world = new World(context))
                    {
                        string currentInput = String.Empty;
                        while (true)  
                        {  
                            if (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo key = Console.ReadKey(true);
                                if (key.Key == ConsoleKey.Enter)
                                {
                                    if (currentInput == "exit")
                                        break;
                                    
                                    currentInput = String.Empty;
                                    Console.WriteLine();
                                }
                                else
                                {
                                    currentInput += key.KeyChar;
                                    Console.Write(key.KeyChar);   
                                }
                            }  
                            
                            world.Run();
                        }
                    }
                });
        }
    }
}