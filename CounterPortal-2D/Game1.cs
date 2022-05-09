using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterPortal_2D.Classes;
using CounterPortal_2D.Classes.UI;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Text;
using System;
using System.Threading;

namespace CounterPortal_2D
{
    public enum GameState
    {
        Menu, Game, End, Pause, Connect, Reset, Exit, Awaiting
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static GameState _gameState = GameState.Menu;
        private MapGenerator _mapGenerator;

        private Player _player;
        public static List<Player> _opponents;
        private Aim _aim;

        private Texture2D _background;
        public static List<Wall> _walls = new List<Wall>();

        Menu _menu;

        private int borderWidth = 10;

        public static int screenWidth;
        public static int screenHeight;
        private int seed;

        public static string ip;
        public static int port;
        public Socket socket;

        private Thread stateSender;
        private Thread stateReceiver;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           /* _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;*/
        }

        protected override void Initialize()
        {
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            _player = new Player();
            _aim = new Aim();
            _menu = new Menu();
            base.Initialize();
            stateReceiver = new Thread(() =>
            {
                while (true)
                {
                    SessionInstance session = new SessionInstance();
                    byte[] data = new byte[1024];
                    socket.Receive(data);
                    string json = Encoding.ASCII.GetString(data);
                    session.Deserialize(json);
                    switch (session.status)
                    {
                        case SessionStatus.StartGame:
                            _gameState = GameState.Game;
                            session.players.Remove(session.players.Find(x => x.Id == _player.Id));
                            _opponents = session.players;
                            foreach (var player in _opponents)
                            {
                                player.LoadContent(Content);
                            }
                            break;
                        case SessionStatus.WaitingForPlayers:
                            _gameState = GameState.Awaiting;
                            break;
                        case SessionStatus.EndGame:
                            _gameState = GameState.End;
                            break;
                        case SessionStatus.StateUpdate:
                            session.players.Remove(session.players.Find(x => x.Id == _player.Id));
                            _opponents = session.players;
                            break;
                        default:
                            break;
                    }
                }
            });
            stateSender = new Thread(() =>
            {
                while (true)
                {
                    byte[] data = Encoding.ASCII.GetBytes(_player.Serialize());
                    socket.Send(data);
                    Thread.Sleep(17);
                }
            });
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //_background = Content.Load<Texture2D>("background");
            Wall.texture = new Texture2D(GraphicsDevice, 1, 1);
            Wall.texture.SetData(new[] { Color.White });

            _player.LoadContent(Content);
            _aim.LoadContent(Content);
            _menu.LoadContent(Content);
        }

        private static byte[] data;
        protected override void Update(GameTime gameTime)
        {
            _aim.Update(gameTime);
            
            switch (_gameState)
            {
                case GameState.Menu:
                    _menu.Update();
                    break;
                case GameState.Game:
                    
                    UpdateGame();
                    break;
                case GameState.Pause:
                    break;
                case GameState.Connect:
                    Connect connect = new Connect();
                    if (connect.ShowDialog() == DialogResult.OK)
                    {
                        _gameState = GameState.Menu;
                        ip = connect.IP;
                        port = connect.Port;
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(ip, port);
                        _player.Id = Guid.NewGuid().ToString();
                        data = Encoding.ASCII.GetBytes(_player.Serialize());
                        socket.Send(data);
                        stateReceiver.Start();
                        Console.WriteLine("Connected to: " + ip);
                    }
                    else 
                    {
                        System.Windows.Forms.MessageBox.Show("Error connecting to server. \nData may be incorrect.");
                        _gameState = GameState.Menu;
                    }
                    break;
                case GameState.Reset:
                    Reset();
                    break;
                case GameState.Awaiting:
                    _player.status = PlayerStatus.Ready;
                    data = Encoding.ASCII.GetBytes(_player.Serialize());
                    socket.Send(data);
                    Console.WriteLine("Waiting for game to start...");
                    while (_gameState != GameState.Game)
                    {

                    }
                    stateSender.Start();
                    Console.WriteLine("Game started!");
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch (_gameState)
            {
                case GameState.Menu:
                    _menu.Draw(_spriteBatch);
                    break;
                case GameState.Game:
                    DrawGame();
                    break;
                case GameState.Pause:
                    break;
                case GameState.Connect:
                    break;
            }
            _aim.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            foreach (var wall in _walls)
            {
                wall.Draw(_spriteBatch);
            }
            foreach (var player in _opponents)
            {
                player.Draw(_spriteBatch);
            }
            _player.Draw(_spriteBatch);
        }

        private void UpdateGame()
        {
            _player.Update();
            foreach (var player in _opponents)
            {
                player.Update();
            }
        }

        private void Reset()
        {
            _mapGenerator = new MapGenerator(seed, borderWidth);
            _walls = _mapGenerator.GenerateMap();
            _gameState = GameState.Awaiting;
        }
    }
}
