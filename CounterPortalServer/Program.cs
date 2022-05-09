using System;
using System.Collections.Generic;
using CounterPortalServer.Classes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

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
                    if (players.FindAll(x => x.status == PlayerStatus.Ready).Count >= 4)
                    {
                        SessionInstance session = new SessionInstance();
                        for (int i = 0; i < 4; i++)
                        {
                            session.players.Add(players[0]);
                            players.Remove(players[0]);
                        }
                        session.seed = random.Next();
                        session.status = SessionStatus.StartGame;
                        Console.WriteLine("Starting game with seed: " + session.seed);
                        StartGame(session);
                    }
                    Thread.Sleep(500);
                }
            });
            matchmakingThread.Start();

            while (true)
            {
                Socket client = socket.Accept();
                Thread thread = new Thread(() =>
                {
                    byte[] data = new byte[1024];
                    int recv = client.Receive(data);
                    string json = Encoding.ASCII.GetString(data, 0, recv);
                    Player player = new Player();
                    player.Deserialize(json);
                    player.ClientSocket = client;
                    SessionInstance session = new SessionInstance();
                    session.status = SessionStatus.WaitingForPlayers;
                    Send(session.Serialize(), player.ClientSocket);
                    players.Add(player);
                    Console.WriteLine("Player " + player.Id + " connected");
                    
                    Thread recievePlayerState = new Thread(() =>
                    {
                        byte[] data = new byte[1024];
                        while (true)
                        {
                            try
                            {
                                int index = players.FindIndex(x => x.Id == player.Id);
                                players[index].ClientSocket.ReceiveTimeout = 120000;
                                int dataLength = players[index].ClientSocket.Receive(data);
                                Player playerInst = new Player();
                                playerInst.Deserialize(Encoding.ASCII.GetString(data, 0, dataLength));
                                playerInst.ClientSocket = players[index].ClientSocket;
                                players[index] = playerInst;
                            }
                            catch
                            {
                                break;
                            }
                            
                        }
                    });
                    recievePlayerState.Start();
                });
                thread.Start();
            }
        }

        static void Listen(string ip_address, int port)
        {
            IPAddress ip = IPAddress.Parse(ip_address);
            IPEndPoint ipe = new IPEndPoint(ip, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipe);
            socket.Listen(100);
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
            foreach (var player in session.players)
            {
                Send(session.Serialize(), player.ClientSocket);
            }
        }

        static void StartGame(SessionInstance session)
        {
            session.status = SessionStatus.StartGame;
            Cast(session);
            session.status = SessionStatus.StateUpdate;
            Console.WriteLine("Session started. State update casted.");

            for (int i = 0; i < session.players.Count; i++)
            {
                Thread recievePlayerState = new Thread(() =>
                {
                    byte[] data = new byte[1024];
                    while (true)
                    {
                        try
                        {
                            session.players[i].ClientSocket.ReceiveTimeout = 120000;
                            int dataLength = session.players[i].ClientSocket.Receive(data);
                            Player playerInst = new Player();
                            playerInst.Deserialize(Encoding.ASCII.GetString(data, 0, dataLength));
                            playerInst.ClientSocket = session.players[i].ClientSocket;
                            session.players[i] = playerInst;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            break;
                        }

                    }
                });
                recievePlayerState.Start();
            }

            Thread updateStateThread = new Thread(() =>
            {
                while (session.status == SessionStatus.StateUpdate)
                { 
                    int aliveCount = 0;
                    foreach (var player in session.players)
                    {
                        if (player.isAlive)
                        {
                            aliveCount++;
                        }
                    }
                    /*if (aliveCount < 2)
                    {
                        session.status = SessionStatus.EndGame;
                    }*/
                    Cast(session);
                    Thread.Sleep(17);
                }
            });
            updateStateThread.Start();
            //Thread receiveStateThread
        }
    }
}
