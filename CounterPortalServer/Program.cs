using System;
using System.Collections.Generic;
using CounterPortalServer.Classes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;

namespace CounterPortalServer
{
    internal class Program
    {
        static List<Player> players = new List<Player>();
        private static Socket socket;

        private static Random random = new Random();

        static void Main(string[] args)
        {
            string ip_address = Console.ReadLine();
                int port = int.Parse(Console.ReadLine());
            Listen(ip_address, port);
            Thread matchmakingThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (players.Count >= 4)
                        {
                            SessionInstance session = new SessionInstance();
                            for (int i = 0; i < 4; i++)
                            {
                                session.players.Add(FindPlayer());
                            }
                            session.seed = random.Next();
                            Console.WriteLine("Starting game with seed: " + session.seed);
                            StartGame(session);
                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            });
            matchmakingThread.Start();

            while (true)
            {
                Socket client = socket.Accept();
                Thread thread = new Thread(() =>
                {
                    byte[] data = new byte[4096];
                    int recv = client.Receive(data);
                    string json = Encoding.ASCII.GetString(data, 0, recv);
                    Player player = new Player();
                    player.Deserialize(json);
                    player.ClientSocket = client;
                    SessionInstance session = new SessionInstance();
                    session.status = SessionStatus.WaitingForPlayers;
                    Send(Serialize(session), player.ClientSocket);
                    players.Add(player);
                    Console.WriteLine("Player " + player.Id + " connected");
                });
                thread.Start();
            }
        }
        
        static Player FindPlayer()
        {
            Player player = players[0];
            if (player.ClientSocket.Connected)
            {
                players.Remove(player);
                return player;
            }
            else
            {
                players.Remove(player);
                return FindPlayer();
            }
        }

        static void Listen(string ip_address, int port)
        {
            IPAddress ip = IPAddress.Parse(ip_address);
            IPEndPoint ipe = new IPEndPoint(ip, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipe);
            socket.Listen(100);
            Console.WriteLine("Listening on " + ip_address + ":" + port);
        }

        static void Send(string message, Socket client)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                client.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        static void Cast(SessionInstance session)
        {
            SessionInstance nm = session.DeepCopy();
            string message = Serialize(nm);
            foreach (var player in nm.players)
            {
                Send(message, player.ClientSocket);
            }
        }
        
        public static string Serialize(SessionInstance session)
        {
            return JsonConvert.SerializeObject(session);
        }

        static void StartGame(SessionInstance session)
        {
            session.status = SessionStatus.StartGame;
            Cast(session);
            session.status = SessionStatus.StateUpdate;
            Console.WriteLine("Session started. State update casted.");
            
            for (int i = 0; i <= session.players.Count - 1; i++)
            {
                Thread recievePlayerState = new Thread((object index) =>
                {
                    int _index = (int)index;
                    byte[] data = new byte[4096];
                    Console.WriteLine($"Started thread for player {session.players[_index].Id}");
                    while (true)
                    {
                        try
                        {
                            session.players[_index].ClientSocket.ReceiveTimeout = 120000;

                            lock (session.players[_index].ClientSocket)
                            {
                                int dataLength = session.players[_index].ClientSocket.Receive(data);
                                Player playerInst = new Player();
                                string json = Encoding.ASCII.GetString(data, 0, dataLength);
                                playerInst.Deserialize(json);
                                playerInst.ClientSocket = session.players[_index].ClientSocket;
                                session.players[_index] = playerInst;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            break;
                        }

                    }
                    Console.WriteLine($"Thread for player {session.players[_index].Id} finished");
                });
                recievePlayerState.Start(i);
            }
            

            Thread updateStateThread = new Thread(() =>
            {
                while (session.status == SessionStatus.StateUpdate)
                { 
                    int aliveCount = 0;
                    SessionInstance nm = session.DeepCopy();
                    foreach (var player in nm.players)
                    {
                        if (player.isAlive)
                        {
                            aliveCount++;
                        }
                    }
                    if (aliveCount < 2)
                    {
                        session.status = SessionStatus.EndGame;
                    }
                    lock (session)
                    {
                        Cast(session);
                    }
                    Thread.Sleep(5);
                }
            });
            updateStateThread.Start();
        }
    }
}
